using SPT_AKI_Profile_Editor.Core.Enums;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{    
    public class QuestCondition
    {
        [JsonPropertyName("_parent")]
        public ConditionType Type { get; set; }
    }
}