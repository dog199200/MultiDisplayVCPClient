using System;
using System.Drawing;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    /// <summary>
    /// A user control that displays the state of a VcpClient
    /// and provides buttons to connect/disconnect and refresh.
    /// </summary>
    public partial class VcpConnectionToggler : UserControl
    {
        private readonly VcpClient _client;

        /// <summary>
        /// Gets the VcpClient associated with this control.
        /// </summary>
        public VcpClient Client => _client;

        private readonly Image _refreshIcon;
        private readonly Image _refreshingIcon;

        private readonly Color _colorRed = Color.Maroon;
        private readonly Color _colorGrey = Color.FromArgb(65, 65, 65);
        private readonly Color _colorWhite = Color.White;

        /// <summary>
        /// Initializes a new instance of the VcpConnectionToggler control.
        /// </summary>
        /// <param name="client">The VcpClient to bind to this control.</param>
        public VcpConnectionToggler(VcpClient client)
        {
            InitializeComponent();
            _client = client;

            _refreshIcon = Properties.Resources.MCC_Refresh;
            _refreshingIcon = Properties.Resources.MCC_Refreshing;
            pbRefresh.Image = _refreshIcon;

            lblName.Text = _client.Name;
            btnToggle.ForeColor = _colorWhite;

            _client.ConnectionStateChanged += OnConnectionStateChanged;
            btnToggle.Click += OnToggleClick;
            pbRefresh.Click += OnRefreshClick;

            UpdateUI(_client.State);
        }

        /// <summary>
        /// Handles the Click event for the 'Refresh' button.
        /// Manually triggers a full data fetch from the server.
        /// </summary>
        private async void OnRefreshClick(object? sender, EventArgs e)
        {
            SetControlsEnabled(false, isRefreshing: true);
            pbRefresh.Image = _refreshingIcon;

            try
            {
                await PluginInstance.Main.FetchAndUpdateVariablesAsync(_client);
            }
            catch (Exception)
            {
                // Fail silently
            }
            finally
            {
                this.InvokeIfRequired(() =>
                {
                    pbRefresh.Image = _refreshIcon;
                    UpdateUI(_client.State);
                });
            }
        }

        /// <summary>
        /// Handles the ConnectionStateChanged event from the VcpClient.
        /// </summary>
        private void OnConnectionStateChanged(object? sender, ConnectionState newState)
        {
            this.InvokeIfRequired(() => UpdateUI(newState));
        }

        /// <summary>
        /// Handles the Click event for the 'Connect' / 'Disconnect' button.
        /// </summary>
        private async void OnToggleClick(object? sender, EventArgs e)
        {
            if (_client.State == ConnectionState.Connecting) return;

            if (_client.State == ConnectionState.Connected)
            {
                _client.Disconnect();
            }
            else
            {
                await PluginInstance.Main.ConnectAndFetchInBackground(_client);
            }
        }

        /// <summary>
        /// Enables or disables the UI controls based on the client's state.
        /// </summary>
        private void SetControlsEnabled(bool isEnabled, bool isConnecting = false, bool isRefreshing = false)
        {
            btnToggle.Enabled = isEnabled;
            pbRefresh.Enabled = (isEnabled && !isConnecting && !isRefreshing);

            if (!isEnabled)
            {
                btnToggle.UseWindowsAccentColor = false;
                btnToggle.BackColor = _colorGrey;
                btnToggle.ForeColor = _colorWhite;
            }
        }

        /// <summary>
        /// Updates all UI elements (labels, buttons) to reflect the given connection state.
        /// </summary>
        /// <param name="state">The new ConnectionState to display.</param>
        private void UpdateUI(ConnectionState state)
        {
            switch (state)
            {
                case ConnectionState.Connected:
                    lblStatus.Text = "Connected";
                    lblStatus.ForeColor = Color.LimeGreen;
                    btnToggle.Text = "Disconnect";
                    btnToggle.UseWindowsAccentColor = false;
                    btnToggle.BackColor = _colorRed;
                    btnToggle.ForeColor = _colorWhite;
                    SetControlsEnabled(true);
                    break;

                case ConnectionState.Offline:
                    lblStatus.Text = "Offline";
                    lblStatus.ForeColor = Color.Gray;
                    btnToggle.Text = "Connect";
                    btnToggle.UseWindowsAccentColor = true;
                    btnToggle.ForeColor = _colorWhite;
                    SetControlsEnabled(true);
                    pbRefresh.Enabled = false;
                    break;

                case ConnectionState.Connecting:
                    lblStatus.Text = "Connecting...";
                    lblStatus.ForeColor = Color.DarkGray;
                    btnToggle.Text = "Connect";
                    SetControlsEnabled(false, isConnecting: true);
                    pbRefresh.Image = _refreshingIcon;
                    break;
            }
        }

        /// <summary>
        /// Compares this instance to another object for equality.
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is VcpConnectionToggler toggler &&
                   EqualityComparer<VcpClient>.Default.Equals(_client, toggler._client);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(_client);
        }
    }
}