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
                [JsonConverter(typeof(JsonStringEnumConverter))]
                [JsonPropertyName("_parent")]
                public QuestConditionType Type { get; set; }

                [JsonPropertyName("_props")]
                public QuestConditionProps Props { get; set; }

                public enum QuestConditionType
                {
                    [EnumMember(Value = "Level")]
                    Level,

                    [EnumMember(Value = "Quest")]
                    Quest,

                    [EnumMember(Value = "TraderLoyalty")]
                    TraderLoyalty
                }

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
                }
            }
        }

        private void TempMetod(string compareMethod)
        {
            switch (compareMethod)
            {
                case ">=":
                    break;
                case ">":
                    break;
                case "<=":
                    break;
                case "<":
                    break;
                case "!=":
                    break;
                case "==":
                    break;

            }
        }
    }
}