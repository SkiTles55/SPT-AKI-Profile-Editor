using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class StashTabViewModel : BindableViewModel
    {
        public static AppSettings AppSettings => AppData.AppSettings;

        public static RelayCommand OpenContainer => new(obj => App.OpenContainerWindow(obj, StashEditMode.PMC));

        public static RelayCommand InspectWeapon => new(obj => App.OpenWeaponBuildWindow(obj, StashEditMode.PMC));

        public static RelayCommand AddItem => new(obj =>
        {
            if (obj == null || obj is not AddableItem item)
                return;
            App.Worker.AddAction(new WorkerTask { Action = () => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(item) });
        });

        public RelayCommand RemoveItem => new(async obj => await RemoveItemFromStash(obj?.ToString()));

        private async Task RemoveItemFromStash(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return;
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
                Profile.Characters.Pmc.Inventory.RemoveItems(new() { obj });
        }

        public RelayCommand RemoveAllItems => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_items_caption"))
            {
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => Profile.Characters.Pmc.Inventory.RemoveAllItems(),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                });
            }
        });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_equipment_items_caption"))
            {
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => Profile.Characters.Pmc.Inventory.RemoveAllEquipment(),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                });
            }
        });

        public RelayCommand AddMoney => new(async obj => await ShowAddMoneyDialog(obj));

        private async Task ShowAddMoneyDialog(object obj)
        {
            if (obj == null)
                return;
            string tpl = obj.ToString();
            CustomDialog addMoneyDialog = new() { Title = AppLocalization.GetLocalizedString("tab_stash_dialog_money") };
            RelayCommand addCommand = new(async obj => await AddMoneyDialogCommand(obj, tpl, addMoneyDialog));
            RelayCommand cancelCommand = new(async obj => await App.DialogCoordinator.HideMetroDialogAsync(this, addMoneyDialog));
            addMoneyDialog.Content = new MoneyDailog { DataContext = new MoneyDailogViewModel(tpl, addCommand, cancelCommand) };
            await App.DialogCoordinator.ShowMetroDialogAsync(this, addMoneyDialog);
        }

        private async Task AddMoneyDialogCommand(object obj, string tpl, CustomDialog addMoneyDialog)
        {
            await App.DialogCoordinator.HideMetroDialogAsync(this, addMoneyDialog);
            if (obj == null)
                return;
            Tuple<int, bool> result = (Tuple<int, bool>)obj;
            if (result == null || result.Item1 <= 0)
                return;
            App.Worker.AddAction(new WorkerTask
            {
                Action = () => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(tpl, result.Item1, result.Item2)
            });
        }
    }
}