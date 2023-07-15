using SPT_AKI_Profile_Editor.Core.Enums;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ModdedEntity
    {
        public ModdedEntity(string id,
                            ModdedEntityType type,
                            bool canBeRemovedWithoutSave,
                            bool markedForRemoving)
        {
            Id = id;
            Type = type;
            CanBeRemovedWithoutSave = canBeRemovedWithoutSave;
            MarkedForRemoving = markedForRemoving;
        }

        public string Id { get; }
        public ModdedEntityType Type { get; }
        public bool CanBeRemovedWithoutSave { get; }
        public bool MarkedForRemoving { get; set; }
        public string LocalizedType => Type.LocalizedName();
    }
}