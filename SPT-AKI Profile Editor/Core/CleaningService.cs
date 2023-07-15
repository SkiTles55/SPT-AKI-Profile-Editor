using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public class CleaningService : BindableEntity
    {
        private ObservableCollection<ModdedEntity> moddedEntities;

        public ObservableCollection<ModdedEntity> ModdedEntities
        {
            get => moddedEntities;
            set
            {
                moddedEntities = value;
                OnPropertyChanged(nameof(ModdedEntities));
            }
        }

        public void LoadEntitesList()
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
                compositeCollection.Add(new ModdedEntity(id,
                                                         type,
                                                         type.CanBeRemovedWithoutSave(),
                                                         existedEntity?.MarkedForRemoving ?? false));
            }
        }
    }
}