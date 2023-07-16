using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public interface ICleaningService
    {
        public void LoadEntitiesList();

        public void MarkAll(bool forRemoving, ModdedEntityType? type = null);

        public void RemoveSelected(RelayCommand saveCommand, IDialogManager dialogManager);
    }

    public class CleaningService : BindableEntity, ICleaningService
    {
        private readonly List<PropertyChangedEventHandler> changedHandlers = new();
        private ObservableCollection<ModdedEntity> moddedEntities;

        public ObservableCollection<ModdedEntity> ModdedEntities
        {
            get => moddedEntities;
            set
            {
                moddedEntities = value;
                OnPropertyChanged(nameof(ModdedEntities));
                OnPropertyChanged(nameof(CanDeselectAny));
                OnPropertyChanged(nameof(CanSelectAll));
            }
        }

        public bool CanDeselectAny => ModdedEntities?.Any(x => x.MarkedForRemoving) ?? false;

        public bool CanSelectAll => ModdedEntities?.Any(x => !x.MarkedForRemoving) ?? false;

        public void LoadEntitiesList()
        {
            var compositeCollection = new ObservableCollection<ModdedEntity>();
            GetModdedInventoryItems(AppData.Profile?.Characters?.Pmc?.Inventory.Items,
                                    ModdedEntityType.PmcInventoryItem,
                                    compositeCollection);
            GetModdedInventoryItems(AppData.Profile?.Characters?.Scav?.Inventory.Items,
                                    ModdedEntityType.ScavInventoryItem,
                                    compositeCollection);
            GetModdedQuests(AppData.Profile?.Characters?.Pmc?.Quests, compositeCollection);
            GetModdedExaminedItems(AppData.Profile?.Characters?.Pmc?.Encyclopedia?.Keys, compositeCollection);
            GetModdedMerchants(AppData.Profile?.Characters?.Pmc?.TraderStandings?.Keys, compositeCollection);

            ModdedEntities = compositeCollection;
        }

        public void MarkAll(bool forRemoving, ModdedEntityType? type = null)
        {
            foreach (var entity in ModdedEntities.Where(x => type == null || x.Type == type))
                entity.MarkedForRemoving = forRemoving;
        }

        public async void RemoveSelected(RelayCommand saveCommand, IDialogManager dialogManager)
        {
            var entitiesForRemove = ModdedEntities.Where(x => x.MarkedForRemoving);
            var needToSaveProfile = entitiesForRemove.Any(x => !x.Type.CanBeRemovedWithoutSave());
            var saveAllowed = needToSaveProfile && await dialogManager.YesNoDialog(AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_title"),
                                                                                   AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_save_dialog"));

            if (needToSaveProfile && !saveAllowed)
                return;

            foreach (var entity in entitiesForRemove.Where(x => x.MarkedForRemoving))
            {
                switch (entity.Type)
                {
                    case ModdedEntityType.PmcInventoryItem:
                        AppData.Profile.Characters.Pmc.Inventory.RemoveItems(new() { entity.Id });
                        break;

                    case ModdedEntityType.ScavInventoryItem:
                        AppData.Profile.Characters.Scav.Inventory.RemoveItems(new() { entity.Id });
                        break;

                    case ModdedEntityType.ExaminedItem:
                        AppData.Profile.Characters.Pmc.RemoveExaminedItem(entity.Id);
                        break;

                    case ModdedEntityType.Merchant:
                    case ModdedEntityType.Quest:
                        AppData.Profile.ModdedEntitiesForRemoving.Add(entity);
                        break;
                }
            }

            if (saveAllowed)
            {
                saveCommand.Execute(null);
                return;
            }

            LoadEntitiesList();
        }

        private void ChildChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CanDeselectAny));
            OnPropertyChanged(nameof(CanSelectAll));
        }

        private void GetModdedInventoryItems(InventoryItem[] inventoryItems,
                                                                             ModdedEntityType type,
                                             ObservableCollection<ModdedEntity> compositeCollection)
        {
            if (inventoryItems != null)
                AddModdedEntity(inventoryItems,
                                x => !x.IsInItemsDB,
                                x => x.Id,
                                type,
                                compositeCollection);
        }

        private void GetModdedQuests(CharacterQuest[] quests, ObservableCollection<ModdedEntity> compositeCollection)
        {
            if (quests != null && AppData.ServerDatabase?.LocalesGlobal != null)
                AddModdedEntity(quests,
                                x => !AppData.ServerDatabase.LocalesGlobal.ContainsKey(x.Qid.QuestName()),
                                x => x.Qid,
                                ModdedEntityType.Quest,
                                compositeCollection);
        }

        private void GetModdedExaminedItems(IEnumerable<string> examinedIds, ObservableCollection<ModdedEntity> compositeCollection)
        {
            if (examinedIds != null && AppData.ServerDatabase?.ItemsDB != null)
                AddModdedEntity(examinedIds,
                                x => x != AppData.AppSettings.EndlessDevBackpackId && !AppData.ServerDatabase.ItemsDB.ContainsKey(x),
                                x => x,
                                ModdedEntityType.ExaminedItem,
                                compositeCollection);
        }

        private void GetModdedMerchants(IEnumerable<string> merchantIds, ObservableCollection<ModdedEntity> compositeCollection)
        {
            if (merchantIds != null && AppData.ServerDatabase?.LocalesGlobal != null)
                AddModdedEntity(merchantIds,
                                x => x != AppData.AppSettings.RagfairTraderId && !AppData.ServerDatabase.LocalesGlobal.ContainsKey(x.Nickname()),
                                x => x,
                                ModdedEntityType.Merchant,
                                compositeCollection);
        }

        private void AddModdedEntity<T>(IEnumerable<T> values,
                                        Func<T, bool> predicate,
                                        Func<T, string> idSelector,
                                        ModdedEntityType type,
                                        ObservableCollection<ModdedEntity> compositeCollection)
        {
            foreach (var value in values.Where(predicate))
            {
                var id = idSelector.Invoke(value);
                var existedEntity = ModdedEntities?.FirstOrDefault(x => x.Id == id);
                ModdedEntity newEntity = new(id,
                                              type,
                                              existedEntity?.MarkedForRemoving ?? false);
                compositeCollection.Add(newEntity);
                PropertyChangedEventHandler eventHandler = new(ChildChanged);
                newEntity.PropertyChanged += eventHandler;
                changedHandlers.Add(eventHandler);
            }
        }
    }
}