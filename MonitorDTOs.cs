using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MultiDisplayVCPClient
{
    /// <summary>
    /// Represents the top-level status object sent from the server to the client.
    /// </summary>
    public class ServerStatus
    {
        /// <summary>
        /// A status message for the client (e.g., "OK: Found 2 monitors.").
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// A list of all DDC/CI compliant monitors discovered by the server.
        /// </summary>
        [JsonPropertyName("monitors")]
        public List<MonitorInfo> Monitors { get; set; } = [];
    }

    /// <summary>
    /// Represents all the information for a single physical monitor.
    /// </summary>
    public class MonitorInfo
    {
        /// <summary>
        /// The stable, unique Plug-and-Play Model ID for the monitor (e.g., "ACR0D1D").
        /// This is used by the client as the primary identifier for SET commands.
        /// </summary>
        [JsonPropertyName("id")]
        public string DeviceID { get; set; } = string.Empty;

        /// <summary>
        /// The monitor's DDC/CI description name.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// A list of all VCP features (e.g., Brightness) discovered for this monitor.
        /// </summary>
        [JsonPropertyName("capabilities")]
        public List<VcpFeature> Capabilities { get; set; } = [];
    }

    /// <summary>
    /// Represents a single VCP (Virtual Control Panel) feature of a monitor.
    /// </summary>
    public class VcpFeature
    {
        /// <summary>
        /// The VCP hex code for this feature (e.g., 0x10 for Brightness).
        /// </summary>
        [JsonPropertyName("code")]
        public byte Code { get; set; }

        /// <summary>
        /// The friendly name of the feature (e.g., "Brightness").
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The type of feature: "Continuous" or "Non-Continuous".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the feature is readable and writable.
        /// </summary>
        [JsonPropertyName("readWrite")]
        public bool ReadWrite { get; set; }

        /// <summary>
        /// The current value of the feature (e.g., 50).
        /// </summary>
        [JsonPropertyName("current")]
        public uint CurrentValue { get; set; }

        /// <summary>
        /// The maximum possible value for a continuous feature (e.g., 100).
        /// </summary>
        [JsonPropertyName("max")]
        public uint MaximumValue { get; set; }

        /// <summary>
        /// For "Non-Continuous" features, a dictionary of possible values.
        /// (e.g., [1, "HDMI 1"], [3, "DisplayPort"]).
        /// </summary>
        [JsonPropertyName("nonContinuousValues")]
        public Dictionary<uint, string> NonContinuousValues { get; set; } = [];
    }
}