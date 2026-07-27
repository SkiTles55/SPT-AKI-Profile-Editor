using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
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
            WindowTitle = item?.LocalizedName ?? "";
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
                _worker.AddTask(ProgressTask(() => RemoveAllItemsFromContainer(),
                                    AppLocalization.GetLocalizedString("remove_stash_item_title")));
        });

        public RelayCommand AddItem => new(obj =>
        {
            if (obj is AddableItem item)
                _worker.AddTask(ProgressTask(() => AddItemToContainer(item)));
        });

        public RelayCommand AddAllKeys => new(obj =>
        {
            if (!AddAllKeysAllowed)
                return;

            _worker.AddTask(ProgressTask(() => AddAllKeysToContainer(),
                AppLocalization.GetLocalizedString("container_window_add_all_keys")));
        });

        public RelayCommand ShowAllItems
            => new(async obj => await _dialogManager.ShowAllItemsDialog(AddItem, false));

        public bool AddAllKeysAllowed => EditingAllowed && IsKeyContainer();

        private void RemoveItemFromContainer(string id)
        {
            _inventory.RemoveItems([id]);
            OnPropertyChanged("");
        }

        private void RemoveAllItemsFromContainer()
        {
            _inventory.RemoveItems([.. Items.Select(x => x.Id)]);
            OnPropertyChanged("");
        }

        private void AddItemToContainer(AddableItem item)
        {
            _inventory.AddNewItemsToContainer(_item, item, "main");
            OnPropertyChanged("");
        }

        private void AddAllKeysToContainer()
        {
            if (!_item.IsInItemsDB || !_item.CanAddItems)
                return;

            if (!AppData.ServerDatabase.ItemsDB.TryGetValue(_item.Tpl, out TarkovItem containerTpl))
                return;

            var existingItemTpls = Items.Select(x => x.Tpl).ToHashSet();
            var keyItems = GetAllKeyItems()
                .Where(x => !existingItemTpls.Contains(x.Id))
                .Where(x => x.CanBeAddedToContainer(containerTpl))
                .OrderBy(x => (x.Properties?.Width ?? 0) * (x.Properties?.Height ?? 0))
                .ToList();

            foreach (var keyItem in keyItems)
            {
                try
                {
                    _inventory.AddNewItemsToContainer(_item, keyItem, "main");
                }
                catch (Exception ex)
                {
                    if (ex.Message == AppLocalization.GetLocalizedString("tab_stash_no_slots"))
                        break;

                    throw;
                }
            }

            OnPropertyChanged("");
        }

        private IEnumerable<TarkovItem> GetAllKeyItems()
        {
            if (AppData.ServerDatabase.Handbook?.Items != null)
            {
                var keyCategoryIds = GetKeyCategoryIds();
                if (keyCategoryIds.Count > 0)
                {
                    return AppData.ServerDatabase.Handbook.Items
                        .Where(x => keyCategoryIds.Contains(x.ParentId))
                        .Select(x => AppData.ServerDatabase.ItemsDB.TryGetValue(x.Id, out TarkovItem item) ? item : null)
                        .Where(x => x != null && !x.IsQuestItem);
                }
            }

            return AppData.ServerDatabase.ItemsDB?.Values
                .Where(x => !x.IsQuestItem && IsKeyItemByName(x)) ?? Enumerable.Empty<TarkovItem>();
        }

        private HashSet<string> GetKeyCategoryIds()
        {
            HashSet<string> keyCategoryIds = new();
            if (AppData.ServerDatabase.Handbook?.Categories == null)
                return keyCategoryIds;

            foreach (var category in AppData.ServerDatabase.Handbook.Categories)
            {
                if (CategoryNameContainsKeyWords(category.LocalizedName))
                    keyCategoryIds.Add(category.Id);
            }

            bool added;
            do
            {
                added = false;
                foreach (var category in AppData.ServerDatabase.Handbook.Categories)
                {
                    if (!string.IsNullOrEmpty(category.ParentId) && keyCategoryIds.Contains(category.ParentId) && keyCategoryIds.Add(category.Id))
                        added = true;
                }
            } while (added);

            return keyCategoryIds;
        }

        private static bool CategoryNameContainsKeyWords(string text)
            => !string.IsNullOrEmpty(text)
                && (text.Contains("key", StringComparison.OrdinalIgnoreCase)
                    || text.Contains("钥匙", StringComparison.OrdinalIgnoreCase)
                    || text.Contains("ключ", StringComparison.OrdinalIgnoreCase));

        private static bool IsKeyItemByName(TarkovItem item)
            => item != null && !string.IsNullOrEmpty(item.LocalizedName)
                && (item.LocalizedName.Contains("key", StringComparison.OrdinalIgnoreCase)
                    || item.LocalizedName.Contains("钥匙", StringComparison.OrdinalIgnoreCase)
                    || item.LocalizedName.Contains("ключ", StringComparison.OrdinalIgnoreCase));

        private bool IsKeyContainer()
        {
            if (!_item.IsInItemsDB || !_item.CanAddItems)
                return false;

            if (!AppData.ServerDatabase.ItemsDB.TryGetValue(_item.Tpl, out TarkovItem containerTpl))
                return false;

            if (CategoryNameContainsKeyWords(containerTpl.LocalizedName))
                return true;

            return GetAllKeyItems().Any(x => x.CanBeAddedToContainer(containerTpl));
        }
    }
}