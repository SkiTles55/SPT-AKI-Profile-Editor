using SPT_AKI_Profile_Editor.Core.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class QuestData
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("traderId")]
        public string TraderId { get; set; }

        [JsonPropertyName("conditions")]
        public QuestConditions Conditions { get; set; }

        public class QuestConditions
        {
            [JsonPropertyName("AvailableForStart")]
            public List<QuestCondition> AvailableForStart { get; set; }

            public class QuestCondition
            {
                public enum QuestConditionType
                {
                    [EnumMember(Value = "Level")]
                    Level,

                    [EnumMember(Value = "Quest")]
                    Quest,

                    [EnumMember(Value = "TraderLoyalty")]
                    TraderLoyalty
                }

                [JsonConverter(typeof(JsonStringEnumConverter))]
                [JsonPropertyName("_parent")]
                public QuestConditionType Type { get; set; }

                [JsonPropertyName("_props")]
                public QuestConditionProps Props { get; set; }

                public class QuestConditionProps
                {
                    [JsonPropertyName("target")]
                    public string Target { get; set; }

                    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
                    [JsonPropertyName("value")]
                    public int RequiredValue { get; set; }

                    [JsonPropertyName("compareMethod")]
                    public string CompareMethod { get; set; }

                    [JsonPropertyName("status")]
                    public QuestStatus[] RequiredStatuses { get; set; }

                    public bool CheckRequiredValue(int currentValue)
                    {
                        return CompareMethod switch
                        {
                            ">=" => currentValue >= RequiredValue,
                            ">" => currentValue > RequiredValue,
                            "<=" => currentValue <= RequiredValue,
                            "<" => currentValue < RequiredValue,
                            "!=" => currentValue != RequiredValue,
                            "==" => currentValue == RequiredValue,
                            _ => true,
                        };
                    }

                    public int GetNearestValue()
                    {
                        return CompareMethod switch
                        {
                            ">=" => RequiredValue,
                            ">" => RequiredValue + 1,
                            "<=" => RequiredValue,
                            "<" => RequiredValue - 1,
                            "!=" => RequiredValue + 1,
                            "==" => RequiredValue,
                            _ => RequiredValue,
                        };
                    }
                }
            }
        }
    }
}