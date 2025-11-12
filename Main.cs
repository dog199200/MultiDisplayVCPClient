using MultiDisplayVCPClient.Actions;
using MultiDisplayVCPClient.GUI;
using MultiDisplayVCPClient.GUI.Controls;
using MultiDisplayVCPClient.Properties;
using SuchByte.MacroDeck;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System.Text.RegularExpressions;

namespace MultiDisplayVCPClient
{
    public static class PluginInstance
    {
        public static Main Main { get; set; } = null!;
    }

    public class VcpVariableData
    {
        public string MonitorName { get; set; } = string.Empty;
        public string FeatureName { get; set; } = string.Empty;
        public uint Current { get; set; }
        public uint Max { get; set; }
        public string PnP_ID { get; set; } = string.Empty;
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
        public string Text { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public override string ToString() => Text;
    }


    public partial class Main : MacroDeckPlugin
    {
        public static string Name => "VCP Monitor Control";
        public static string Author => "DoggoDogPack";
        public override bool CanConfigure => true;

        public Dictionary<string, VcpClient> Connections { get; private set; } = [];
        private MainWindow _mainWindow;
        private VcpSelectorButton _statusButton;
        private VcpConnectionList? _togglerList;
        private ToolTip? _statusToolTip;

        public event EventHandler? OnVariableListChanged;

        public List<VcpVariable> ParsedVcpVariables { get; private set; } = [];

        [GeneratedRegex(@"[^a-z0-9\s_-]", RegexOptions.Compiled)]
        private static partial Regex SlugifyInvalidCharsRegex();

        [GeneratedRegex(@"[\s_-]+", RegexOptions.Compiled)]
        private static partial Regex SlugifySeparatorsRegex();

        public Main()
        {
            PluginInstance.Main = this;
            MacroDeck.OnMainWindowLoad += MacroDeck_OnMainWindowLoad;

            _mainWindow = null!;
            _statusButton = null!;
            _togglerList = null;
            _statusToolTip = null;
        }

        private void MacroDeck_OnMainWindowLoad(object? sender, EventArgs e)
        {
            _mainWindow = sender as MainWindow;
            if (_mainWindow == null) return;

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

            SetupAndStart();
        }

        public int GetNumConnected()
        {
            return Connections?.Where(c => c.Value.State == ConnectionState.Connected).Select(c => c.Value).Count() ?? 0;
        }

        private void UpdateStatusButton()
        {
            if (_statusButton == null || _statusToolTip == null) return;
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

        public override void Enable() => this.Actions = [new SetVcpAction()];

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
            catch (Exception)
            {
                // Silently fail
            }
        }

        public async Task<ServerStatus> FetchAndUpdateVariablesAsync(VcpClient client)
        {
            if (client == null) return new ServerStatus { Message = "ERROR: Client not found." };
            if (client.State == ConnectionState.Connecting) return new ServerStatus { Message = "ERROR: Client is busy connecting." };
            if (client.State == ConnectionState.Offline) return new ServerStatus { Message = "ERROR: Client is offline." };

            ServerStatus status = await client.GetCapabilitiesAsync();

            if (!status.Message.StartsWith("ERROR:"))
            {
                if (status.Monitors.Count > 0 && status.Monitors.Any(m => m.Capabilities.Count > 0))
                {
                    UpdateMacroDeckVariables(client.Name, status);
                }
            }
            return status;
        }

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

        private void UpdateMacroDeckVariables(string connectionName, ServerStatus status)
        {
            if (status == null || status.Monitors == null) return;
            try
            {
                string connectionSlug = Slugify(connectionName);

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
                        if (string.IsNullOrEmpty(feature.Name)) continue;

                        string featureSlug = Slugify(feature.Name);
                        string variableName = $"mdc_{connectionSlug}_{monitorSlug}_{featureSlug}";

                        var variableData = new VcpVariableData
                        {
                            MonitorName = $"{monitor.Description} ({monitor.DeviceID})",
                            FeatureName = $"{feature.Name} (0x{feature.Code:X2})",
                            Current = feature.CurrentValue,
                            Max = feature.MaximumValue,
                            PnP_ID = monitor.DeviceID,
                            VcpCode = feature.Code
                        };

                        newParsedVariables.Add(new VcpVariable
                        {
                            VariableName = variableName,
                            Data = variableData,
                            ConnectionSlug = connectionSlug,
                            PnP_ID = monitor.DeviceID,
                            VcpCode = feature.Code
                        });

                        newVariableNames.Add(variableName);
                        VariableManager.SetValue(variableName, feature.CurrentValue.ToString(), VariableType.String, this, null);
                    }
                }

