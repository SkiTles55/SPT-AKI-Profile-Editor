﻿using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.Equipment;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterInventory : BindableEntity
    {
        private InventoryItem[] items;
        private string stash;
        private string equipment;

        [JsonProperty("items")]
        public InventoryItem[] Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged("");
            }
        }

        [JsonProperty("hideoutAreaStashes")]
        public Dictionary<string, string> HideoutAreaStashes { get; set; }

        [JsonProperty("stash")]
        public string Stash
        {
            get => stash;
            set
            {
                stash = value;
                OnPropertyChanged(nameof(Stash));
            }
        }

        [JsonProperty("questRaidItems")]
        public string QuestRaidItems { get; set; }

        [JsonProperty("questStashItems")]
        public string QuestStashItems { get; set; }

        [JsonProperty("equipment")]
        public string Equipment
        {
            get => equipment;
            set
            {
                equipment = value;
                OnPropertyChanged(nameof(Equipment));
            }
        }

        [JsonIgnore]
        public string Pockets
        {
            get => Items?
                .Where(x => x.IsPockets)
                .FirstOrDefault()?.Tpl;
            set
            {
                var pocketsSlot = Items?
                .Where(x => x.IsPockets)
                .FirstOrDefault();
                if (pocketsSlot != null)
                {
                    pocketsSlot.Tpl = value;
                    OnPropertyChanged(nameof(Pockets));
                }
            }
        }

        [JsonIgnore]
        public string DollarsCount => GetMoneyCountString(AppData.AppSettings.MoneysDollarsTpl);

        [JsonIgnore]
        public string RublesCount => GetMoneyCountString(AppData.AppSettings.MoneysRublesTpl);

        [JsonIgnore]
        public string EurosCount => GetMoneyCountString(AppData.AppSettings.MoneysEurosTpl);

        [JsonIgnore]
        public IEnumerable<InventoryItem> InventoryItems => Items?
            .Where(x => x.ParentId == Stash && x.Location != null);

        [JsonIgnore]
        public bool HasItems => InventoryItems?.Count() > 0;

        [JsonIgnore]
        public bool ContainsModdedItems => InventoryItems.Any(x => !x.IsInItemsDB);

        [JsonIgnore]
        public bool InventoryHaveDuplicatedItems => GroupedInventory.Any();

        [JsonIgnore]
        public List<EquipmentSlot> EquipmentSlots
            => EquipmentAdapter.GetCharacterEquipment(this,
                                                      Equipment,
                                                      AppData.ServerDatabase?.LocalesGlobal,
                                                      AppData.AppLocalization,
                                                      AppData.AppSettings);

        [JsonIgnore]
        public IEnumerable<InventoryItem> PocketsItems => Items?
            .Where(x => x.ParentId == Items?.Where(x => x.IsPockets).FirstOrDefault()?.Id);

        [JsonIgnore]
        public bool PocketsHasItems => PocketsItems?.Count() > 0;

        [JsonIgnore]
        public bool HasEquipment
            => EquipmentSlots?.SelectMany(x => x.ItemsList)?.Any(x => x != null) == true;

        private IEnumerable<string> GroupedInventory => Items?
            .GroupBy(x => x.Id)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);

        public void RemoveDuplicatedItems() => FinalRemoveItems(GroupedInventory);

        public void RemoveItems(List<string> itemIds) => FinalRemoveItems(itemIds);

        public void RemoveAllItems() => FinalRemoveItems(InventoryItems.Select(x => x.Id));

        public List<InventoryItem> GetInnerItems(string itemId, List<string> skippedSlots = null)
        {
            List<InventoryItem> items = new();
            foreach (var item in Items?.Where(x => x.ParentId == itemId))
            {
                if (skippedSlots != null && skippedSlots.Count > 0 && skippedSlots.Contains(item.SlotId))
                    continue;
                items.Add(item);
                items.AddRange(GetInnerItems(item.Id, skippedSlots));
            }
            return items;
        }

        public void AddNewItemsToContainer(InventoryItem container, AddableItem item, string slotId)
        {
            switch (item)
            {
                case TarkovItem:
                    AddNewItemsToContainer(container, (TarkovItem)item, slotId);
                    break;

                case WeaponBuild:
                    AddNewWeaponToContainer(container, (WeaponBuild)item, slotId);
                    break;
            }
        }

        public void AddNewItemsToStash(AddableItem item)
        {
            var stashId = item.IsQuestItem ? GetStashId(item.StashType) : Stash;
            InventoryItem ProfileStash = Items.Where(x => x.Id == stashId).FirstOrDefault();
            AddNewItemsToContainer(ProfileStash, item, "hideout");
        }

        public void RemoveAllEquipment()
            => FinalRemoveItems(EquipmentSlots?
                .SelectMany(x => x.ItemsList)?
                .Where(x => x != null)?
                .Select(x => x.Id));

        public int[,] GetSlotsMap(InventoryItem container)
        {
            int[,] Stash2D = CreateContainerStash2D(container);
            foreach (var item in Items?.Where(x => x.ParentId == container.Id))
            {
                (int itemWidth, int itemHeight) = GetSizeOfInventoryItem(item.Id, item.Tpl, Items);
                int rotatedHeight = item.Location.R == ItemRotation.Vertical ? itemWidth : itemHeight;
                int rotatedWidth = item.Location.R == ItemRotation.Vertical ? itemHeight : itemWidth;
                for (int y = 0; y < rotatedHeight; y++)
                {
                    try
                    {
                        for (int z = item.Location.X; z < item.Location.X + rotatedWidth; z++)
                            Stash2D[item.Location.Y + y, z] = 1;
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Failed to insert item with id {item.Id} to Stash2D: {ex.Message}");
                    }
                }
            }
            return Stash2D;
        }

        private static void AddItemToList(List<InventoryItem> items, string id, string parentId, string slotId, string tpl, ItemLocation location = null, ItemUpd itemUpd = null)
        {
            var newItem = new InventoryItem
            {
                Id = id,
                ParentId = parentId,
                SlotId = slotId,
                Tpl = tpl,
                Location = location,
                Upd = itemUpd
            };
            items.Add(newItem);
        }

        private static List<ItemLocation> GetItemLocations(int itemWidth, int itemHeight, int[,] stash, int stacks)
        {
            List<ItemLocation> freeSlots = GetFreeSlots(stash);
            if (freeSlots.Count < itemWidth * itemHeight * stacks)
                return null;
            List<ItemLocation> NewItemsLocations = new();
            foreach (var slot in freeSlots)
            {
                if (itemWidth == 1 && itemHeight == 1)
                    NewItemsLocations.Add(slot);
                else
                {
                    ItemLocation itemLocation = GetItemLocation(itemHeight, itemWidth, stash, slot);
                    if (itemLocation != null)
                        NewItemsLocations.Add(itemLocation);
                    if (NewItemsLocations.Count == stacks)
                        return NewItemsLocations;
                    itemLocation = GetItemLocation(itemWidth, itemHeight, stash, slot);
                    if (itemLocation != null)
                    {
                        itemLocation.R = ItemRotation.Vertical;
                        NewItemsLocations.Add(itemLocation);
                    }
                }
                if (NewItemsLocations.Count == stacks)
                    return NewItemsLocations;
            }
            return null;
        }

        private static List<ItemLocation> GetFreeSlots(int[,] Stash)
        {
            List<ItemLocation> locations = new();
            for (int y = 0; y < Stash.GetLength(0); y++)
                for (int x = 0; x < Stash.GetLength(1); x++)
                    if (Stash[y, x] == 0)
                        locations.Add(new ItemLocation { X = x, Y = y, R = ItemRotation.Horizontal });
            return locations;
        }

        private static ItemLocation GetItemLocation(int Width, int Height, int[,] Stash, ItemLocation slot)
        {
            int size = 0;
            for (int y = 0; y < Width; y++)
            {
                if (slot.X + Height < Stash.GetLength(1) && slot.Y + y < Stash.GetLength(0))
                    for (int z = slot.X; z < slot.X + Height; z++)
                        if (Stash[slot.Y + y, z] == 0) size++;
            }
            if (size == Width * Height)
            {
                for (int y = 0; y < Width; y++)
                    for (int z = slot.X; z < slot.X + Height; z++)
                        Stash[slot.Y + y, z] = 1;
                return new ItemLocation { X = slot.X, Y = slot.Y };
            }
            return null;
        }

        private static (int itemWidth, int itemHeight) GetSizeOfInventoryItem(string itemId, string itemTpl, IEnumerable<InventoryItem> itemsArray)
        {
            List<string> toDo = new() { itemId };
            TarkovItem tmpItem = AppData.ServerDatabase.ItemsDB[itemTpl];
            InventoryItem rootItem = itemsArray.Where(x => x.ParentId == itemId).FirstOrDefault();
            bool FoldableWeapon = tmpItem.Properties.Foldable;
            string FoldedSlot = tmpItem.Properties.FoldedSlot;

            int SizeUp = 0;
            int SizeDown = 0;
            int SizeLeft = 0;
            int SizeRight = 0;

            int ForcedUp = 0;
            int ForcedDown = 0;
            int ForcedLeft = 0;
            int ForcedRight = 0;
            int outX = tmpItem.Properties.Width;
            int outY = tmpItem.Properties.Height;
            if (rootItem != null && !rootItem.IsContainer)
            {
                List<string> skipThisItems = new() { "5448e53e4bdc2d60728b4567", "566168634bdc2d144c8b456c", "5795f317245977243854e041" };
                bool rootFolded = rootItem.Upd != null && rootItem.Upd.Foldable != null && rootItem.Upd.Foldable.Folded;

                if (FoldableWeapon && string.IsNullOrEmpty(FoldedSlot) && rootFolded)
                    outX -= tmpItem.Properties.SizeReduceRight;

                if (!skipThisItems.Contains(tmpItem.Parent))
                {
                    while (toDo.Count > 0)
                    {
                        if (toDo.ElementAt(0) != null)
                        {
                            foreach (var item in itemsArray.Where(x => x.ParentId == toDo.ElementAt(0)))
                            {
                                if (!item.SlotId.Contains("mod_"))
                                    continue;
                                toDo.Add(item.Id);
                                if (!AppData.ServerDatabase.ItemsDB.ContainsKey(item.Tpl))
                                    throw new Exception(AppData.AppLocalization.GetLocalizedString("tab_stash_modded_item_founded_error"));
                                TarkovItem itm = AppData.ServerDatabase.ItemsDB[item.Tpl];
                                bool childFoldable = itm.Properties.Foldable;
                                bool childFolded = item.Upd != null && item.Upd.Foldable != null && item.Upd.Foldable.Folded;
                                if (FoldableWeapon && FoldedSlot == item.SlotId && (rootFolded || childFolded))
                                    continue;
                                else if (childFoldable && rootFolded && childFolded)
                                    continue;
                                if (itm.Properties.ExtraSizeForceAdd)
                                {
                                    ForcedUp += itm.Properties.ExtraSizeUp;
                                    ForcedDown += itm.Properties.ExtraSizeDown;
                                    ForcedLeft += itm.Properties.ExtraSizeLeft;
                                    ForcedRight += itm.Properties.ExtraSizeRight;
                                }
                                else
                                {
                                    SizeUp = (SizeUp < itm.Properties.ExtraSizeUp) ? itm.Properties.ExtraSizeUp : SizeUp;
                                    SizeDown = (SizeDown < itm.Properties.ExtraSizeDown) ? itm.Properties.ExtraSizeDown : SizeDown;
                                    SizeLeft = (SizeLeft < itm.Properties.ExtraSizeLeft) ? itm.Properties.ExtraSizeLeft : SizeLeft;
                                    SizeRight = (SizeRight < itm.Properties.ExtraSizeRight) ? itm.Properties.ExtraSizeRight : SizeRight;
                                }
                            }
                        }
                        toDo.Remove(toDo.ElementAt(0));
                    }
                }
            }

            return (outX + SizeLeft + SizeRight + ForcedLeft + ForcedRight, outY + SizeUp + SizeDown + ForcedUp + ForcedDown);
        }

        private int[,] CreateContainerStash2D(InventoryItem container)
        {
            Grid stashGrid = AppData.ServerDatabase.ItemsDB[container.Tpl].Properties.Grids.FirstOrDefault();
            var width = stashGrid?.Props?.CellsV ?? 0;
            var height = stashGrid?.Props?.CellsH ?? 0;
            if (container.Id == Stash)
                width += AppData.Profile.Characters.Pmc.StashRowsBonusCount;
            return new int[width, height];
        }

        private string GetStashId(StashType stashType)
        {
            return stashType switch
            {
                StashType.QuestRaidItems => QuestRaidItems,
                StashType.QuestStashItems => QuestStashItems,
                _ => Stash,
            };
        }

        private void AddNewItemsToContainer(InventoryItem container, TarkovItem tarkovItem, string slotId)
        {
            AddItemToContainer(container,
                               tarkovItem.Properties.Width,
                               tarkovItem.Properties.Height,
                               tarkovItem.Id,
                               tarkovItem.AddingQuantity,
                               tarkovItem.AddingFir,
                               slotId,
                               tarkovItem.Properties.StackMaxSize,
                               null,
                               null,
                               null,
                               tarkovItem.DogtagProperties);
        }

        private void AddNewWeaponToContainer(InventoryItem container, WeaponBuild weaponBuild, string slotId)
        {
            var (itemWidth, itemHeight) = GetSizeOfInventoryItem(weaponBuild.Root, weaponBuild.RootTpl, weaponBuild.BuildItems);
            AddItemToContainer(container,
                               itemWidth,
                               itemHeight,
                               weaponBuild.RootTpl,
                               weaponBuild.AddingQuantity,
                               weaponBuild.AddingFir,
                               slotId,
                               1,
                               weaponBuild.Root,
                               weaponBuild.BuildItems);
        }

        private void AddItemToContainer(InventoryItem container,
                                        int itemWidth,
                                        int itemHeight,
                                        string itemTpl,
                                        int count,
                                        bool fir,
                                        string slotId,
                                        int stackSize,
                                        string rootId = null,
                                        IEnumerable<InventoryItem> innerItems = null,
                                        string tag = null,
                                        DogtagProperties dogtagProperties = null)
        {
            int stacks = count / stackSize;
            if (stackSize * stacks < count) stacks++;
            int[,] Stash = GetSlotsMap(container);
            List<ItemLocation> NewItemsLocations = GetItemLocations(itemWidth, itemHeight, Stash, stacks)
                ?? throw new Exception(AppData.AppLocalization.GetLocalizedString("tab_stash_no_slots"));
            List<string> iDs = Items.Select(x => x.Id).ToList();
            List<InventoryItem> items = Items.ToList();
            for (int i = 0; i < NewItemsLocations.Count; i++)
            {
                if (count <= 0) break;
                string rootNewId = ExtMethods.GenerateNewId(iDs);
                iDs.Add(rootNewId);
                var location = new ItemLocation { R = NewItemsLocations[i].R, X = NewItemsLocations[i].X, Y = NewItemsLocations[i].Y, IsSearched = true };
                var upd = new ItemUpd { StackObjectsCount = count > stackSize ? stackSize : count, SpawnedInSession = fir };
                if (!string.IsNullOrEmpty(tag))
                    upd.Tag = new Tag { Name = tag };
                if (dogtagProperties != null)
                {
                    dogtagProperties.UpdateProperties();
                    upd.Dogtag = dogtagProperties;
                }
                AddItemToList(items, rootNewId, container.Id, slotId, itemTpl, location, upd);
                AddInnerItems(rootId, rootNewId, fir);
                count -= stackSize;
            }
            Items = items.ToArray();

            void AddInnerItems(string rootId, string newRootId, bool fir)
            {
                if (string.IsNullOrEmpty(rootId) || innerItems == null)
                    return;
                foreach (var item in innerItems.Where(x => x.ParentId == rootId))
                {
                    string newId = ExtMethods.GenerateNewId(iDs);
                    iDs.Add(newId);
                    AddItemToList(items, newId, newRootId, item.SlotId, item.Tpl, null, fir ? new ItemUpd { SpawnedInSession = fir } : null);
                    AddInnerItems(item.Id, newId, fir);
                }
            }
        }

        private List<string> GetCompleteItemsList(IEnumerable<string> items)
        {
            List<string> itemIds = new();
            foreach (var TargetItem in items)
            {
                if (string.IsNullOrEmpty(TargetItem))
                    continue;
                List<string> toDo = new() { TargetItem };
                while (toDo.Count > 0)
                {
                    foreach (var item in Items.Where(x => x.ParentId == toDo.ElementAt(0)))
                        toDo.Add(item.Id);
                    itemIds.Add(toDo.ElementAt(0));
                    toDo.Remove(toDo.ElementAt(0));
                }
            }
            return itemIds;
        }

        private void FinalRemoveItems(IEnumerable<string> itemIds)
        {
            var completedList = GetCompleteItemsList(itemIds);
            App.ApplicationManager.CloseItemViewWindows(completedList);
            List<InventoryItem> ItemsList = Items.ToList();
            while (completedList.Count > 0)
            {
                var item = ItemsList.Where(x => x.Id == completedList[0]).FirstOrDefault();
                if (item != null)
                    ItemsList.Remove(item);
                else
                    completedList.RemoveAt(0);
            }
            Items = ItemsList.ToArray();
        }

        private string GetMoneyCountString(string moneys) => (Items?
            .Where(x => x.Tpl == moneys)
            .Sum(x => x.Upd.StackObjectsCount ?? 0) ?? 0)
            .ToString("N0");
    }
}