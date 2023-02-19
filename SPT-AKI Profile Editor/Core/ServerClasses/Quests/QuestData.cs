using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class QuestData
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("traderId")]
        public string TraderId { get; set; }

        [JsonProperty("conditions")]
        public QuestConditions Conditions { get; set; }

        public class QuestConditions
        {
            [JsonProperty("AvailableForStart")]
            public List<QuestCondition> AvailableForStart { get; set; }

            public class QuestCondition
            {
                [JsonConverter(typeof(SafeEnumConverter<QuestConditionType>))]
                [JsonProperty("_parent")]
                public QuestConditionType Type { get; set; }

                [JsonProperty("_props")]
                public QuestConditionProps Props { get; set; }

                public class QuestConditionProps
                {
                    [JsonProperty("target")]
                    public string Target { get; set; }

                    [JsonProperty("value")]
                    public int RequiredValue { get; set; }

                    [JsonProperty("compareMethod")]
                    public string CompareMethod { get; set; }

                    [JsonProperty("status")]
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

                    public float GetNearestFloatValue()
                    {
                        return CompareMethod switch
                        {
                            ">=" => RequiredValue,
                            ">" => RequiredValue + 0.01f,
                            "<=" => RequiredValue,
                            "<" => RequiredValue - 0.01f,
                            "!=" => RequiredValue + 0.01f,
                            "==" => RequiredValue,
                            _ => RequiredValue,
                        };
                    }
                }
            }
        }
    }
}