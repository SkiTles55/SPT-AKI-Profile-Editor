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
        private static readonly HashSet<string> KeyParentIds = ["5c99f98d86f7745c314214b3", "5c164d2286f774194c5e69fa"];
        private static readonly HashSet<string> BadKeyItems = ["5a043f2c86f7741aa57b5145", "5751916f24597720a27126df", "57518f7724597720a31c09ab", "57518fd424597720c85dbaaa", "590de4a286f77423d9312a32"];
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

        public RelayCommand AddAllKeys => new(async obj =>
        {
            if (!AddAllKeysAllowed)
                return;

            var keys = GetKeysToAdd(out int fitCount);
            if (keys.Count == 0)
                return;

            bool addNewHolder = false;
            if (fitCount < keys.Count)
                addNewHolder = await _dialogManager.YesNoDialog(
                    "container_window_add_all_keys",
                    AppLocalization.GetLocalizedString("container_window_add_all_keys_no_space",
                        (keys.Count - fitCount).ToString()));

            _worker.AddTask(ProgressTask(() => AddAllKeysToContainer(keys.Take(fitCount).ToList(),
                                                                     addNewHolder ? keys.Skip(fitCount).ToList() : new List<TarkovItem>()),
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

        private void AddAllKeysToContainer(List<TarkovItem> keys, List<TarkovItem> keysForNewHolder)
        {
            AddKeysToContainer(_item, keys);

            if (keysForNewHolder.Count > 0)
                AddKeysToNewHolder(keysForNewHolder);

            OnPropertyChanged("");
        }

        private List<TarkovItem> GetKeysToAdd(out int fitCount)
        {
            fitCount = 0;
            if (!_item.IsInItemsDB || !_item.CanAddItems)
                return [];

            if (!AppData.ServerDatabase.ItemsDB.TryGetValue(_item.Tpl, out TarkovItem containerTpl))
                return [];

            var existingItemTpls = Items.Select(x => x.Tpl).ToHashSet();
            var keys = GetAllKeyItems()
                .Where(x => !existingItemTpls.Contains(x.Id))
                .Where(x => x.CanBeAddedToContainer(containerTpl))
                .OrderBy(x => (x.Properties?.Width ?? 0) * (x.Properties?.Height ?? 0))
                .ToList();

            fitCount = _inventory.GetMaxItemsToFit(_item, keys);
            return keys;
        }

        private int AddKeysToContainer(InventoryItem container, List<TarkovItem> keys)
        {
            int added = 0;
            foreach (var key in keys)
            {
                try
                {
                    _inventory.AddNewItemsToContainer(container, key, "main");
                    added++;
                }
                catch (Exception ex)
                {
                    if (ex.Message == AppLocalization.GetLocalizedString("tab_stash_no_slots"))
                        return added;

                    throw;
                }
            }
            return added;
        }

        private void AddKeysToNewHolder(List<TarkovItem> keys)
        {
            if (!AppData.ServerDatabase.ItemsDB.TryGetValue(DefaultValues.KeycardHolderTpl, out TarkovItem holderTemplate))
                return;

            var remainingKeys = new List<TarkovItem>(keys);
            while (remainingKeys.Count > 0)
            {
                var holder = AddNewKeycardHolder(holderTemplate);
                if (holder == null)
                    return;

                int added = AddKeysToContainer(holder, remainingKeys);
                if (added == 0)
                    return;

                remainingKeys.RemoveRange(0, added);
            }
        }

        private InventoryItem AddNewKeycardHolder(TarkovItem holderTemplate)
        {
            try
            {
                _inventory.AddNewItemsToStash(holderTemplate);
            }
            catch (Exception ex)
            {
                if (ex.Message == AppLocalization.GetLocalizedString("tab_stash_no_slots"))
                    return null;

                throw;
            }

            return _inventory.Items?.LastOrDefault(x => x.Tpl == holderTemplate.Id
                                                        && x.ParentId == _inventory.Stash
                                                        && x.SlotId == "hideout");
        }

        private IEnumerable<TarkovItem> GetAllKeyItems()
            => AppData.ServerDatabase.ItemsDB?.Values
                .Where(x => x != null && !BadKeyItems.Contains(x.Id) && KeyParentIds.Contains(x.Parent))
                ?? Enumerable.Empty<TarkovItem>();

        private static bool CategoryNameContainsKeyWords(string text)
            => !string.IsNullOrEmpty(text)
                && (text.Contains("key", StringComparison.OrdinalIgnoreCase)
                    || text.Contains("钥匙", StringComparison.OrdinalIgnoreCase)
                    || text.Contains("ключ", StringComparison.OrdinalIgnoreCase));

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