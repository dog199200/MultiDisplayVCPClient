using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Language;

namespace MultiDisplayVCPClient.GUI
{
    /// <summary>
    /// A pop-up dialog form for editing a single VCP connection.
    /// </summary>
    public partial class ConnectionEditorForm : DialogForm
    {
        /// <summary>
        /// Gets the connection settings from the configurator control.
        /// </summary>
        public Dictionary<string, string>? Settings => _configurator.Settings;

        public ConnectionEditorForm(Dictionary<string, string>? settings)
        {
            InitializeComponent();
            btnOk.Text = LanguageManager.Strings.Ok;

            // Pass the settings (either existing or null for new)
            // to the VcpConnectionConfigurator control.
            _configurator.Settings = settings;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            // Validate that a name is entered
            if (string.IsNullOrWhiteSpace(_configurator.Settings?["name"]))
            {
                System.Windows.Forms.MessageBox.Show("A connection name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}