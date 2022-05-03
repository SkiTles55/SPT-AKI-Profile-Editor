using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum ConditionType
    {
        [EnumMember(Value = "Quest")]
        Quest,

        [EnumMember(Value = "Level")]
        Level
    }
}