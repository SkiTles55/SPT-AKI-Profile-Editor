using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum QuestStatus
    {
        [EnumMember(Value = "Locked")]
        Locked,

        [EnumMember(Value = "AvailableForStart")]
        AvailableForStart,

        [EnumMember(Value = "Started")]
        Started,

        [EnumMember(Value = "AvailableForFinish")]
        AvailableForFinish,

        [EnumMember(Value = "Success")]
        Success,

        [EnumMember(Value = "Fail")]
        Fail,

        [EnumMember(Value = "FailRestartable")]
        FailRestartable,

        [EnumMember(Value = "MarkedAsFailed")]
        MarkedAsFailed,

        [EnumMember(Value = "Expired")]
        Expired
    }

    public class QuestStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(QuestStatus);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                string value = reader.Value?.ToString();

                if (reader.TokenType == JsonToken.String)
                {
                    if (string.IsNullOrEmpty(value))
                        return null;

                    return Enum.Parse(typeof(QuestStatus), value);
                }

                if (reader.TokenType == JsonToken.Integer)
                {
                    var integerValue = int.Parse(value);
                    return (QuestStatus)integerValue;
                }

                return QuestStatus.Fail;
            }
            catch
            {
                return QuestStatus.Fail;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var status = (QuestStatus)value;
            writer.WriteValue(status.ToString());
        }
    }
}