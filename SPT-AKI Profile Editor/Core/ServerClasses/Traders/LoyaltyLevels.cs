using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LoyaltyLevels
    {
        [JsonProperty("minLevel")]
        public int MinLevel { get; set; }

        [JsonProperty("minSalesSum")]
        public long MinSalesSum { get; set; }

        [JsonProperty("minStanding")]
        public float MinStanding { get; set; }
    }
}