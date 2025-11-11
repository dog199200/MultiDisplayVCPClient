using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using System;
using System.Linq;
using System.Text.Json;
using SuchByte.MacroDeck.Variables;
using SuchByte.MacroDeck.Logging;

namespace MultiDisplayVCPClient.Actions
{
    public class SetVcpAction : PluginAction
    {
        public override string Name => "Set VCP Value";
        public override string Description => "Sets a VCP value (e.g., Brightness, Input) on a monitor.";
        public override bool CanConfigure => true;

        // --- THIS METHOD IS UPDATED ---
        public override void Trigger(string clientId, ActionButton actionButton)
        {
            try
            {
                var config = JsonSerializer.Deserialize<SetVcpActionConfig>(this.Configuration);
                if (config == null || string.IsNullOrEmpty(config.ConnectionName)) return;

                // 1. Find the VcpClient
                if (PluginInstance.Main.Connections.TryGetValue(config.ConnectionName, out VcpClient client))
                {
                    // 2. Build and send the command (this part was correct)
                    string command = $"SET:{config.MonitorPnP_ID}:{config.VcpCode}:{config.VcpValue}";
                    _ = client.SendCommandAsync(command);

                    // --- 3. NEW "Optimistic Update" Logic ---
                    try
                    {
                        // We must find the *correct* variable name from our in-memory list
                        // based on the action's saved config.

                        string connectionSlug = PluginInstance.Main.Slugify(config.ConnectionName);

                        var variableToUpdate = PluginInstance.Main.ParsedVcpVariables
                            .FirstOrDefault(v =>
                                v.ConnectionSlug == connectionSlug &&
                                v.PnP_ID == config.MonitorPnP_ID &&
                                v.VcpCode.ToString() == config.VcpCode);

                        if (variableToUpdate.VariableName != null)
                        {
                            // We found it! Now update the simple value in Macro Deck.
                            VariableManager.SetValue(
                                variableToUpdate.VariableName,
                                config.VcpValue.ToString(),
                                VariableType.String,
                                PluginInstance.Main,
                                false); // 'false' = don't save to DB, it's temporary

                            MacroDeckLogger.Info(PluginInstance.Main, $"SetVcpAction optimistically set {variableToUpdate.VariableName} to {config.VcpValue}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MacroDeckLogger.Error(PluginInstance.Main, $"SetVcpAction failed optimistic update: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(PluginInstance.Main, $"SetVcpAction Trigger Error: {ex.Message}");
            }
        }

        public override ActionConfigControl GetActionConfigControl(ActionConfigurator actionConfigurator)
        {
            return new SetVcpActionConfigControl(this);
        }
    }

    // --- THIS CLASS IS UPDATED ---
    public class SetVcpActionConfig
    {
        // ConnectionName is still just the friendly name
        public string ConnectionName { get; set; } = "";

        // This will now store the PnP ID (e.g., "ACR0D1D")
        public string MonitorPnP_ID { get; set; } = "";

        // This will now store the VCP code (e.g., "16" for Brightness)
        public string VcpCode { get; set; } = "";

        // This is the value the user wants to set
        public uint VcpValue { get; set; } = 0;
    }
}