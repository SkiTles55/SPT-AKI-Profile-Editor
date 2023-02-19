using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ActiveQuest
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("traderId")]
        public string TraderId { get; set; }

        [JsonConverter(typeof(SafeEnumConverter<ActiveQuestType>))]
        [JsonProperty("type")]
        public ActiveQuestType Type { get; set; }
    }
}