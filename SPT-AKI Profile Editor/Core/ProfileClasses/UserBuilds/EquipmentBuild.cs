using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class EquipmentBuild : Build
    {
        public static readonly string EquipmentBuildType = "Custom";

        [JsonProperty("buildType")]
        public override string Type { get; set; }

        [JsonProperty("fastPanel")]
        public object[] FastPanel { get; set; }
    }
}