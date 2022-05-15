using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor
{
    public class WeaponBuildWindowViewModel : BindableViewModel
    {
        private readonly InventoryItem _item;
        private readonly StashEditMode _editMode;
        private WeaponBuild weaponBuild;

        public WeaponBuildWindowViewModel(InventoryItem item, StashEditMode editMode)
        {
            WindowTitle = item.LocalizedName;
            _item = item;
            _editMode = editMode;
            WeaponBuild = new WeaponBuild(_item, new List<InventoryItem>() { _item });
        }

        public string WindowTitle { get; }

        public WeaponBuild WeaponBuild
        {
            get => weaponBuild;
            set
            {
                weaponBuild = value;
                OnPropertyChanged("WeaponBuild");
            }
        }

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
            {
                switch (_editMode)
                {
                    case StashEditMode.Scav:
                        Profile.Characters.Scav.Inventory.RemoveItems(new() { _item.Id });
                        break;
                    case StashEditMode.PMC:
                        Profile.Characters.Pmc.Inventory.RemoveItems(new() { _item.Id });
                        break;
                }
            }
        });
    }
}