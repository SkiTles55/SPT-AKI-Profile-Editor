using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class UnlockedInfo
    {
        [JsonProperty("unlockedProductionRecipe")]
        public string[] UnlockedProductionRecipe { get; set; }
    }
}