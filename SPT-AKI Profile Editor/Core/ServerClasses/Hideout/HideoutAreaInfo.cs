using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HideoutAreaInfo
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("stages")]
        public Dictionary<string, object> Stages { get; set; }
    }
}