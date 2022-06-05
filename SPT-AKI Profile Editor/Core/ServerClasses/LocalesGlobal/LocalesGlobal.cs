using System.Collections.Generic;
using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobal
    {
        [JsonProperty("interface")]
        public Dictionary<string, string> Interface { get; set; }

        [JsonProperty("trading")]
        public Dictionary<string, LocalesGlobalTrading> Trading { get; set; }

        [JsonProperty("quest")]
        public Dictionary<string, LocalesGlobalQuest> Quests { get; set; }

        [JsonProperty("templates")]
        public Dictionary<string, LocalesGlobalTemplate> Templates { get; set; }

        [JsonProperty("handbook")]
        public Dictionary<string, string> Handbook { get; set; }

        [JsonProperty("customization")]
        public Dictionary<string, LocalesGlobalTemplate> Customization { get; set; }

        [JsonProperty("preset")]
        public Dictionary<string, LocalesGlobalPreset> Preset { get; set; }
    }
}