                ParsedVcpVariables.AddRange(newParsedVariables);

                var allPluginVariables = VariableManager.Variables
                    .Where(v => v.Creator == "MultiDisplayVCPClient")
                    .ToList();

                foreach (var oldVar in allPluginVariables)
                {
                    if (oldVar.Name.StartsWith($"mdc_{connectionSlug}_") &&
                        !newVariableNames.Contains(oldVar.Name))
                    {
                        VariableManager.DeleteVariable(oldVar.Name);
                    }
                }
            }
            catch (Exception)
            {
                // Fail silently
            }
            finally
            {
                try
                {
                    OnVariableListChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                    // Fail silently
                }
            }
        }

        public static string Slugify(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            string slug = text.ToLowerInvariant();
            slug = SlugifyInvalidCharsRegex().Replace(slug, " ");
            slug = SlugifySeparatorsRegex().Replace(slug, "_");
            slug = slug.Trim('_');

            return slug;
        }

        public void SetupAndStart()
        {
            try
            {
                var credSet = PluginCredentials.GetPluginCredentials(this);
                var newConnectionNames = new List<string>();
                var newConnections = new Dictionary<string, VcpClient>();

                foreach (var creds in credSet)
                {
                    creds.TryGetValue("name", out string? name);
                    creds.TryGetValue("ipAddress", out string? ip);
                    creds.TryGetValue("port", out string? portStr);
                    creds.TryGetValue("password", out string? password);

                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ip) || !int.TryParse(portStr, out int port)) continue;
                    newConnectionNames.Add(name);

                    if (Connections.TryGetValue(name, out VcpClient? existingClient))
                    {
                        if (existingClient.HasSameSettings(ip, port, password ?? ""))
                        {
                            newConnections[name] = existingClient;
                        }
                        else
                        {
                            existingClient.Disconnect();
                            existingClient.ConnectionStateChanged -= OnClientConnectionStateChanged;
                            var newClient = new VcpClient(name, ip, port, password ?? "");
                            newClient.ConnectionStateChanged += OnClientConnectionStateChanged;
                            newConnections[name] = newClient;
                        }
                    }
                    else
                    {
                        var newClient = new VcpClient(name, ip, port, password ?? "");
                        newClient.ConnectionStateChanged += OnClientConnectionStateChanged;
                        newConnections[name] = newClient;
                    }
                }
                var oldConnectionNames = Connections.Keys.ToList();
                foreach (var oldName in oldConnectionNames)
                {
                    if (!newConnectionNames.Contains(oldName))
                    {
                        if (Connections.TryGetValue(oldName, out VcpClient? deletedClient))
                        {
                            deletedClient.Disconnect();
                            deletedClient.ConnectionStateChanged -= OnClientConnectionStateChanged;
                        }
                    }
                }
                Connections = newConnections;
                ParsedVcpVariables.Clear();

                foreach (var client in Connections.Values)
                {
                    _ = ConnectAndFetchInBackground(client);
                }
            }
            catch (Exception)
            {
                // Fail silently
            }
            UpdateStatusButton();
        }

        public void RemoveConnectionData(string connectionName)
        {
            try
            {
                string connectionSlug = Slugify(connectionName);
                string variablePrefix = $"mdc_{connectionSlug}_";

                ParsedVcpVariables.RemoveAll(v => v.VariableName.StartsWith(variablePrefix));

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
            }
            catch (Exception)
            {
                // Fail silently
            }
            finally
            {
                try
                {
                    OnVariableListChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                    // Fail silently
                }
            }
        }

        private void OnClientConnectionStateChanged(object? sender, ConnectionState e)
        {
            UpdateStatusButton();
        }

        public override void OpenConfigurator()
        {
            try
            {
                using var pluginConfig = new PluginConfig();
                pluginConfig.ShowDialog();
            }
            catch (Exception)
            {
                // Fail silently
            }
        }

        private void StatusButton_Click(object? sender, EventArgs e)
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

            _togglerList = new VcpConnectionList([.. this.Connections.Values])
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(
                    _mainWindow.Location.X + _mainWindow.contentButtonPanel.Location.X + _mainWindow.contentButtonPanel.Width + 4,
                    _mainWindow.Location.Y + _statusButton.Location.Y + _statusButton.Height
                )
            };
            _togglerList.Deactivate += (sender, args) =>
            {
                RemoveTogglerList();
            };
            _togglerList.Show(_mainWindow);
        }

        private void RemoveTogglerList()
        {
            _togglerList?.Close();
            _togglerList = null;
        }
    }
}