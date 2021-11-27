using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class UpdFoldable
    {
        [JsonProperty("Folded")]
        public bool Folded { get; set; }
    }
}