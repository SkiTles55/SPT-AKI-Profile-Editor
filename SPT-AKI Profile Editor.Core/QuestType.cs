using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core
{
    public enum QuestType
    {
        [EnumMember(Value = "Standart")]
        Standart,
        [EnumMember(Value = "Weekly")]
        Weekly,
        [EnumMember(Value = "Daily")]
        Daily
    }
}
