using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout
{
    public class HideoutProductionRequirement
    {
        [JsonConverter(typeof(SafeEnumConverter<RequirementType>))]
        [JsonProperty("type")]
        public RequirementType Type { get; set; }

        [JsonProperty("questId")]
        public string QuestId { get; set; }
    }
}