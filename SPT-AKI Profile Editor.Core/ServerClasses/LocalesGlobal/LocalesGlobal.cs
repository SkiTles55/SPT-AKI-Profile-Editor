using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobal
    {
        [JsonPropertyName("interface")]
        public Dictionary<string, string> Interface { get; set; }

        [JsonPropertyName("trading")]
        public Dictionary<string, LocalesGlobalTrading> Trading { get; set; }

        [JsonPropertyName("quest")]
        public Dictionary<string, LocalesGlobalQuest> Quests { get; set; }

        [JsonPropertyName("templates")]
        public Dictionary<string, LocalesGlobalTemplate> Templates { get; set; }

        [JsonPropertyName("customization")]
        public Dictionary<string, LocalesGlobalTemplate> Customization { get; set; }
    }
}