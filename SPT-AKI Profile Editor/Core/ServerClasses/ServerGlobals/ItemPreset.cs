using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ItemPreset
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_name")]
        public string Name { get; set; }

        [JsonProperty("_parent")]
        public string Root { get; set; }

        [JsonProperty("_items")]
        public object[] Items { get; set; }
    }
}