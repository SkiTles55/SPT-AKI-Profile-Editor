using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
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
        private readonly CharacterInventory _inventory;
        private readonly IDialogManager _dialogManager;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;
        private ObservableCollection<AddableCategory> categoriesForItemsAdding;

        public ContainerWindowViewModel(InventoryItem item,
                                        CharacterInventory inventory,
                                        IDialogCoordinator dialogCoordinator,
                                        IApplicationManager applicationManager,
                                        bool editingAllowed,
                                        IDialogManager dialogManager = null,
                                        IWorker worker = null)
        {
            _dialogManager = dialogManager ?? new MetroDialogManager(this, dialogCoordinator);
            _worker = worker ?? new Worker(_dialogManager);
            WindowTitle = item.LocalizedName;
            _item = item;
            _inventory = inventory;
            _applicationManager = applicationManager;
            EditingAllowed = editingAllowed;
        }

        public RelayCommand OpenContainer => new(obj =>
        {
            if (obj is InventoryItem item)
                _applicationManager.OpenContainerWindow(item, _inventory, EditingAllowed);
        });

        public RelayCommand InspectWeapon => new(obj =>
        {
            if (obj is InventoryItem item)
                _applicationManager.OpenWeaponBuildWindow(item, _inventory, EditingAllowed);
        });

        public string WindowTitle { get; }

        public bool EditingAllowed { get; }

        public ObservableCollection<InventoryItem> Items
            => new(_inventory.Items?.Where(x => x.ParentId == _item.Id));

        public bool HasItems => Items.Count > 0;

        public bool ItemsAddingAllowed
            => _item.CanAddItems && CategoriesForItemsAdding.Count > 0 && EditingAllowed;

        public bool ItemsAddingBlocked
            => !ItemsAddingAllowed || Items.Where(x => !x.IsInItemsDB).Any();

        public ObservableCollection<AddableCategory> CategoriesForItemsAdding
        {
            get
            {
                categoriesForItemsAdding ??= ServerDatabase.HandbookHelper.CategoriesForItemsAddingWithFilter(_item.Tpl);
                return categoriesForItemsAdding;
            }
        }

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj is string id && await _dialogManager.YesNoDialog("remove_stash_item_title",
                                                                     "remove_stash_item_caption"))
                RemoveItemFromContainer(id);
        });

        public RelayCommand RemoveAllItems => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog("remove_stash_item_title", "remove_stash_items_caption"))
                _worker.AddTask(new(() => RemoveAllItemsFromContainer(),
                                    AppLocalization.GetLocalizedString("progress_dialog_title"),
                                    AppLocalization.GetLocalizedString("remove_stash_item_title")));
        });

        public RelayCommand AddItem => new(obj =>
        {
            if (obj is AddableItem item)
                _worker.AddTask(new(() => AddItemToContainer(item), null, null));
        });

        public RelayCommand ShowAllItems
            => new(async obj => await _dialogManager.ShowAllItemsDialog(AddItem, false));

        private void RemoveItemFromContainer(string id)
        {
            _inventory.RemoveItems(new() { id });
            OnPropertyChanged("");
        }

        private void RemoveAllItemsFromContainer()
        {
            _inventory.RemoveItems(Items.Select(x => x.Id).ToList());
            OnPropertyChanged("");
        }

        private void AddItemToContainer(AddableItem item)
        {
            _inventory.AddNewItemsToContainer(_item, item, "main");
            OnPropertyChanged("");
        }
    }
}