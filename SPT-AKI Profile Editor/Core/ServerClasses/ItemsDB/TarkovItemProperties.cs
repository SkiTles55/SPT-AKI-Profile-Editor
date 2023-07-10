using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItemProperties
    {
        [JsonProperty("Width")]
        public int Width { get; set; }

        [JsonProperty("Height")]
        public int Height { get; set; }

        [JsonProperty("StackMaxSize")]
        public int StackMaxSize { get; set; }

        [JsonProperty("ExtraSizeLeft")]
        public int ExtraSizeLeft { get; set; }

        [JsonProperty("ExtraSizeRight")]
        public int ExtraSizeRight { get; set; }

        [JsonProperty("ExtraSizeUp")]
        public int ExtraSizeUp { get; set; }

        [JsonProperty("ExtraSizeDown")]
        public int ExtraSizeDown { get; set; }

        [JsonProperty("Grids")]
        public Grid[] Grids { get; set; }

        [JsonProperty("Foldable")]
        public bool Foldable { get; set; }

        [JsonProperty("FoldedSlot")]
        public string FoldedSlot { get; set; }

        [JsonProperty("SizeReduceRight")]
        public int SizeReduceRight { get; set; }

        [JsonProperty("ExtraSizeForceAdd")]
        public bool ExtraSizeForceAdd { get; set; }

        [JsonProperty("ExaminedByDefault")]
        public bool ExaminedByDefault { get; set; }

        [JsonProperty("QuestItem")]
        public bool QuestItem { get; set; }

        [JsonProperty("Ergonomics")]
        public float Ergonomics { get; set; }

        [JsonProperty("RecoilForceUp")]
        public int RecoilForceUp { get; set; }

        [JsonProperty("RecoilForceBack")]
        public int RecoilForceBack { get; set; }

        [JsonProperty("Recoil")]
        public float Recoil { get; set; }

        [JsonProperty("DogTagQualities")]
        public bool DogTagQualities { get; set; }
    }
}