using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout;
using System;

namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public class SafeEnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                string value = reader.Value?.ToString();

                if (reader.TokenType == JsonToken.String && !string.IsNullOrEmpty(value))
                    return (T)Enum.Parse(typeof(T), value);

                if (reader.TokenType == JsonToken.Integer)
                    return (T)(object)int.Parse(value);

                return (T)GetDefaultValue();
            }
            catch
            {
                return (T)GetDefaultValue();
            }
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer) => writer.WriteValue(value.ToString());

        private static Enum GetDefaultValue()
        {
            if (typeof(T) == typeof(QuestType))
                return QuestType.Unknown;

            if (typeof(T) == typeof(ActiveQuestType))
                return ActiveQuestType.Unknown;

            if (typeof(T) == typeof(QuestConditionType))
                return QuestConditionType.Unknown;

            if (typeof(T) == typeof(QuestStatus))
                return QuestStatus.Fail;

            if (typeof(T) == typeof(RequirementType))
                return RequirementType.Unknown;

            return default;
        }
    }
}