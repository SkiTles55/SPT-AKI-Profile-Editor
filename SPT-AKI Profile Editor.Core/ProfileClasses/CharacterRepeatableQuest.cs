using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterRepeatableQuest : BindableEntity
    {
        [JsonProperty("name")]
        public QuestType Type { get; set; }

        [JsonProperty("activeQuests")]
        public ActiveQuest[] ActiveQuests { get; set; }
    }
}