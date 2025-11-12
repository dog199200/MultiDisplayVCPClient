using System.Collections.Generic;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    /// <summary>
    /// A user control that provides text boxes for configuring a single VCP server connection.
    /// </summary>
    public partial class VcpConnectionConfigurator : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the VcpConnectionConfigurator control.
        /// </summary>
        public VcpConnectionConfigurator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the connection settings from the UI controls.
        /// When setting, this populates the text boxes.
        /// When getting, it reads the values from the text boxes.
        /// </summary>
        public Dictionary<string, string>? Settings
        {
            get
            {
                var settings = new Dictionary<string, string>
                {
                    { "name", txtName.Text },
                    { "ipAddress", txtIpAddress.Text },
                    { "port", txtPort.Text },
                    { "password", txtPassword.Text }
                };
                return settings;
            }
            set
            {
                if (value == null)
                {
                    txtName.Text = "";
                    txtIpAddress.Text = "127.0.0.1";
                    txtPort.Text = "21000";
                    txtPassword.Text = "1234";
                }
                else
                {
                    value.TryGetValue("name", out var name);
                    value.TryGetValue("ipAddress", out var ip);
                    value.TryGetValue("port", out var port);
                    value.TryGetValue("password", out var pass);

                    txtName.Text = name;
                    txtIpAddress.Text = ip ?? "127.0.0.1";
                    txtPort.Text = port ?? "21000";
                    txtPassword.Text = pass ?? "1234";
                }
            }
        }
    }
}