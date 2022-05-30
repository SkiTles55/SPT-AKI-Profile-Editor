using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor
{
    public class ContainerWindowViewModel : BindableViewModel
    {
        private readonly InventoryItem _item;
        private readonly StashEditMode _editMode;
        private ObservableCollection<HandbookCategory> categoriesForItemsAdding;

        public ContainerWindowViewModel(InventoryItem item, StashEditMode editMode, IDialogCoordinator dialogCoordinator)
        {
            Worker = new Worker(dialogCoordinator, this);
            WindowTitle = item.LocalizedName;
            _item = item;
            _editMode = editMode;
        }

        public Worker Worker { get; }

        public RelayCommand OpenContainer => new(obj => App.OpenContainerWindow(obj, _editMode));

        public RelayCommand InspectWeapon => new(obj => App.OpenWeaponBuildWindow(obj, _editMode));

        public string WindowTitle { get; }

        public ObservableCollection<InventoryItem> Items => _editMode switch
        {
            StashEditMode.Scav => new(Profile.Characters.Scav.Inventory.Items?.Where(x => x.ParentId == _item.Id)),
            _ => new(Profile.Characters.Pmc.Inventory.Items?.Where(x => x.ParentId == _item.Id)),
        };

        public bool HasItems => Items.Count > 0;

        public bool ItemsAddingAllowed => _item.CanAddItems && CategoriesForItemsAdding.Count > 0;

        public ObservableCollection<HandbookCategory> CategoriesForItemsAdding
        {
            get
            {
                if (categoriesForItemsAdding == null)
                    categoriesForItemsAdding = new ObservableCollection<HandbookCategory>(ServerDatabase.Handbook.Categories
                        .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden)
                        .Select(x => FilterForConatiner(HandbookCategory.CopyFrom(x)))
                        .Where(x => x.IsNotHidden));
                return categoriesForItemsAdding;
            }
            set
            {
                categoriesForItemsAdding = value;
                OnPropertyChanged("CategoriesForItemsAdding");
            }
        }

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
                Worker.AddAction(new WorkerTask
                {
                    Action = () =>
                    {
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

        public RelayCommand AddItem => new(obj =>
        {
            if (obj == null || obj is not TarkovItem item)
                return;
            Worker.AddAction(new WorkerTask
            {
                Action = () =>
                {
                    switch (_editMode)
                    {
                        case StashEditMode.Scav:
                            Profile.Characters.Scav.Inventory.AddNewItemsToContainer(_item, item, item.AddingQuantity, item.AddingFir, "main");
                            break;

                        case StashEditMode.PMC:
                            Profile.Characters.Pmc.Inventory.AddNewItemsToContainer(_item, item, item.AddingQuantity, item.AddingFir, "main");
                            break;
                    }
                    OnPropertyChanged("");
                }
            });
        });

        private HandbookCategory FilterForConatiner(HandbookCategory category)
        {
            if (ServerDatabase.ItemsDB.ContainsKey(_item.Tpl))
            {
                category.Items = new ObservableCollection<TarkovItem>(category.Items.Where(x => x.CanBeAddedToContainer(ServerDatabase.ItemsDB[_item.Tpl])));
                category.Categories = new ObservableCollection<HandbookCategory>(category.Categories.Select(x => FilterForConatiner(x)).Where(x => x.IsNotHidden));
            }
            return category;
        }
    }
}