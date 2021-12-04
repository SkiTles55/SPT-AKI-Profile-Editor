using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using System.Threading.Tasks;
using System.Windows;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class Dialogs
    {
        public static async Task<MessageDialogResult> YesNoDialog(object context, string title, string caption) =>
            await App.dialogCoordinator.ShowMessageAsync(context,
                AppData.AppLocalization.GetLocalizedString(title),
                AppData.AppLocalization.GetLocalizedString(caption),
                MessageDialogStyle.AffirmativeAndNegative,
                DialogSettings);

        private static MetroDialogSettings DialogSettings => new()
        {
            DefaultButtonFocus = MessageDialogResult.Affirmative,
            AffirmativeButtonText = AppData.AppLocalization.GetLocalizedString("button_yes"),
            NegativeButtonText = AppData.AppLocalization.GetLocalizedString("button_no"),
            AnimateShow = true,
            AnimateHide = true
        };

        public static async Task ShowSettingsDialog(object context, int index = 0)
        {
            string startValues = AppSettings.GetStamp();
            CustomDialog settingsDialog = new()
            {
                Title = AppData.AppLocalization.GetLocalizedString("tab_settings_title"),
                DialogContentWidth = new GridLength(600)
            };
            RelayCommand closeCommand = new(async obj =>
            {
                await App.dialogCoordinator.HideMetroDialogAsync(context, settingsDialog);
                string newValues = AppSettings.GetStamp();
                if (startValues != newValues)
                    App.StartupEventsWorker();
            });
            settingsDialog.Content = new SettingsDialog { DataContext = new SettingsDialogViewModel(closeCommand, index) };
            await App.dialogCoordinator.ShowMetroDialogAsync(context, settingsDialog);
        }
    }
}