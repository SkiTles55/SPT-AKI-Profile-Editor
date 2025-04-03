using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CustomisationUnlock
    {
        private const string suiteType = "suite";
        private const string unlockedInGameSource = "unlockedInGame";

        public CustomisationUnlock(string id)
        {
            Id = id;
            Type = suiteType;
            Source = unlockedInGameSource;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonIgnore]
        public bool IsSuitUnlock => Type == suiteType && Source == unlockedInGameSource;
    }
}