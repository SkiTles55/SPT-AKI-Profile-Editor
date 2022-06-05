using Newtonsoft.Json;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HideoutAreaInfo
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("stages")]
        public Dictionary<string, object> Stages { get; set; }
    }
}