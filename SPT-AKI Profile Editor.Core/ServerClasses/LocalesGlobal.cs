using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobal
    {
        //[JsonPropertyName("interface")]
        //public Dictionary<string, string> Interface { get; set; }

        //[JsonPropertyName("trading")]
        //public Dictionary<string, TraderLocale> Traders { get; set; }

        //[JsonPropertyName("quest")]
        //public Dictionary<string, QuestLocale> Quests { get; set; }

        //[JsonPropertyName("templates")]
        //public Dictionary<string, Template> Templates { get; set; }

        [JsonPropertyName("customization")]
        public Dictionary<string, Template> Customization { get; set; }
    }
}