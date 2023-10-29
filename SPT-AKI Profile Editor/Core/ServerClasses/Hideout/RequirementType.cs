using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout
{
    public enum RequirementType
    {
        [EnumMember(Value = "QuestComplete")]
        QuestComplete,

        Unknown
    }
}