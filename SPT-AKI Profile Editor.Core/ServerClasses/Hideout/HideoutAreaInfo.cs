using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HideoutAreaInfo
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("stages")]
        public Dictionary<string, JsonElement> Stages { get; set; }
    }
}
