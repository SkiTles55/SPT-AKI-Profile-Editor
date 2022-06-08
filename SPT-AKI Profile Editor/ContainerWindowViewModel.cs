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
        private ObservableCollection<AddableCategory> categoriesForItemsAdding;

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

        public ObservableCollection<InventoryItem> Items => new(GetInventory().Items?.Where(x => x.ParentId == _item.Id));

        public bool HasItems => Items.Count > 0;

        public bool ItemsAddingAllowed => _item.CanAddItems && CategoriesForItemsAdding.Count > 0;

        public ObservableCollection<AddableCategory> CategoriesForItemsAdding
        {
            get
            {
                if (categoriesForItemsAdding == null)
                    categoriesForItemsAdding = new ObservableCollection<AddableCategory>(ServerDatabase.Handbook.Categories
                        .Select(x => FilterForConatiner(HandbookCategory.CopyFrom(x)))
                        .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden));
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
                RemoveItemFromContainer(obj.ToString());
        });

        public RelayCommand RemoveAllItems => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_items_caption"))
            {
                Worker.AddAction(new WorkerTask
                {
                    Action = () => RemoveAllItemsFromContainer(),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                });
            }
        });

        public RelayCommand AddItem => new(obj =>
        {
            if (obj == null || obj is not TarkovItem item)
                return;
            Worker.AddAction(new WorkerTask { Action = () => AddItemToContainer(item) });
        });

        private void RemoveItemFromContainer(string id)
        {
            GetInventory().RemoveItems(new() { id });
            OnPropertyChanged("");
        }

        private void RemoveAllItemsFromContainer()
        {
            GetInventory().RemoveItems(Items.Select(x => x.Id).ToList());
            OnPropertyChanged("");
        }

        private void AddItemToContainer(TarkovItem item)
        {
            GetInventory().AddNewItemsToContainer(_item, item, item.AddingQuantity, item.AddingFir, "main");
            OnPropertyChanged("");
        }

        private CharacterInventory GetInventory()
        {
            return _editMode switch
            {
                StashEditMode.Scav => Profile.Characters.Scav.Inventory,
                _ => Profile.Characters.Pmc.Inventory,
            };
        }

        private AddableCategory FilterForConatiner(AddableCategory category)
        {
            if (ServerDatabase.ItemsDB.ContainsKey(_item.Tpl))
            {
                category.Items = new ObservableCollection<AddableItem>(category.Items.Where(x => x.CanBeAddedToContainer(ServerDatabase.ItemsDB[_item.Tpl])));
                category.Categories = new ObservableCollection<AddableCategory>(category.Categories.Select(x => FilterForConatiner(x)).Where(x => x.IsNotHidden));
            }
            return category;
        }
    }
}