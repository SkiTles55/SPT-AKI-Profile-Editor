using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
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

    public static class QuestTypeExtension
    {
        public static List<QuestStatus> GetAvailableStatuses(this QuestType type) => type switch
        {
            QuestType.Standart => AppData.AppSettings.standartQuestStatuses,
            _ => AppData.AppSettings.repeatableQuestStatuses,
        };
    }
}