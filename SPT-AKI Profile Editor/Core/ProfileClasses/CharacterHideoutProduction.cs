using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterHideoutProduction: BindableEntity
    {
        private bool added;

        public CharacterHideoutProduction(HideoutProduction production, bool added)
        {
            Production = production;
            Added = added;
        }

        public HideoutProduction Production { get; set; }

        public bool Added
        {
            get => added;
            set
            {
                added = value;
                OnPropertyChanged(nameof(Added));
            }
        }

        public string ProductLocalizedName
            => AppData.ServerDatabase.LocalesGlobal.ContainsKey(Production.EndProduct.Name())
            ? AppData.ServerDatabase.LocalesGlobal[Production.EndProduct.Name()]
            : Production.EndProduct;
    }
}