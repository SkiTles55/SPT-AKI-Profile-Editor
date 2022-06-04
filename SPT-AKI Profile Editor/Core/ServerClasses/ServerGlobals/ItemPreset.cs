using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ItemPreset
    {

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("_name")]
        public string Name { get; set; }

        [JsonPropertyName("_parent")]
        public string Root { get; set; }

        [JsonPropertyName("_items")]
        public object[] Items { get; set; }
    }
}