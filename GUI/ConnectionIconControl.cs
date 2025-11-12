using System.Drawing;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    /// <summary>
    /// A control that visually represents a single VCP connection in the config grid.
    /// </summary>
    public partial class ConnectionIconControl : UserControl
    {
        private Dictionary<string, string>? _settings;

        /// <summary>
        /// Fires when the 'X' delete button is clicked.
        /// </summary>
        public event EventHandler? DeleteClicked;

        /// <summary>
        /// Fires when the main icon/button is clicked to edit.
        /// </summary>
        public event EventHandler? EditClicked;

        /// <summary>
        /// Gets or sets the connection settings associated with this control.
        /// </summary>
        public Dictionary<string, string>? Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                if (_settings != null && _settings.TryGetValue("name", out var name))
                {
                    this.lblName.Text = name;
                }
                else
                {
                    this.lblName.Text = "Unnamed";
                }
            }
        }

        public ConnectionIconControl()
        {
            InitializeComponent();
            this.btnDelete.Click += (sender, args) => DeleteClicked?.Invoke(this, args);
            this.btnIcon.Click += (sender, args) => EditClicked?.Invoke(this, args);
            this.lblName.Click += (sender, args) => EditClicked?.Invoke(this, args);
        }

        /// <summary>
        /// Updates the icon to be online (green) or offline (red).
        /// </summary>
        public void SetOnlineStatus(bool isOnline)
        {
            this.InvokeIfRequired(() =>
            {
                btnIcon.Image = isOnline
                    ? Properties.Resources.MCC_Online
                    : Properties.Resources.MCC_Offline;
            });
        }
    }
}