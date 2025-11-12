using MultiDisplayVCPClient.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using System.Text.Json;

namespace MultiDisplayVCPClient.Actions
{
    public partial class SetVcpActionConfigControl : ActionConfigControl
    {
        private readonly SetVcpAction _action;
        private SetVcpActionConfig? _config;
        private List<VcpVariable>? _allPluginVariables;

        private bool _isLoaded;
        private bool _configLoadedToUI;

        private DropdownItem? _previousConnection;
        private DropdownItem? _previousMonitor;
        private DropdownItem? _previousSetting;

        public SetVcpActionConfigControl(SetVcpAction action)
        {
            _action = action;
            InitializeComponent();
            LoadConfig();
        }

        private void OnLoad(object? sender, EventArgs e)
        {
            LoadConfig();
            PluginInstance.Main.OnVariableListChanged += OnVariableListChanged;

            if (PluginInstance.Main.ParsedVcpVariables.Count > 0)
            {
                comboConnections.Enabled = true;
                LoadConfigData();
            }

            _isLoaded = true;
        }

        #region Config Load/Save

        public override bool OnActionSave()
        {
            return SaveConfig();
        }

        private void LoadConfig()
        {
            try
            {
                if (!string.IsNullOrEmpty(_action.Configuration))
                {
                    _config = JsonSerializer.Deserialize<SetVcpActionConfig>(_action.Configuration);
                }
            }
            catch { }

            _config ??= new SetVcpActionConfig();
        }

        private void LoadConfigData()
        {
            if (_configLoadedToUI) return;

            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;
            PopulateConnections();
            LoadControlValues();

            _configLoadedToUI = true;
        }

        private bool SaveConfig()
        {
            if (_config == null) return false;

            _config.ConnectionName = (comboConnections.SelectedItem as DropdownItem)?.Text ?? "";
            _config.MonitorPnP_ID = (comboMonitors.SelectedItem as DropdownItem)?.Value ?? "";
            _config.VcpCode = (comboSettings.SelectedItem as DropdownItem)?.Value ?? "";
            _config.VcpValue = (uint)numericValue.Value;

            _action.Configuration = JsonSerializer.Serialize(_config);
            _action.ConfigurationSummary = $"{_config.ConnectionName} | {(comboMonitors.SelectedItem as DropdownItem)?.Text ?? "..."} | {(comboSettings.SelectedItem as DropdownItem)?.Text ?? "..."}: {_config.VcpValue}";

            return true;
        }

        private void LoadControlValues()
        {
            if (_config == null) return;

            comboConnections.SelectedIndexChanged -= OnConnectionSelected;
            comboMonitors.SelectedIndexChanged -= OnMonitorSelected;
            comboSettings.SelectedIndexChanged -= OnSettingSelected;

            if (!string.IsNullOrEmpty(_config.ConnectionName))
            {
                var item = comboConnections.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Text == _config.ConnectionName);
                if (item != null)
                {
                    comboConnections.SelectedItem = item;
                }
            }

            PopulateMonitors();
            if (!string.IsNullOrEmpty(_config.MonitorPnP_ID))
            {
                var item = comboMonitors.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == _config.MonitorPnP_ID);
                if (item != null)
                {
                    comboMonitors.SelectedItem = item;
                }
            }

