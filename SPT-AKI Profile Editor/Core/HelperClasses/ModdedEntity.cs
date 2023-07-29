using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ModdedEntity : BindableEntity
    {
        private bool markedForRemoving;

        public ModdedEntity(string id,
                            ModdedEntityType type,
                            bool markedForRemoving,
                            string tpl = null)
        {
            Id = id;
            Type = type;
            MarkedForRemoving = markedForRemoving;
            Tpl = tpl;
        }

        public string Id { get; }

        public string Tpl { get; }
        public ModdedEntityType Type { get; }

        public bool MarkedForRemoving
        {
            get => markedForRemoving;
            set
            {
                markedForRemoving = value;
                OnPropertyChanged(nameof(MarkedForRemoving));
            }
        }

        public string LocalizedType => Type.LocalizedName();
    }
}