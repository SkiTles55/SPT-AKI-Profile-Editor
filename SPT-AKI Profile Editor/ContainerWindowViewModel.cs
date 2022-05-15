using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor
{
    public class ContainerWindowViewModel : BindableViewModel
    {
        private readonly InventoryItem _item;
        private readonly StashEditMode _editMode;

        public ContainerWindowViewModel(InventoryItem item, StashEditMode editMode)
        {
            WindowTitle = item.LocalizedName;
            _item = item;
            _editMode = editMode;
        }

        public RelayCommand OpenContainer => new(obj => App.OpenContainerWindow(obj, _editMode));

        public RelayCommand InspectWeapon => new(obj => App.OpenWeaponBuildWindow(obj, _editMode));

        public string WindowTitle { get; }

        public ObservableCollection<InventoryItem> Items => _editMode switch
        {
            StashEditMode.Scav => new(Profile.Characters.Scav.Inventory.Items?.Where(x => x.ParentId == _item.Id)),
            _ => new(Profile.Characters.Pmc.Inventory.Items?.Where(x => x.ParentId == _item.Id)),
        };

        public bool HasItems => Items.Count > 0;

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj == null)
                return;
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
            {
                switch (_editMode)
                {
                    case StashEditMode.Scav:
                        Profile.Characters.Scav.Inventory.RemoveItems(new() { obj.ToString() });
                        break;
                    case StashEditMode.PMC:
                        Profile.Characters.Pmc.Inventory.RemoveItems(new() { obj.ToString() });
                        break;
                }
                OnPropertyChanged("");
            }
        });

        public RelayCommand RemoveAllItems => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_items_caption"))
            {
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => {
                        switch (_editMode)
                        {
                            case StashEditMode.Scav:
                                Profile.Characters.Scav.Inventory.RemoveItems(Items.Select(x => x.Id).ToList());
                                break;
                            case StashEditMode.PMC:
                                Profile.Characters.Pmc.Inventory.RemoveItems(Items.Select(x => x.Id).ToList());
                                break;
                        }
                        OnPropertyChanged("");
                    },
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                });
            }
        });
    }
}