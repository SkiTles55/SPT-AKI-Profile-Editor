using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItem
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("_name")]
        public string Name { get; set; }

        [JsonPropertyName("_props")]
        public TarkovItemProperties Properties { get; set; }

        [JsonPropertyName("_parent")]
        public string Parent { get; set; }

        [JsonPropertyName("_type")]
        public string Type { get; set; }
    }
}