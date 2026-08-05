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
            ProductItem = AppData.ServerDatabase.ItemsDB.TryGetValue(production.EndProduct, out ServerClasses.TarkovItem value)
            ? value.GetExaminedItem()
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

        public string AreaLocalizedName => ExtMethods.HideoutAreaLocalizedName(Production.AreaType);
    }
}