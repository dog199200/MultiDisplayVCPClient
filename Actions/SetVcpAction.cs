using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System.Text.Json;

namespace MultiDisplayVCPClient.Actions
{
    /// <summary>
    /// This class implements the "Set VCP Value" action for MacroDeck.
    /// </summary>
    public class SetVcpAction : PluginAction
    {
        /// <summary>
        /// The display name of the action.
        /// </summary>
        public override string Name => "Set VCP Value";

        /// <summary>
        /// A short description of what the action does.
        /// </summary>
        public override string Description => "Sets a VCP value (e.g., Brightness, Input) on a monitor.";

        /// <summary>
        /// Indicates that this action is configurable.
        /// </summary>
        public override bool CanConfigure => true;

        /// <summary>
        /// Called when the action is executed by MacroDeck.
        /// </summary>
        /// <param name="clientId">The client ID that triggered the action (if any).</param>
        /// <param name="actionButton">The action button instance that was pressed.</param>
        public override void Trigger(string clientId, ActionButton actionButton)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Configuration)) return;
                SetVcpActionConfig? config = JsonSerializer.Deserialize<SetVcpActionConfig>(this.Configuration);
                if (config == null || string.IsNullOrEmpty(config.ConnectionName)) return;

                if (PluginInstance.Main.Connections.TryGetValue(config.ConnectionName, out VcpClient? client))
                {
                    string command = $"SET:{config.MonitorPnP_ID}:{config.VcpCode}:{config.VcpValue}";
                    _ = client.SendCommandAsync(command);

                    try
                    {
                        string connectionSlug = Main.Slugify(config.ConnectionName);

                        var variableToUpdate = PluginInstance.Main.ParsedVcpVariables
                            .FirstOrDefault(v =>
                                v.ConnectionSlug == connectionSlug &&
                                v.PnP_ID == config.MonitorPnP_ID &&
                                v.VcpCode.ToString() == config.VcpCode);

                        if (variableToUpdate.VariableName != null)
                        {
                            VariableManager.SetValue(
                                variableToUpdate.VariableName,
                                config.VcpValue.ToString(),
                                VariableType.String,
                                PluginInstance.Main,
                                []);
                        }
                    }
                    catch (Exception)
                    {
                        // Fail silently on optimistic update
                    }
                }
            }
            catch (Exception)
            {
                // Fail silently on trigger
            }
        }

        /// <summary>
        /// Returns the user control used to configure this action.
        /// </summary>
        /// <param name="actionConfigurator">The Macro Deck action configurator.</param>
        /// <returns>An instance of the SetVcpActionConfigControl.</returns>
        public override ActionConfigControl GetActionConfigControl(ActionConfigurator actionConfigurator)
        {
            return new SetVcpActionConfigControl(this);
        }
    }

    /// <summary>
    /// This class holds the configuration data for the SetVcpAction.
    /// </summary>
    public class SetVcpActionConfig
    {
        /// <summary>
        /// The friendly name of the connection to use (e.g., "Server 1").
        /// </summary>
        public string ConnectionName { get; set; } = "";

        /// <summary>
        /// The PnP Model ID of the target monitor (e.g., "ACR0D1D").
        /// </summary>
        public string MonitorPnP_ID { get; set; } = "";

        /// <summary>
        /// The VCP code to set, as a string (e.g., "16" for Brightness).
        /// </summary>
        public string VcpCode { get; set; } = "";

        /// <summary>
        /// The value to set the VCP feature to.
        /// </summary>
        public uint VcpValue { get; set; } = 0;
    }
}