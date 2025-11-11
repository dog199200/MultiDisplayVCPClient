using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Logging;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    public partial class VcpConnectionConfigurator : UserControl
    {
        public VcpConnectionConfigurator()
        {
            InitializeComponent();
        }

        public Dictionary<string, string> Settings
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
                MacroDeckLogger.Info(PluginInstance.Main, $"Getting settings from VcpConnectionConfigurator: Name={txtName.Text}");
                return settings;
            }
            set
            {
                // --- THIS IS THE FIX ---
                // If the settings are null (new row), set defaults
                if (value == null)
                {
                    MacroDeckLogger.Info(PluginInstance.Main, "Setting default values for new VcpConnectionConfigurator row.");
                    txtName.Text = "";
                    txtIpAddress.Text = "127.0.0.1";
                    txtPort.Text = "21000";
                    txtPassword.Text = "1234";
                }
                else // Otherwise, load the saved settings
                {
                    value.TryGetValue("name", out var name);
                    value.TryGetValue("ipAddress", out var ip);
                    value.TryGetValue("port", out var port);
                    value.TryGetValue("password", out var pass);

                    txtName.Text = name;
                    txtIpAddress.Text = ip ?? "127.0.0.1";
                    txtPort.Text = port ?? "21000";
                    txtPassword.Text = pass ?? "1234";

                    MacroDeckLogger.Info(PluginInstance.Main, $"Setting loaded settings in VcpConnectionConfigurator: Name={name}");
                }
            }
        }
    }
}