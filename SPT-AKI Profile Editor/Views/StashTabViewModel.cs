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
    public class StashTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;

        public StashTabViewModel(IDialogManager dialogManager,
                                 IWorker worker,
                                 IApplicationManager applicationManager)
        {
            _dialogManager = dialogManager;
            _worker = worker;
            _applicationManager = applicationManager;
        }

        public static AppSettings AppSettings => AppData.AppSettings;

        public RelayCommand OpenContainer => new(obj =>
        {
            if (obj is InventoryItem item)
                _applicationManager.OpenContainerWindow(item, GetInventory(), true);
        });

        public RelayCommand InspectWeapon => new(obj =>
        {
            if (obj is InventoryItem item)
                _applicationManager.OpenWeaponBuildWindow(item, GetInventory());
        });

        public RelayCommand AddItem => new(obj =>
        {
            if (obj != null && obj is AddableItem item)
                _worker.AddTask(new(() => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(item), null, null));
        });

        public ScavStashTabViewModel ScavStashTabViewModel
            => new(_dialogManager, _worker, _applicationManager);

        public RelayCommand RemoveItem
            => new(async obj => await RemoveItemFromStash(obj?.ToString()));

        public RelayCommand RemoveAllItems => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog("remove_stash_item_title", "remove_stash_items_caption"))
                _worker.AddTask(RemoveTask(() => Profile.Characters.Pmc.Inventory.RemoveAllItems()));
        });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog("remove_stash_item_title", "remove_equipment_items_caption"))
                _worker.AddTask(RemoveTask(() => Profile.Characters.Pmc.Inventory.RemoveAllEquipment()));
        });

        public RelayCommand AddMoney
            => new(async obj => await ShowAddMoneyDialog(obj?.ToString()));

        public RelayCommand ShowAllItems
            => new(async obj => await _dialogManager.ShowAllItemsDialog(AddItem, true));

        private static CharacterInventory GetInventory()
            => Profile.Characters.GetInventory(StashEditMode.PMC);

        private static WorkerTask RemoveTask(Action action)
            => new(action,
                   AppLocalization.GetLocalizedString("progress_dialog_title"),
                   AppLocalization.GetLocalizedString("remove_stash_item_title"));

        private async Task RemoveItemFromStash(string obj)
        {
            if (!string.IsNullOrEmpty(obj) && await _dialogManager.YesNoDialog("remove_stash_item_title",
                                                                               "remove_stash_item_caption"))
                Profile.Characters.Pmc.Inventory.RemoveItems(new() { obj });
        }

        private async Task ShowAddMoneyDialog(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                var money = TarkovItem.CopyFrom(ServerDatabase.ItemsDB[obj]);
                await _dialogManager.ShowAddMoneyDialog(money, AddMoneyDialogCommand(money));
            }
        }

        private RelayCommand AddMoneyDialogCommand(AddableItem money)
            => new(obj => _worker.AddTask(new(() => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(money), null, null)));
    }
}