using SuchByte.MacroDeck.Logging;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace MultiDisplayVCPClient.GUI
{
    public partial class VcpConnectionToggler : UserControl
    {
        private VcpClient _client;
        public VcpClient Client => _client;

        private readonly Image _refreshIcon;
        private readonly Image _refreshingIcon;

        private readonly Color _colorRed = Color.Maroon;
        private readonly Color _colorGrey = Color.FromArgb(65, 65, 65);
        private readonly Color _colorWhite = Color.White;

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

        // --- THIS IS THE FIX ---
        // Renamed FetchAndCacheAsync to FetchAndUpdateVariablesAsync
        private async void OnRefreshClick(object sender, EventArgs e)
        {
            SetControlsEnabled(false, isRefreshing: true);
            pbRefresh.Image = _refreshingIcon;

            try
            {
                // This is the "slow" fetch
                await PluginInstance.Main.FetchAndUpdateVariablesAsync(_client); // <-- RENAMED
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Warning(PluginInstance.Main, $"({_client.Name}) Manual refresh failed: {ex.Message}");
            }
            finally
            {
                this.InvokeIfRequired(() => {
                    pbRefresh.Image = _refreshIcon;
                    UpdateUI(_client.State);
                });
            }
        }
        // --- END FIX ---

        private void OnConnectionStateChanged(object sender, ConnectionState newState)
        {
            this.InvokeIfRequired(() => UpdateUI(newState));
        }

        private async void OnToggleClick(object sender, EventArgs e)
        {
            if (_client.State == ConnectionState.Connecting) return;

            if (_client.State == ConnectionState.Connected)
            {
                _client.Disconnect();
            }
            else // It's Offline
            {
                // This is the "fast" connect + "slow" background fetch
                await PluginInstance.Main.ConnectAndFetchInBackground(_client);
            }
        }

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
    }
}