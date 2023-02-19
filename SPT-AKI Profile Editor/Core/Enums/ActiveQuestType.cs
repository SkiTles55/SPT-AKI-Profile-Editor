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
        public static string LocalizedName(this ActiveQuestType type)
        {
            var key = $"DailyQuestName/{type}";
            return AppData.ServerDatabase.LocalesGlobal.ContainsKey(key) ? AppData.ServerDatabase.LocalesGlobal[key] : type.ToString();
        }
    }
}