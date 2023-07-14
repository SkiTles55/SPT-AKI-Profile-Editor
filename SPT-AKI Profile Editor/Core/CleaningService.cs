using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
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
            GetModdedInventoryItems(AppData.Profile?.Characters?.Pmc?.Inventory.Items, ModdedEntityType.PmcInventoryItem);
            GetModdedInventoryItems(AppData.Profile?.Characters?.Scav?.Inventory.Items, ModdedEntityType.ScavInventoryItem);
            ModdedEntities = compositeCollection;

            void GetModdedInventoryItems(InventoryItem[] inventoryItems, ModdedEntityType type)
            {
                if (inventoryItems != null)
                    foreach (var item in inventoryItems.Where(x => !x.IsInItemsDB))
                        compositeCollection.Add(new ModdedEntity(item.Id, type, true));
            }
        }
    }
}