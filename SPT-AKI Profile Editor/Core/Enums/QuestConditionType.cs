using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum QuestConditionType
    {
        [EnumMember(Value = "Level")]
        Level,

        [EnumMember(Value = "Quest")]
        Quest,

        [EnumMember(Value = "TraderLoyalty")]
        TraderLoyalty,

        [EnumMember(Value = "TraderStanding")]
        TraderStanding,

        [EnumMember(Value = "Unknown")]
        Unknown
    }

    public class QuestConditionTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(QuestConditionType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                string value = reader.Value?.ToString();

                if (reader.TokenType == JsonToken.String)
                {
                    if (string.IsNullOrEmpty(value))
                        return null;

                    return Enum.Parse(typeof(QuestConditionType), value);
                }

                return QuestConditionType.Unknown;
            }
            catch
            {
                return QuestConditionType.Unknown;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var status = (QuestConditionType)value;
            writer.WriteValue(status.ToString());
        }
    }
}