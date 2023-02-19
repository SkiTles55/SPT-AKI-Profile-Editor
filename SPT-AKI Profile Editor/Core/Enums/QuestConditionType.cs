using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum QuestConditionType
    {
        [EnumMember(Value = "Level")]
        Level,

        [EnumMember(Value = "Quest")]
        Quest,

        [EnumMember(Value = "TraderLoyalty")]
        TraderLoyalty,

        [EnumMember(Value = "TraderStanding")]
        TraderStanding,

        [EnumMember(Value = "Unknown")]
        Unknown
    }
}