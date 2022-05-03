using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum IssuesAction
    {
        [EnumMember(Value = "AlwaysShow")]
        AlwaysShow,

        [EnumMember(Value = "AlwaysIgnore")]
        AlwaysIgnore,

        [EnumMember(Value = "AlwaysFix")]
        AlwaysFix
    }
}
