namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ItemUpd
    {
        public long? StackObjectsCount { get; set; }

        public UpdFoldable Foldable { get; set; }

        public Tag Tag { get; set; }

        public bool SpawnedInSession { get; set; }

        public DogtagProperties Dogtag { get; set; }
    }
}