using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Language;
using SuchByte.MacroDeck.Logging;

using SuchByte.MacroDeck.Plugins;

// ... existing using statements ...
using SuchByte.MacroDeck.Variables;
using System.Text.RegularExpressions;

namespace MultiDisplayVCPClient.GUI
{
    public partial class PluginConfig : DialogForm
    {
        private readonly List<string> _originalConnectionNames;

        public PluginConfig()
        {
            InitializeComponent();

            btnOk.Text = LanguageManager.Strings.Ok;

            btnCleanUp.Click += BtnCleanUp_Click;

            MacroDeckLogger.Info(PluginInstance.Main, "PluginConfig window loading...");

            _originalConnectionNames = PluginInstance.Main.Connections.Keys.ToList();

            LoadCredentials();
        }

        private void LoadCredentials()
        {
            try
            {
                List<Dictionary<string, string>> credentials = PluginCredentials.GetPluginCredentials(PluginInstance.Main);
                MacroDeckLogger.Info(PluginInstance.Main, $"Found {credentials.Count} saved credentials.");
                if (credentials != null && credentials.Count > 0)
                {
                    foreach (Dictionary<string, string> creds in credentials)
                    {
                        AddRow(creds);
                    }
                }
                else
                {
                    MacroDeckLogger.Info(PluginInstance.Main, "No credentials found, adding one blank row.");
                    AddRow(null);
                }
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(PluginInstance.Main, $"Error loading credentials: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private async void BtnOk_Click(object sender, EventArgs e)
        {
            MacroDeckLogger.Info(PluginInstance.Main, "OK button clicked. Saving credentials...");
            try
            {
                var newCredentials = new List<Dictionary<string, string>>();
                var newConnectionNames = new List<string>();

                foreach (Control control in repeatingLayout.Controls)
                {
                    if (control is VcpConnectionConfigurator config)
                    {
                        var settings = config.Settings;
                        if (settings.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
                        {
                            newConnectionNames.Add(name);
                            newCredentials.Add(settings);
                        }
                    }
                }

                // 1. Save all the new/remaining credentials
                PluginCredentials.DeletePluginCredentials(PluginInstance.Main);
                foreach (var creds in newCredentials)
                {
                    PluginCredentials.AddCredentials(PluginInstance.Main, creds);
                }
                MacroDeckLogger.Info(PluginInstance.Main, $"Saved {newCredentials.Count} credentials.");

                // 2. Find and delete data for removed connections
                var removedConnectionNames = _originalConnectionNames.Except(newConnectionNames).ToList();
                foreach (var removedName in removedConnectionNames)
                {
                    MacroDeckLogger.Info(PluginInstance.Main, $"Connection '{removedName}' was removed. Deleting data...");
                    _ = PluginInstance.Main.RemoveConnectionData(removedName);
                }

                // 3. Reload all connections in the main plugin
                await PluginInstance.Main.SetupAndStartAsync();
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(PluginInstance.Main, $"Error saving credentials: {ex.Message + Environment.NewLine + ex.StackTrace} ");
            }

            this.Close();
        }


        // --- THIS IS THE CORRECTED LOGIC ---
        private void BtnCleanUp_Click(object sender, EventArgs e)
        {
            MacroDeckLogger.Info(PluginInstance.Main, "Clean Up Variables button clicked.");

            // 1. Get all variables created by this plugin
            var allPluginVariables = VariableManager.Variables
                .Where(v => v.Creator == "MultiDisplayVCPClient")
                .ToList();

            // --- Logging ---
            MacroDeckLogger.Info(PluginInstance.Main, $"--- Found {allPluginVariables.Count} variables created by this plugin ---");
            foreach (var variable in allPluginVariables)
            {
                MacroDeckLogger.Info(PluginInstance.Main, $"Found variable: {variable.Name} (Creator: {variable.Creator})");
            }
            // --- End Logging ---

            if (!allPluginVariables.Any())
            {
                System.Windows.Forms.MessageBox.Show("No variables found for this plugin.", "Clean Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Get all *currently configured* connection slugs (e.g., "server_1")
            var activeConnectionSlugs = new HashSet<string>();
            foreach (Control control in repeatingLayout.Controls)
            {
                if (control is VcpConnectionConfigurator config)
                {
                    if (config.Settings.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
                    {
                        string slug = PluginInstance.Main.Slugify(name);
                        activeConnectionSlugs.Add(slug);
                        MacroDeckLogger.Info(PluginInstance.Main, $"Found active connection slug: {slug}");
                    }
                }
            }
            MacroDeckLogger.Info(PluginInstance.Main, $"Found {activeConnectionSlugs.Count} active connection slugs in config.");

            // 3. Find variables to delete (orphans OR legacy)
            var variablesToDelete = new List<Variable>();
            const string prefix = "mdc_";

            foreach (var variable in allPluginVariables)
            {
                bool isModernAndSafe = false;

                // Check against all active connection slugs
                foreach (var slug in activeConnectionSlugs)
                {
                    string newPrefix = prefix + slug + "_";

                    // Check for NEW format: "mdc_server_1_..."
                    if (variable.Name.StartsWith(newPrefix))
                    {
                        isModernAndSafe = true;
                        break;
                    }
                }

                // 4. If it's NOT in the new, safe format, it's an orphan or legacy. Delete it.
                if (!isModernAndSafe)
                {
                    variablesToDelete.Add(variable);
                }
            }

            // 5. Delete the variables
            if (variablesToDelete.Any())
            {
                MacroDeckLogger.Info(PluginInstance.Main, $"Found {variablesToDelete.Count} orphaned or legacy variables to delete.");
                foreach (var orphan in variablesToDelete)
                {
                    MacroDeckLogger.Info(PluginInstance.Main, $"Deleting: {orphan.Name}");
                    VariableManager.DeleteVariable(orphan.Name);
                }

                System.Windows.Forms.MessageBox.Show($"Clean up complete. Removed {variablesToDelete.Count} orphaned/legacy variables.", "Clean Up Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No orphaned or legacy variables found. All variables are up to date.", "Clean Up Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // --- END CORRECTED LOGIC ---


        private void AddRow(Dictionary<string, string> settings)
        {
            try
            {
                repeatingLayout.RowCount++;
                repeatingLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var elm = new VcpConnectionConfigurator();
                elm.Settings = settings;
                elm.Dock = DockStyle.Fill;
                elm.Margin = new Padding(13);
                repeatingLayout.Controls.Add(elm, 0, repeatingLayout.RowCount - 1);

                var btnRemove = new Button
                {
                    Text = "-",
                    Dock = DockStyle.Top,
                    BackColor = Color.Maroon,
                    Margin = new Padding(13),
                };
                btnRemove.Click += BtnRemove_Click;
                repeatingLayout.Controls.Add(btnRemove, 1, repeatingLayout.RowCount - 1);

                string connectionName = settings?["name"] ?? "New Row";
                MacroDeckLogger.Info(PluginInstance.Main, $"Added connection row to config UI: '{connectionName}'");
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(PluginInstance.Main, $"Error adding row to config UI: {ex.Message + Environment.NewLine + ex.StackTrace} ");
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MacroDeckLogger.Info(PluginInstance.Main, "Add row button clicked.");
            AddRow(null);
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                var row = repeatingLayout.GetRow(sender as Control);
                MacroDeckLogger.Info(PluginInstance.Main, $"Remove row button clicked for row {row}.");
                TableLayoutHelper.RemoveArbitraryRow(repeatingLayout, row);
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(PluginInstance.Main, $"Error removing row: {ex.Message + Environment.NewLine + ex.StackTrace} ");
            }
        }

        private void RepeatingLayout_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            // Draws the separator line
            var bottomLeft = new Point(e.CellBounds.Left, e.CellBounds.Bottom);
            var bottomRight = new Point(e.CellBounds.Right, e.CellBounds.Bottom);
            e.Graphics.DrawLine(Pens.White, bottomLeft, bottomRight);
        }
    }
}