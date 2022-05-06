using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum QuestStatus
    {
        [EnumMember(Value = "Locked")]
        Locked,

        [EnumMember(Value = "AvailableForStart")]
        AvailableForStart,

        [EnumMember(Value = "Started")]
        Started,

        [EnumMember(Value = "AvailableForFinish")]
        AvailableForFinish,

        [EnumMember(Value = "Success")]
        Success,

        [EnumMember(Value = "Fail")]
        Fail,

        [EnumMember(Value = "FailRestartable")]
        FailRestartable,

        [EnumMember(Value = "MarkedAsFailed")]
        MarkedAsFailed
    }
}