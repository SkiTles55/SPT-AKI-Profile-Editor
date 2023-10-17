using Newtonsoft.Json;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses.Configs
{
    public class Quest
    {
        [JsonProperty("eventQuests")]
        public Dictionary<string, object> EventQuests { get; set; }
    }
}