            PopulateSettings();
            if (!string.IsNullOrEmpty(_config.VcpCode))
            {
                var item = comboSettings.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == _config.VcpCode);
                if (item != null)
                {
                    comboSettings.SelectedItem = item;
                }
            }

            PopulateValue();

            if (!string.IsNullOrEmpty(_config.ConnectionName) &&
                !string.IsNullOrEmpty(_config.MonitorPnP_ID) &&
                !string.IsNullOrEmpty(_config.VcpCode))
            {
                numericValue.Value = _config.VcpValue;
            }
            else
            {
                numericValue.Value = 0;
            }

            comboConnections.SelectedIndexChanged += OnConnectionSelected;
            comboMonitors.SelectedIndexChanged += OnMonitorSelected;
            comboSettings.SelectedIndexChanged += OnSettingSelected;
        }

        #endregion

        #region Dropdown Population

        private void OnVariableListChanged(object? sender, EventArgs e)
        {
            if (!_isLoaded || this.IsDisposed) return;

            this.InvokeIfRequired(() =>
            {
                comboConnections.Enabled = true;
                LoadConfigData();
            });
        }

        private void PopulateConnections()
        {
            comboConnections.Items.Clear();
            var connectionItems = PluginInstance.Main.GetConnectionDropdownItems();
            comboConnections.Items.AddRange([.. connectionItems]);
        }

        private void PopulateMonitors()
        {
            comboMonitors.Items.Clear();
            comboMonitors.Enabled = false;

            if (_allPluginVariables == null ||
                comboConnections.SelectedItem is not DropdownItem selectedConnection) return;

            var monitors = _allPluginVariables
                .Where(v => v.ConnectionSlug == selectedConnection.Value)
                .Select(v => new { v.PnP_ID, v.Data.MonitorName })
                .Distinct()
                .OrderBy(m => m.MonitorName);

            foreach (var monitor in monitors)
            {
                comboMonitors.Items.Add(new DropdownItem { Text = monitor.MonitorName, Value = monitor.PnP_ID });
            }

            if (comboMonitors.Items.Count > 0)
            {
                comboMonitors.Enabled = true;
            }
        }

        private void PopulateSettings()
        {
            comboSettings.Items.Clear();
            comboSettings.Enabled = false;

            if (_allPluginVariables == null ||
                comboConnections.SelectedItem is not DropdownItem selectedConnection ||
                comboMonitors.SelectedItem is not DropdownItem selectedMonitor) return;

            var settings = _allPluginVariables
                .Where(v => v.ConnectionSlug == selectedConnection.Value && v.PnP_ID == selectedMonitor.Value)
                .Select(v => new { v.VcpCode, v.Data.FeatureName, v.Data.Max })
                .Distinct()
                .OrderBy(s => s.FeatureName);

            foreach (var setting in settings)
            {
                comboSettings.Items.Add(new DropdownItem { Text = setting.FeatureName, Value = setting.VcpCode.ToString() });
            }

            if (comboSettings.Items.Count > 0)
            {
                comboSettings.Enabled = true;
            }
        }

        private void PopulateValue()
        {
            numericValue.Enabled = false;
            numericValue.Minimum = 0;
            numericValue.Maximum = 0;

            if (_allPluginVariables == null ||
                comboConnections.SelectedItem is not DropdownItem selectedConnection ||
                comboMonitors.SelectedItem is not DropdownItem selectedMonitor ||
                comboSettings.SelectedItem is not DropdownItem selectedSetting) return;

            var variable = _allPluginVariables
                .FirstOrDefault(v => v.ConnectionSlug == selectedConnection.Value &&
                                     v.PnP_ID == selectedMonitor.Value &&
                                     v.VcpCode.ToString() == selectedSetting.Value);

            if (variable.Data != null)
            {
                numericValue.Maximum = variable.Data.Max;
                toolTip1.SetToolTip(numericValue, $"Min: 0, Max: {variable.Data.Max}");
                numericValue.Enabled = true;
            }
        }

        #endregion

        #region Dropdown Enter Events
        private void OnConnectionsEnter(object? sender, EventArgs e)
        {
            _previousConnection = comboConnections.SelectedItem as DropdownItem;
            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;
            string? currentValue = (comboConnections.SelectedItem as DropdownItem)?.Value;
            PopulateConnections();

            if (currentValue != null)
            {
                var itemToRestore = comboConnections.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == currentValue);
                if (itemToRestore != null)
                {
                    comboConnections.SelectedItem = itemToRestore;
                }
            }
        }

        private void OnMonitorsEnter(object? sender, EventArgs e)
        {
            _previousMonitor = comboMonitors.SelectedItem as DropdownItem;
            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;
            string? currentValue = (comboMonitors.SelectedItem as DropdownItem)?.Value;
            PopulateMonitors();

            if (currentValue != null)
            {
                var itemToRestore = comboMonitors.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == currentValue);
                if (itemToRestore != null)
                {
                    comboMonitors.SelectedItem = itemToRestore;
                }
            }
        }

        private void OnSettingsEnter(object? sender, EventArgs e)
        {
            _previousSetting = comboSettings.SelectedItem as DropdownItem;
            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;
            string? currentValue = (comboSettings.SelectedItem as DropdownItem)?.Value;
            PopulateSettings();

            if (currentValue != null)
            {
                var itemToRestore = comboSettings.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == currentValue);
                if (itemToRestore != null)
                {
                    comboSettings.SelectedItem = itemToRestore;
                }
            }
        }
        #endregion


        #region Dropdown Changed Events
        private void OnConnectionSelected(object? sender, EventArgs e)
        {
            if (comboConnections.SelectedItem is DropdownItem currentConnection &&
                currentConnection == _previousConnection)
            {
                return;
            }

            comboMonitors.Items.Clear();
            comboSettings.Items.Clear();
            numericValue.Value = 0;
            numericValue.Enabled = false;

            PopulateMonitors();
            SaveConfig();
        }

        private void OnMonitorSelected(object? sender, EventArgs e)
        {
            if (comboMonitors.SelectedItem is DropdownItem currentMonitor &&
                currentMonitor == _previousMonitor)
            {
                return;
            }

            comboSettings.Items.Clear();
            numericValue.Value = 0;
            numericValue.Enabled = false;

            PopulateSettings();
            SaveConfig();
        }

        private void OnSettingSelected(object? sender, EventArgs e)
        {
            if (comboSettings.SelectedItem is DropdownItem currentSetting &&
                currentSetting == _previousSetting)
            {
                return;
            }

            numericValue.Value = 0;
            PopulateValue();
            SaveConfig();
        }


        private void OnValueChanged(object? sender, EventArgs e)
        {
            SaveConfig();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PluginInstance.Main.OnVariableListChanged -= OnVariableListChanged;

                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}