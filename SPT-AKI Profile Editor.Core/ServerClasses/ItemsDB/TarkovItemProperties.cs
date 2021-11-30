using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItemProperties
    {
        [JsonPropertyName("Width")]
        public int Width { get; set; }

        [JsonPropertyName("Height")]
        public int Height { get; set; }

        [JsonPropertyName("StackMaxSize")]
        public int StackMaxSize { get; set; }

        [JsonPropertyName("ExtraSizeLeft")]
        public int ExtraSizeLeft { get; set; }

        [JsonPropertyName("ExtraSizeRight")]
        public int ExtraSizeRight { get; set; }

        [JsonPropertyName("ExtraSizeUp")]
        public int ExtraSizeUp { get; set; }

        [JsonPropertyName("ExtraSizeDown")]
        public int ExtraSizeDown { get; set; }

        [JsonPropertyName("Grids")]
        public Grid[] Grids { get; set; }

        [JsonPropertyName("Foldable")]
        public bool Foldable { get; set; }

        [JsonPropertyName("FoldedSlot")]
        public string FoldedSlot { get; set; }

        [JsonPropertyName("SizeReduceRight")]
        public int SizeReduceRight { get; set; }

        [JsonPropertyName("ExtraSizeForceAdd")]
        public bool ExtraSizeForceAdd { get; set; }

        [JsonPropertyName("ExaminedByDefault")]
        public bool ExaminedByDefault { get; set; }

        [JsonPropertyName("QuestItem")]
        public bool QuestItem { get; set; }
    }
}