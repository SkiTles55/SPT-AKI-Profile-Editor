using Newtonsoft.Json;
using System;
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

    public class ActiveQuestTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(ActiveQuestType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                string value = reader.Value?.ToString();

                if (reader.TokenType == JsonToken.String)
                {
                    if (string.IsNullOrEmpty(value))
                        return null;

                    return Enum.Parse(typeof(ActiveQuestType), value);
                }

                return ActiveQuestType.Unknown;
            }
            catch
            {
                return ActiveQuestType.Unknown;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var status = (ActiveQuestType)value;
            writer.WriteValue(status.ToString());
        }
    }
}