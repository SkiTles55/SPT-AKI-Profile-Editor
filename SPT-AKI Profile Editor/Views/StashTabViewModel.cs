using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    public class StashTabViewModel(IDialogManager dialogManager,
        IWorker worker,
        IApplicationManager applicationManager) : BindableViewModel
    {
        public static AppSettings AppSettings => AppData.AppSettings;

        public RelayCommand OpenContainer => new(obj =>
        {
            if (obj is InventoryItem item)
                applicationManager.OpenContainerWindow(item, GetInventory(), true);
        });

        public RelayCommand InspectWeapon => new(obj =>
        {
            if (obj is InventoryItem item)
                applicationManager.OpenWeaponBuildWindow(item, GetInventory(), true);
        });

        public RelayCommand AddItem => new(obj =>
        {
            if (obj != null && obj is AddableItem item)
                worker.AddTask(ProgressTask(() => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(item)));
        });

        public ScavStashTabViewModel ScavStashTabViewModel
            => new(dialogManager, worker, applicationManager);

        public RelayCommand RemoveItem
            => new(async obj => await RemoveItemFromStash(obj?.ToString()));

        public RelayCommand RemoveAllItems => new(async obj =>
        {
            if (await dialogManager.YesNoDialog("remove_stash_item_title", "remove_stash_items_caption"))
                worker.AddTask(RemoveTask(() => Profile.Characters.Pmc.Inventory.RemoveAllItems()));
        });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await dialogManager.YesNoDialog("remove_stash_item_title", "remove_equipment_items_caption"))
                worker.AddTask(RemoveTask(() => Profile.Characters.Pmc.Inventory.RemoveAllEquipment()));
        });

        public RelayCommand AddMoney
            => new(async obj => await ShowAddMoneyDialog(obj?.ToString()));

        public RelayCommand ShowAllItems
            => new(async obj => await dialogManager.ShowAllItemsDialog(AddItem, true));

        private static CharacterInventory GetInventory()
            => Profile.Characters.GetInventory(StashEditMode.PMC);

        private static WorkerTask RemoveTask(Action action)
            => ProgressTask(action,
                            AppLocalization.GetLocalizedString("remove_stash_item_title"));

        private async Task RemoveItemFromStash(string obj)
        {
            if (!string.IsNullOrEmpty(obj) && await dialogManager.YesNoDialog("remove_stash_item_title",
                                                                              "remove_stash_item_caption"))
                Profile.Characters.Pmc.Inventory.RemoveItems([obj]);
        }

        private async Task ShowAddMoneyDialog(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                var money = TarkovItem.CopyFrom(ServerDatabase.ItemsDB[obj]);
                await dialogManager.ShowAddMoneyDialog(money, AddMoneyDialogCommand(money));
            }
        }

        private RelayCommand AddMoneyDialogCommand(AddableItem money)
            => new(obj => worker.AddTask(ProgressTask(() => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(money))));
    }
}