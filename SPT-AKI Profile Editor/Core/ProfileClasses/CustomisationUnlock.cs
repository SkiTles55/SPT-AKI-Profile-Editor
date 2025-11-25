using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CustomisationUnlock(string id)
    {
        private const string suiteType = "suite";
        private const string unlockedInGameSource = "unlockedInGame";

        [JsonProperty("id")]
        public string Id { get; set; } = id;

        [JsonProperty("source")]
        public string Source { get; set; } = unlockedInGameSource;

        [JsonProperty("type")]
        public string Type { get; set; } = suiteType;

        [JsonIgnore]
        public bool IsSuitUnlock => Type == suiteType && Source == unlockedInGameSource;
    }
}