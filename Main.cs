using MultiDisplayVCPClient.Actions;
using MultiDisplayVCPClient.GUI;
using MultiDisplayVCPClient.GUI.Controls;
using MultiDisplayVCPClient.Properties;
using SuchByte.MacroDeck;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Logging;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Startup;
using SuchByte.MacroDeck.Variables;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions; // --- ADD THIS ---

namespace MultiDisplayVCPClient
{
    public static class PluginInstance
    {
        public static Main Main { get; set; }
    }

    // --- These are the shared data models ---

    public class VcpVariableData
    {
        public string MonitorName { get; set; }
        public string FeatureName { get; set; }
        public uint Current { get; set; }
        public uint Max { get; set; }
        public string PnP_ID { get; set; }
        public byte VcpCode { get; set; }
    }

    public struct VcpVariable
    {
        public string VariableName { get; set; }
        public VcpVariableData Data { get; set; }

        public string ConnectionSlug { get; set; }
        public string PnP_ID { get; set; }
        public byte VcpCode { get; set; }
    }

    public class DropdownItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public override string ToString() => Text;
    }


    public partial class Main : MacroDeckPlugin
    {
        public string Name => "VCP Monitor Control";
        public string Author => "DoggoDogPack";
        public override bool CanConfigure => true;

        // Connection management
        public Dictionary<string, VcpClient> Connections = new();
        private MainWindow _mainWindow;
        private VcpSelectorButton _statusButton;
        private VcpConnectionList _togglerList;
        private ToolTip _statusToolTip;

        public event EventHandler OnVariableListChanged;

        // In-memory "source of truth"
        public List<VcpVariable> ParsedVcpVariables { get; private set; } = new List<VcpVariable>();

        // --- ALL FILE CACHE LOGIC IS REMOVED ---

        public Main()
        {
            PluginInstance.Main = this;
            MacroDeckLogger.Info(this, "VCP Monitor Control plugin constructed.");
            MacroDeck.OnMainWindowLoad += MacroDeck_OnMainWindowLoad;
        }

        private async void MacroDeck_OnMainWindowLoad(object sender, EventArgs e)
        {
            _mainWindow = sender as MainWindow;
            var numConnected = GetNumConnected();
            var buttonWidth = _mainWindow.contentButtonPanel.ClientRectangle.Width;
            _statusToolTip = new ToolTip();
            this._statusButton = new VcpSelectorButton
            {
                AlertText = numConnected.ToString(),
                BackgroundImage = numConnected > 0 ? Properties.Resources.MCC_Online : Properties.Resources.MCC_Offline,
                BackgroundImageLayout = ImageLayout.Zoom,
                Width = buttonWidth,
            };
            _statusToolTip.SetToolTip(_statusButton, $"{numConnected} Connection(s) Active");
            _statusButton.Click += StatusButton_Click;
            _mainWindow.contentButtonPanel.Controls.Add(_statusButton);
            MacroDeckLogger.Info(this, "Main window loaded. Firing SetupAndStartAsync().");

            await SetupAndStartAsync();
        }

        public int GetNumConnected()
        {
            return Connections?.Where(c => c.Value.State == ConnectionState.Connected).Select(c => c.Value).Count() ?? 0;
        }

        private void UpdateStatusButton()
        {
            if (_statusButton == null) return;
            var numConnected = GetNumConnected();
            var newIcon = numConnected > 0 ? Resources.MCC_Online : Resources.MCC_Offline;
            string tooltipText = $"VCP: {numConnected} connection(s) active";
            _statusButton.InvokeIfRequired(() =>
            {
                _statusButton.BackgroundImage = newIcon;
                _statusButton.AlertText = numConnected.ToString();
                _statusToolTip.SetToolTip(_statusButton, tooltipText);
            });
        }

        public override void Enable()
        {
            this.Actions = new List<PluginAction> { new SetVcpAction() };
            MacroDeckLogger.Info(this, "Plugin enabled successfully. 'SetVcpAction' registered.");
        }


        public async Task ConnectAndFetchInBackground(VcpClient client)
        {
            if (client == null || client.State == ConnectionState.Connected) return;

            try
            {
                await client.ConnectAsync();
                if (client.State == ConnectionState.Connected)
                {
                    _ = FetchAndUpdateVariablesAsync(client);
                }
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Warning(this, $"({client.Name}) Auto-connect PING failed: {ex.Message}");
            }
        }

        public async Task<ServerStatus> FetchAndUpdateVariablesAsync(VcpClient client)
        {
            if (client == null) return new ServerStatus { Message = "ERROR: Client not found." };
            if (client.State == ConnectionState.Connecting) return new ServerStatus { Message = "ERROR: Client is busy connecting." };
            if (client.State == ConnectionState.Offline) return new ServerStatus { Message = "ERROR: Client is offline." };

            MacroDeckLogger.Info(this, $"({client.Name}) Fetching new monitor data from server...");

            ServerStatus status = await client.GetCapabilitiesAsync();

            if (!status.Message.StartsWith("ERROR:"))
            {
                if (status.Monitors.Any() && status.Monitors.Any(m => m.Capabilities.Any()))
                {
                    UpdateMacroDeckVariables(client.Name, status);
                }
                else
                {
                    MacroDeckLogger.Warning(this, $"({client.Name}) Fetch successful but returned no monitors or no capabilities. Ignoring data.");
                }
            }
            return status;
        }

        /// <summary>
        /// Gets a pre-built list of DropdownItems for all active connections.
        /// </summary>
        public List<DropdownItem> GetConnectionDropdownItems()
        {
            var items = new List<DropdownItem>();

            var connections = this.Connections.Keys
                .OrderBy(name => name);

            foreach (var friendlyName in connections)
            {
                string slug = Slugify(friendlyName);
                items.Add(new DropdownItem { Text = friendlyName, Value = slug });
            }

            return items;
        }

        // --- THIS IS THE CORRECTED METHOD ---
        private void UpdateMacroDeckVariables(string connectionName, ServerStatus status)
        {
            if (status == null || status.Monitors == null) return;
            try
            {
                string connectionSlug = Slugify(connectionName);

                // --- This method now rebuilds the *in-memory* list ---
                // --- We must clear data ONLY for this connection ---
                ParsedVcpVariables.RemoveAll(v =>
                    v.VariableName.StartsWith($"mdc_{connectionSlug}_"));

                var newVariableNames = new HashSet<string>();
                var newParsedVariables = new List<VcpVariable>();

                foreach (var monitor in status.Monitors)
                {
                    if (string.IsNullOrEmpty(monitor.DeviceID)) continue;
                    if (string.IsNullOrEmpty(monitor.Description)) continue;

                    string monitorSlug = Slugify(monitor.DeviceID);

                    foreach (var feature in monitor.Capabilities)
                    {
                        // If the feature has no name, it's not usable. Skip it.
                        if (string.IsNullOrEmpty(feature.Name)) continue;

                        string featureSlug = Slugify(feature.Name);

                        // 1. --- PREPARE DATA ---

                        // Format: mdc_{connection_slug}_{monitor_slug}_{feature_slug}
                        string variableName = $"mdc_{connectionSlug}_{monitorSlug}_{featureSlug}";

                        // This is the rich data packet for our "in-code" list
                        var variableData = new VcpVariableData
                        {
                            MonitorName = $"{monitor.Description} ({monitor.DeviceID})",
                            FeatureName = $"{feature.Name} (0x{feature.Code:X2})",
                            Current = feature.CurrentValue,
                            Max = feature.MaximumValue,
                            PnP_ID = monitor.DeviceID,
                            VcpCode = feature.Code
                        };

                        // 2. --- UPDATE "IN-CODE" LIST ---
                        newParsedVariables.Add(new VcpVariable
                        {
                            VariableName = variableName,
                            Data = variableData,

                            // --- ADD THESE THREE LINES ---
                            ConnectionSlug = connectionSlug,
                            PnP_ID = monitor.DeviceID,
                            VcpCode = feature.Code
                        });

                        // 3. --- UPDATE MACRO DECK VARIABLE ---
                        // Set the simple value (e.g., "50") in VariableManager
                        newVariableNames.Add(variableName);
                        VariableManager.SetValue(variableName, feature.CurrentValue.ToString(), VariableType.String, this, false);
                    }
                }

                // Add all the new variables to our master in-memory list
                ParsedVcpVariables.AddRange(newParsedVariables);


                // --- 4. CLEAN UP STALE VARIABLES ---
                // Get all variables *created by this plugin*
                var allPluginVariables = VariableManager.Variables
                    .Where(v => v.Creator == "MultiDisplayVCPClient")
                    .ToList();

                // Find any variable from this plugin that belongs to *this connection*
                // but is *not* in the "new names" list from the server.
                foreach (var oldVar in allPluginVariables)
                {
                    if (oldVar.Name.StartsWith($"mdc_{connectionSlug}_") &&
                        !newVariableNames.Contains(oldVar.Name))
                    {
                        MacroDeckLogger.Info(this, $"Deleting stale variable: {oldVar.Name}");
                        VariableManager.DeleteVariable(oldVar.Name);
                    }
                }

                MacroDeckLogger.Info(this, $"({connectionName}) Updated variables for {status.Monitors.Count} monitors.");
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(this, $"Failed to update Macro Deck variables: {ex.Message}");
            }
            finally
            {
                // After variables are updated, notify all listeners (like the UI).
                try
                {
                    OnVariableListChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MacroDeckLogger.Error(this, $"Error firing OnVariableListChanged event: {ex.Message}");
                }
            }
        }
        // --- END CORRECTED METHOD ---

        // --- THIS IS THE NEW, ROBUST SLUGIFY METHOD ---
        public string Slugify(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            // 1. Convert to lower
            string slug = text.ToLowerInvariant();

            // 2. Replace toxic characters (like ':') with a space
            slug = Regex.Replace(slug, @"[^a-z0-9\s_-]", " ", RegexOptions.Compiled);

            // 3. Replace one or more spaces/underscores/hyphens with a single underscore
            // --- THIS REGEX IS CORRECTED to include hyphens ---
            slug = Regex.Replace(slug, @"[\s_-]+", "_", RegexOptions.Compiled);

            // 4. Trim leading/trailing underscores
            slug = slug.Trim('_');

            return slug;
        }
        // --- END NEW SLUGIFY METHOD ---

        public async Task SetupAndStartAsync()
        {
            MacroDeckLogger.Info(this, "Running connection sync...");
            try
            {
                var credSet = PluginCredentials.GetPluginCredentials(this);
                var newConnectionNames = new List<string>();
                var newConnections = new Dictionary<string, VcpClient>();

                foreach (var creds in credSet)
                {
                    creds.TryGetValue("name", out string name);
                    creds.TryGetValue("ipAddress", out string ip);
                    creds.TryGetValue("port", out string portStr);
                    creds.TryGetValue("password", out string password);

                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ip) || !int.TryParse(portStr, out int port)) continue;
                    newConnectionNames.Add(name);

                    if (Connections.TryGetValue(name, out VcpClient existingClient))
                    {
                        if (existingClient.HasSameSettings(ip, port, password))
                        {
                            newConnections[name] = existingClient;
                        }
                        else
                        {
                            MacroDeckLogger.Info(this, $"Connection '{name}' settings changed. Re-initializing...");
                            existingClient.Disconnect();
                            existingClient.ConnectionStateChanged -= OnClientConnectionStateChanged;
                            var newClient = new VcpClient(name, ip, port, password);
                            newClient.ConnectionStateChanged += OnClientConnectionStateChanged;
                            newConnections[name] = newClient;
                        }
                    }
                    else
                    {
                        MacroDeckLogger.Info(this, $"New connection found: '{name}'.");
                        var newClient = new VcpClient(name, ip, port, password);
                        newClient.ConnectionStateChanged += OnClientConnectionStateChanged;
                        newConnections[name] = newClient;
                    }
                }
                var oldConnectionNames = Connections.Keys.ToList();
                foreach (var oldName in oldConnectionNames)
                {
                    if (!newConnectionNames.Contains(oldName))
                    {
                        MacroDeckLogger.Info(this, $"Connection '{oldName}' removed. Disconnecting...");
                        if (Connections.TryGetValue(oldName, out VcpClient deletedClient))
                        {
                            deletedClient.Disconnect();
                            deletedClient.ConnectionStateChanged -= OnClientConnectionStateChanged;
                        }
                    }
                }
                Connections = newConnections;
                MacroDeckLogger.Info(this, "Connection sync complete.");
                MacroDeckLogger.Info(this, "Attempting to auto-test all loaded connections...");

                ParsedVcpVariables.Clear();

                foreach (var client in Connections.Values)
                {
                    _ = ConnectAndFetchInBackground(client);
                }
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(this, $"Error during connection sync: {ex.Message}\n{ex.StackTrace}");
            }
            UpdateStatusButton();
        }

        // --- THIS IS THE CORRECTED METHOD ---
        public async Task RemoveConnectionData(string connectionName)
        {
            try
            {
                string connectionSlug = Slugify(connectionName);
                string variablePrefix = $"mdc_{connectionSlug}_";

                // --- 1. Remove from "in-code" list ---
                int removedCount = ParsedVcpVariables.RemoveAll(v => v.VariableName.StartsWith(variablePrefix));
                MacroDeckLogger.Info(this, $"Removed {removedCount} in-memory variables for '{connectionName}'.");

                // --- 2. Remove from VariableManager ---
                var allVariableNames = VariableManager.Variables
                    .Where(v => v.Creator == "MultiDisplayVCPClient")
                    .Select(v => v.Name)
                    .ToList();

                var variablesToDelete = allVariableNames
                    .Where(varName => varName.StartsWith(variablePrefix))
                    .ToList();

                foreach (var varName in variablesToDelete)
                {
                    VariableManager.DeleteVariable(varName);
                }

                MacroDeckLogger.Info(this, $"Deleted {variablesToDelete.Count} Macro Deck variables for '{connectionName}'.");
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(this, $"Failed to delete variables for '{connectionName}': {ex.Message}");
            }
            finally
            {
                // After variables are deleted, notify listeners.
                try
                {
                    OnVariableListChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MacroDeckLogger.Error(this, $"Error firing OnVariableListChanged event: {ex.Message}");
                }
            }
            await Task.CompletedTask;
        }
        // --- END CORRECTED METHOD ---

        private void OnClientConnectionStateChanged(object sender, ConnectionState e)
        {
            UpdateStatusButton();
        }

        public override void OpenConfigurator()
        {
            MacroDeckLogger.Info(this, "Opening configurator...");
            try
            {
                using var pluginConfig = new PluginConfig();
                pluginConfig.ShowDialog();
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(this, $"Error opening configurator: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void StatusButton_Click(object sender, EventArgs e)
        {
            if (_togglerList?.Visible ?? false)
            {
                RemoveTogglerList();
            }
            else
            {
                ShowTogglerList();
            }
        }

        private void ShowTogglerList()
        {
            _togglerList?.Close();

            _togglerList = new VcpConnectionList(this.Connections.Values.ToList())
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(
                    _mainWindow.Location.X + _mainWindow.contentButtonPanel.Location.X + _mainWindow.contentButtonPanel.Width + 4,
                    _mainWindow.Location.Y + _statusButton.Location.Y + _statusButton.Height
                )
            };
            _togglerList.Deactivate += (object sender, EventArgs args) =>
            {
                RemoveTogglerList();
            };
            _togglerList.Show(_mainWindow);
            MacroDeckLogger.Info(this, "Showing connection list popup.");
        }

        private void RemoveTogglerList()
        {
            _togglerList?.Close();
            _togglerList = null;
            MacroDeckLogger.Info(this, "Hiding connection list popup.");
        }
    }
}