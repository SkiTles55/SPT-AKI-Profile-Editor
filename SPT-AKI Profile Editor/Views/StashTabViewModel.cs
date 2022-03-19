using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class StashTabViewModel : BindableViewModel
    {
        public static AppSettings AppSettings => AppData.AppSettings;

        public RelayCommand RemoveItem => new(async obj =>
         {
             if (obj == null)
                 return;
             if (await Dialogs.YesNoDialog(this,
                 "remove_stash_item_title",
                 "remove_stash_item_caption") == MessageDialogResult.Affirmative)
                 Profile.Characters.Pmc.Inventory.RemoveItems(new() { obj.ToString() });
         });

        public RelayCommand RemoveAllItems => new(async obj =>
         {
             if (await Dialogs.YesNoDialog(this,
                 "remove_stash_item_title",
                 "remove_stash_items_caption") == MessageDialogResult.Affirmative)
             {
                 App.Worker.AddAction(new WorkerTask
                 {
                     Action = () => { Profile.Characters.Pmc.Inventory.RemoveAllItems(); },
                     Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                     Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                 });
             }
         });

        public RelayCommand AddMoney => new(async obj => { await ShowAddMoneyDialog(obj); });

        private async Task ShowAddMoneyDialog(object obj)
        {
            if (obj == null)
                return;
            string tpl = obj.ToString();
            CustomDialog addMoneyDialog = new()
            {
                Title = AppLocalization.GetLocalizedString("tab_stash_dialog_money")
            };
            RelayCommand addCommand = new(async obj =>
            {
                await App.DialogCoordinator.HideMetroDialogAsync(this, addMoneyDialog);
                if (obj == null)
                    return;
                Tuple<int, bool> result = (Tuple<int, bool>)obj;
                if (result == null || result.Item1 <= 0)
                    return;
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => { Profile.Characters.Pmc.Inventory.AddNewItems(tpl, result.Item1, result.Item2); }
                });
            });
            RelayCommand cancelCommand = new(async obj =>
            {
                await App.DialogCoordinator.HideMetroDialogAsync(this, addMoneyDialog);
            });
            addMoneyDialog.Content = new MoneyDailog { DataContext = new MoneyDailogViewModel(tpl, addCommand, cancelCommand) };
            await App.DialogCoordinator.ShowMetroDialogAsync(this, addMoneyDialog);
        }
    }
}