using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterRepeatableQuest
    {
        [JsonConverter(typeof(SafeEnumConverter<QuestType>))]
        [JsonProperty("name")]
        public QuestType Type { get; set; }

        [JsonProperty("activeQuests")]
        public ActiveQuest[] ActiveQuests { get; set; }

        [JsonProperty("inactiveQuests")]
        public ActiveQuest[] InactiveQuests { get; set; }
    }
}