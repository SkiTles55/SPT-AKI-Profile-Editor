using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterHideoutProduction : BindableEntity
    {
        private bool added;

        public CharacterHideoutProduction(HideoutProduction production, bool added)
        {
            Production = production;
            Added = added;
            ProductItem = AppData.ServerDatabase.ItemsDB.ContainsKey(production.EndProduct)
            ? AppData.ServerDatabase.ItemsDB[production.EndProduct].GetExaminedItem()
            : new ExaminedItem(production.EndProduct, production.EndProduct, null);
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

        public ExaminedItem ProductItem { get; set; }

        public string AreaLocalizedName => AppData.ServerDatabase.LocalesGlobal.ContainsKey($"hideout_area_{Production.AreaType}_name") ?
            AppData.ServerDatabase.LocalesGlobal[$"hideout_area_{Production.AreaType}_name"] :
            $"hideout_area_{Production.AreaType}_name";
    }
}