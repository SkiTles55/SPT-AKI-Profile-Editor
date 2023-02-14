using Newtonsoft.Json;
using System;
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
        Daily,

        [EnumMember(Value = "Daily_Savage")]
        Daily_Savage,

        [EnumMember(Value = "Unknown")]
        Unknown
    }

    public static class QuestTypeExtension
    {
        public static List<QuestStatus> GetAvailableStatuses(this QuestType type) => type switch
        {
            QuestType.Standart => AppData.AppSettings.standartQuestStatuses,
            _ => AppData.AppSettings.repeatableQuestStatuses,
        };

        public static string LocalizedName(this QuestType type) => type switch
        {
            QuestType.Daily => AppData.AppLocalization.GetLocalizedString("tab_quests_daily_group"),
            QuestType.Weekly => AppData.AppLocalization.GetLocalizedString("tab_quests_weekly_group"),
            QuestType.Daily_Savage => AppData.AppLocalization.GetLocalizedString("tab_quests_daily_savage_group"),
            QuestType.Unknown => AppData.AppLocalization.GetLocalizedString("tab_quests_unknown_group"),
            _ => AppData.AppLocalization.GetLocalizedString("tab_quests_standart_group")
        };
    }

    public class QuestTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(QuestType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                string value = reader.Value?.ToString();

                if (reader.TokenType == JsonToken.String)
                {
                    if (string.IsNullOrEmpty(value))
                        return null;

                    return Enum.Parse(typeof(QuestType), value);
                }

                return QuestType.Unknown;
            }
            catch
            {
                return QuestType.Unknown;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var status = (QuestType)value;
            writer.WriteValue(status.ToString());
        }
    }
}