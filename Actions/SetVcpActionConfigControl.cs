using MultiDisplayVCPClient.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Logging;
using SuchByte.MacroDeck.Variables; // --- ADD THIS ---
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MultiDisplayVCPClient.Actions
{
    public partial class SetVcpActionConfigControl : ActionConfigControl
    {
        private readonly SetVcpAction _action;
        private SetVcpActionConfig _config;
        private List<VcpVariable> _allPluginVariables;

        private bool _isLoaded = false;
        private bool _configLoadedToUI = false;

        private DropdownItem _previousConnection;
        private DropdownItem _previousMonitor;
        private DropdownItem _previousSetting;



        public SetVcpActionConfigControl(SetVcpAction action)
        {
            _action = action;
            InitializeComponent();
            LoadConfig();
        }

        private void SetVcpActionConfigControl_Load(object sender, EventArgs e)
        {
            // 1. Load the saved config object
            LoadConfig();

            // 2. Subscribe to the "data ready" signal
            PluginInstance.Main.OnVariableListChanged += OnVariableListChanged;

            // 3. (SCENARIO 1)
            // If data is already available when we load...
            if (PluginInstance.Main.ParsedVcpVariables.Count > 0)
            {
                // Enable the dropdown
                comboConnections.Enabled = true;

                // --- ADD THIS ---
                // And load the saved data right now
                LoadConfigData();
                // --- END ADD ---
            }

            // 4. Mark the control as loaded
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
                _config = JsonSerializer.Deserialize<SetVcpActionConfig>(_action.Configuration);
            }
            catch { }

            if (_config == null)
            {
                _config = new SetVcpActionConfig();
            }
        }

        /// <summary>
        /// This method loads the saved config values into the UI controls.
        /// It's designed to only run once.
        /// </summary>
        private void LoadConfigData()
        {
            // If we've already run this, don't do it again.
            if (_configLoadedToUI) return;

            // 1. Get the fresh data from Main
            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;

            // --- THIS IS THE FIX ---
            // 2. Populate the connections dropdown *before* loading values
            PopulateConnections();
            // --- END FIX ---

            // 3. This is the original method that now populates all
            //    child dropdowns and sets the saved values.
            LoadControlValues();

            // 4. Mark that we've finished.
            _configLoadedToUI = true;
        }

        private bool SaveConfig()
        {
            var selectedConnection = comboConnections.SelectedItem as DropdownItem;
            var selectedMonitor = comboMonitors.SelectedItem as DropdownItem;
            var selectedSetting = comboSettings.SelectedItem as DropdownItem;

            // --- Save logic ---
            _config.ConnectionName = selectedConnection?.Text ?? "";
            _config.MonitorPnP_ID = selectedMonitor?.Value ?? "";
            _config.VcpCode = selectedSetting?.Value ?? "";
            _config.VcpValue = (uint)numericValue.Value;

            _action.Configuration = JsonSerializer.Serialize(_config);

            // --- Summary logic ---
            _action.ConfigurationSummary = $"{_config.ConnectionName} | {selectedMonitor?.Text ?? "..."} | {selectedSetting?.Text ?? "..."}: {_config.VcpValue}";

            return true;
        }

        // This loads the dropdowns based on saved config
        private void LoadControlValues()
        {
            // --- Unhook events to prevent cascade-firing ---
            comboConnections.SelectedIndexChanged -= comboConnections_SelectedIndexChanged;
            comboMonitors.SelectedIndexChanged -= comboMonitors_SelectedIndexChanged;
            comboSettings.SelectedIndexChanged -= comboSettings_SelectedIndexChanged;

            // 1. Select Connection
            if (!string.IsNullOrEmpty(_config.ConnectionName))
            {
                var item = comboConnections.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Text == _config.ConnectionName);
                if (item != null)
                {
                    comboConnections.SelectedItem = item;
                }
            }

            // 2. Select Monitor (which populates settings)
            PopulateMonitors(); // This needs to run to populate the next dropdown
            if (!string.IsNullOrEmpty(_config.MonitorPnP_ID))
            {
                var item = comboMonitors.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == _config.MonitorPnP_ID);
                if (item != null)
                {
                    comboMonitors.SelectedItem = item;
                }
            }

            // 3. Select Setting (which populates value box)
            PopulateSettings(); // This needs to run to populate the next dropdown
            if (!string.IsNullOrEmpty(_config.VcpCode))
            {
                var item = comboSettings.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == _config.VcpCode);
                if (item != null)
                {
                    comboSettings.SelectedItem = item;
                }
            }

            // This will set the min/max/enabled state
            PopulateValue();

            // 4. Set Value
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

            // --- Re-hook events ---
            comboConnections.SelectedIndexChanged += comboConnections_SelectedIndexChanged;
            comboMonitors.SelectedIndexChanged += comboMonitors_SelectedIndexChanged;
            comboSettings.SelectedIndexChanged += comboSettings_SelectedIndexChanged;
        }

        #endregion

        #region Dropdown Population

        private void OnVariableListChanged(object sender, EventArgs e)
        {
            // This is the "check logic" from Main.cs
            // Its ONLY job is to enable the dropdown.
            if (!_isLoaded || this.IsDisposed) return;

            this.InvokeIfRequired(() => {
                // (SCENARIO 2)
                // Data just became available.

                // Enable the dropdown
                comboConnections.Enabled = true;

                // --- ADD THIS ---
                // And load the saved data right now
                LoadConfigData();
                // --- END ADD ---
            });
        }

        private void PopulateConnections()
        {
            comboConnections.Items.Clear();

            // Get the pre-built list directly from Main
            var connectionItems = PluginInstance.Main.GetConnectionDropdownItems();

            // Add them all to the combo box
            comboConnections.Items.AddRange(connectionItems.ToArray());
        }

        private void PopulateMonitors()
        {
            comboMonitors.Items.Clear();
            // --- "..." item REMOVED ---
            comboMonitors.Enabled = false;

            var selectedConnection = comboConnections.SelectedItem as DropdownItem;
            // --- Revert check: just check for null ---
            if (selectedConnection == null) return;


            // Find all unique Monitors for *this* connection
            var monitors = _allPluginVariables
                .Where(v => v.ConnectionSlug == selectedConnection.Value)
                .Select(v => new { v.PnP_ID, v.Data.MonitorName })
                .Distinct()
                .OrderBy(m => m.MonitorName);

            foreach (var monitor in monitors)
            {
                comboMonitors.Items.Add(new DropdownItem { Text = monitor.MonitorName, Value = monitor.PnP_ID });
            }

            if (comboMonitors.Items.Count > 0) // --- Revert check to > 0 ---
            {
                comboMonitors.Enabled = true;
            }
        }

        private void PopulateSettings()
        {
            comboSettings.Items.Clear();
            // --- "..." item REMOVED ---
            comboSettings.Enabled = false;

            var selectedConnection = comboConnections.SelectedItem as DropdownItem;
            var selectedMonitor = comboMonitors.SelectedItem as DropdownItem;

            // --- Revert check: just check for null ---
            if (selectedConnection == null || selectedMonitor == null) return;


            // Find all unique VCP features for *this* monitor
            var settings = _allPluginVariables
                .Where(v => v.ConnectionSlug == selectedConnection.Value && v.PnP_ID == selectedMonitor.Value)
                .Select(v => new { v.VcpCode, v.Data.FeatureName, v.Data.Max })
                .Distinct()
                .OrderBy(s => s.FeatureName);

            foreach (var setting in settings)
            {
                comboSettings.Items.Add(new DropdownItem { Text = setting.FeatureName, Value = setting.VcpCode.ToString() });
            }

            if (comboSettings.Items.Count > 0) // --- Revert check to > 0 ---
            {
                comboSettings.Enabled = true;
            }
        }

        private void PopulateValue()
        {
            numericValue.Enabled = false;
            numericValue.Minimum = 0;
            numericValue.Maximum = 0;

            var selectedConnection = comboConnections.SelectedItem as DropdownItem;
            var selectedMonitor = comboMonitors.SelectedItem as DropdownItem;
            var selectedSetting = comboSettings.SelectedItem as DropdownItem;

            // --- Revert check: just check for null ---
            if (selectedConnection == null || selectedMonitor == null || selectedSetting == null) return;

            // Find the *one* variable that matches all three
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
        private void comboConnections_Enter(object sender, EventArgs e)
        {
            _previousConnection = comboConnections.SelectedItem as DropdownItem;
            // 1. Get FRESH data from Main every time
            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;

            // 2. Save current selection
            string currentValue = (comboConnections.SelectedItem as DropdownItem)?.Value;

            // 3. Repopulate (this also clears)
            PopulateConnections();

            // 4. Restore selection
            if (currentValue != null)
            {
                var itemToRestore = comboConnections.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == currentValue);
                if (itemToRestore != null)
                {
                    comboConnections.SelectedItem = itemToRestore;
                }
            }
        }

        private void comboMonitors_Enter(object sender, EventArgs e)
        {
            _previousMonitor = comboMonitors.SelectedItem as DropdownItem;
            // 1. Get FRESH data from Main
            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;

            // 2. Save current selection
            string currentValue = (comboMonitors.SelectedItem as DropdownItem)?.Value;

            // 3. Repopulate (based on selected connection)
            PopulateMonitors();

            // 4. Restore selection
            if (currentValue != null)
            {
                var itemToRestore = comboMonitors.Items.Cast<DropdownItem>().FirstOrDefault(x => x.Value == currentValue);
                if (itemToRestore != null)
                {
                    comboMonitors.SelectedItem = itemToRestore;
                }
            }
        }

        private void comboSettings_Enter(object sender, EventArgs e)
        {
            _previousSetting = comboSettings.SelectedItem as DropdownItem;
            // 1. Get FRESH data from Main
            _allPluginVariables = PluginInstance.Main.ParsedVcpVariables;

            // 2. Save current selection
            string currentValue = (comboSettings.SelectedItem as DropdownItem)?.Value;

            // 3. Repopulate (based on selected monitor)
            PopulateSettings();

            // 4. Restore selection
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


        #region Dropdown Events
        private void comboConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentConnection = comboConnections.SelectedItem as DropdownItem;
            if (currentConnection == _previousConnection)
            {
                return;
            }
            // --- End Check ---

            // --- Clear children ---
            comboMonitors.Items.Clear();
            comboSettings.Items.Clear();
            numericValue.Value = 0;
            numericValue.Enabled = false;

            PopulateMonitors();
            SaveConfig(); // Save partial config
        }

        private void comboMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentMonitor = comboMonitors.SelectedItem as DropdownItem;
            if (currentMonitor == _previousMonitor)
            {
                return;
            }
            // --- End Check ---

            // --- Clear children ---
            comboSettings.Items.Clear();
            numericValue.Value = 0;
            numericValue.Enabled = false;

            PopulateSettings();
            SaveConfig(); // Save partial config

        }

        private void comboSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentSetting = comboSettings.SelectedItem as DropdownItem;
            if (currentSetting == _previousSetting)
            {
                return;
            }
            // --- End Check ---

            // --- Clear children ---
            numericValue.Value = 0; // Default to 0 for a new selection

            PopulateValue();
            SaveConfig(); // Save partial config
        }


        private void Value_Changed(object sender, EventArgs e)
        {
            // This is the only one that doesn't need to repopulate children
            SaveConfig();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe from the event to prevent memory leaks
                PluginInstance.Main.OnVariableListChanged -= OnVariableListChanged;

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}