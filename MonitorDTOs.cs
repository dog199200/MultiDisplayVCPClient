using System.Text.Json.Serialization;

namespace MultiDisplayVCPClient
{
    // The top-level object sent to the client (e.g., Macro Deck)
    public class ServerStatus
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("monitors")]
        public List<MonitorInfo> Monitors { get; set; } = new List<MonitorInfo>();
    }

    // Details about a single monitor
    public class MonitorInfo
    {
        // CRITICAL: The unique device path used by the client for commands.
        [JsonPropertyName("id")]
        public string DeviceID { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("capabilities")]
        public List<VcpFeature> Capabilities { get; set; } = new List<VcpFeature>();
    }

    // Details about a single VCP (Virtual Control Panel) feature (e.g., Brightness, Input)
    public class VcpFeature
    {
        [JsonPropertyName("code")]
        public byte Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } // "Continuous" or "Non-Continuous"

        [JsonPropertyName("readWrite")]
        public bool ReadWrite { get; set; }

        [JsonPropertyName("current")]
        public uint CurrentValue { get; set; }

        [JsonPropertyName("max")]
        public uint MaximumValue { get; set; }

        // Only used for Non-Continuous features like Input Select (0x60)
        [JsonPropertyName("nonContinuousValues")]
        public Dictionary<uint, string> NonContinuousValues { get; set; } = new Dictionary<uint, string>();
    }
}