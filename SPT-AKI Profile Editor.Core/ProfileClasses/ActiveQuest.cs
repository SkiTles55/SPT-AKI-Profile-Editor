using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ActiveQuest
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("traderId")]
        public string TraderId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}