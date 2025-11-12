using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Language;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    public partial class PluginConfig : DialogForm
    {
        public PluginConfig()
        {
            InitializeComponent();

            btnOk.Text = LanguageManager.Strings.Ok;
            btnCleanUp.Click += BtnCleanUp_Click;

            LoadCredentials();
        }

        /// <summary>
        /// Loads credentials and populates the grid with ConnectionIconControls.
        /// </summary>
        private void LoadCredentials()
        {
            connectionsPanel.Controls.Clear();
            List<Dictionary<string, string>>? credentials = PluginCredentials.GetPluginCredentials(PluginInstance.Main);

            if (credentials == null || credentials.Count == 0) return;

            foreach (Dictionary<string, string> creds in credentials)
            {
                CreateConnectionIcon(creds);
            }
        }

        /// <summary>
        /// Creates a new ConnectionIconControl, wires up its events, and adds it to the panel.
        /// </summary>
        private void CreateConnectionIcon(Dictionary<string, string>? settings)
        {
            if (settings == null) return;

            var iconControl = new ConnectionIconControl
            {
                Settings = settings
            };

            // Wire up events
            iconControl.EditClicked += OnEditConnection;
            iconControl.DeleteClicked += OnDeleteConnection;

            // Check if this connection is currently active to set the icon
            if (settings.TryGetValue("name", out var name) &&
                PluginInstance.Main.Connections.TryGetValue(name, out var client) &&
                client.State == ConnectionState.Connected)
            {
                iconControl.SetOnlineStatus(true);
            }
            else
            {
                iconControl.SetOnlineStatus(false);
            }

            connectionsPanel.Controls.Add(iconControl);
        }

        /// <summary>
        /// Handles the click event for the 'Ok' button (Save and Close).
        /// </summary>
        private void BtnOk_Click(object? sender, EventArgs e)
        {
            try
            {
                var newCredentials = new List<Dictionary<string, string>>();
                var newConnectionNames = new List<string>();
                var originalConnectionNames = PluginInstance.Main.Connections.Keys.ToList();

                // Loop over the new icon controls to get their settings
                foreach (ConnectionIconControl iconControl in connectionsPanel.Controls)
                {
                    if (iconControl.Settings != null)
                    {
                        newCredentials.Add(iconControl.Settings);
                        if (iconControl.Settings.TryGetValue("name", out var name))
                        {
                            newConnectionNames.Add(name);
                        }
                    }
                }

                // Save the new credential list
                PluginCredentials.DeletePluginCredentials(PluginInstance.Main);
                foreach (var creds in newCredentials)
                {
                    PluginCredentials.AddCredentials(PluginInstance.Main, creds);
                }

                // Find any connections that were deleted
                var removedConnectionNames = originalConnectionNames.Except(newConnectionNames).ToList();
                foreach (var removedName in removedConnectionNames)
                {
                    PluginInstance.Main.RemoveConnectionData(removedName);
                }

                // Reload all connections
                PluginInstance.Main.SetupAndStart();
            }
            catch (Exception)
            {
                // Fail silently
            }

            this.Close();
        }

        /// <summary>
        /// Handles the click event for the 'Clean Up Variables' button.
        /// </summary>
        private void BtnCleanUp_Click(object? sender, EventArgs e)
        {
            var allPluginVariables = VariableManager.Variables
                .Where(v => v.Creator == "MultiDisplayVCPClient")
                .ToList();

            if (allPluginVariables.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No variables found for this plugin.", "Clean Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get active connection slugs from the current UI state
            var activeConnectionSlugs = new HashSet<string>();
            foreach (ConnectionIconControl iconControl in connectionsPanel.Controls)
            {
                if (iconControl.Settings != null && iconControl.Settings.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
                {
                    string slug = Main.Slugify(name);
                    activeConnectionSlugs.Add(slug);
                }
            }

            var variablesToDelete = new List<Variable>();
            const string prefix = "mdc_";

            foreach (var variable in allPluginVariables)
            {
                bool isModernAndSafe = false;
                foreach (var slug in activeConnectionSlugs)
                {
                    string newPrefix = prefix + slug + "_";
                    if (variable.Name.StartsWith(newPrefix))
                    {
                        isModernAndSafe = true;
                        break;
                    }
                }
                if (!isModernAndSafe)
                {
                    variablesToDelete.Add(variable);
                }
            }

            if (variablesToDelete.Count > 0)
            {
                foreach (var orphan in variablesToDelete)
                {
                    VariableManager.DeleteVariable(orphan.Name);
                }
                System.Windows.Forms.MessageBox.Show($"Clean up complete. Removed {variablesToDelete.Count} orphaned/legacy variables.", "Clean Up Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No orphaned or legacy variables found. All variables are up to date.", "Clean Up Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles the click event for the 'New Connection' button.
        /// </summary>
        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            // Open the editor with null settings to create a new connection
            using var editor = new ConnectionEditorForm(null);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                // Create a new icon with the settings from the editor
                CreateConnectionIcon(editor.Settings);
            }
        }

        /// <summary>
        /// Handles the 'Delete' event from a ConnectionIconControl.
        /// </summary>
        private void OnDeleteConnection(object? sender, EventArgs e)
        {
            if (sender is ConnectionIconControl iconControl)
            {
                var result = System.Windows.Forms.MessageBox.Show($"Are you sure you want to delete '{iconControl.Settings?["name"]}'?", "Delete Connection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    connectionsPanel.Controls.Remove(iconControl);
                    iconControl.Dispose();
                }
            }
        }

        /// <summary>
        /// Handles the 'Edit' event from a ConnectionIconControl.
        /// </summary>
        private void OnEditConnection(object? sender, EventArgs e)
        {
            if (sender is ConnectionIconControl iconControl)
            {
                // Open the editor with this icon's current settings
                using var editor = new ConnectionEditorForm(iconControl.Settings);
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    // Update the icon's settings with the new ones from the editor
                    iconControl.Settings = editor.Settings;
                }
            }
        }
    }
}