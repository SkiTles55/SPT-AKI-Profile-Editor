namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItemProperties
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public int StackMaxSize { get; set; }

        public int ExtraSizeLeft { get; set; }

        public int ExtraSizeRight { get; set; }

        public int ExtraSizeUp { get; set; }

        public int ExtraSizeDown { get; set; }

        public Grid[] Grids { get; set; }

        public bool Foldable { get; set; }

        public string FoldedSlot { get; set; }

        public int SizeReduceRight { get; set; }

        public bool ExtraSizeForceAdd { get; set; }

        public bool ExaminedByDefault { get; set; }

        public bool QuestItem { get; set; }

        public float Ergonomics { get; set; }

        public float RecoilForceUp { get; set; }

        public float RecoilForceBack { get; set; }

        public float Recoil { get; set; }

        public bool DogTagQualities { get; set; }
    }
}