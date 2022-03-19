using SPT_AKI_Profile_Editor.Core.Enums;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core
{
    public static class QuestTypeExtension
    {
        public static List<QuestStatus> GetAvailableStatuses(this QuestType type) => type switch
        {
            QuestType.Standart => AppData.AppSettings.standartQuestStatuses,
            _ => AppData.AppSettings.repeatableQuestStatuses,
        };
    }
}