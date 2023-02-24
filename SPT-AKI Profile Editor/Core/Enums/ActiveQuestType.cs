using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum ActiveQuestType
    {
        [EnumMember(Value = "Completion")]
        Completion,

        [EnumMember(Value = "Exploration")]
        Exploration,

        [EnumMember(Value = "Elimination")]
        Elimination,

        [EnumMember(Value = "Unknown")]
        Unknown
    }

    public static class ActiveQuestTypeExtension
    {
        public static string LocalizationKey(this ActiveQuestType type) => $"DailyQuestName/{type}";

        public static string LocalizedName(this ActiveQuestType type) =>
            AppData.ServerDatabase.LocalesGlobal.ContainsKey(type.LocalizationKey()) ? AppData.ServerDatabase.LocalesGlobal[type.LocalizationKey()] : type.ToString();
    }
}