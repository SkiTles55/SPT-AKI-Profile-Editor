using MahApps.Metro.Controls.Dialogs;
using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using System.Threading.Tasks;
using System.Windows;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class Dialogs
    {
        private static MetroDialogSettings YesNoDialogSettings => new()
        {
            DefaultButtonFocus = MessageDialogResult.Affirmative,
            AffirmativeButtonText = AppData.AppLocalization.GetLocalizedString("button_yes"),
            NegativeButtonText = AppData.AppLocalization.GetLocalizedString("button_no"),
            AnimateShow = true,
            AnimateHide = true
        };

        private static MetroDialogSettings ShutdownDialogSettings => new()
        {
            AffirmativeButtonText = AppData.AppLocalization.GetLocalizedString("button_quit"),
            AnimateShow = true,
            AnimateHide = true
        };

        public static async Task<MessageDialogResult> YesNoDialog(object context, string title, string caption) =>
            await App.DialogCoordinator.ShowMessageAsync(context,
                AppData.AppLocalization.GetLocalizedString(title),
                AppData.AppLocalization.GetLocalizedString(caption),
                MessageDialogStyle.AffirmativeAndNegative,
                YesNoDialogSettings);

        public static async Task ShutdownCozServerRunned(object context)
        {
            if (await App.DialogCoordinator.ShowMessageAsync(context,
                AppData.AppLocalization.GetLocalizedString("app_quit"),
                AppData.AppLocalization.GetLocalizedString("server_runned"),
                MessageDialogStyle.Affirmative,
                ShutdownDialogSettings) == MessageDialogResult.Affirmative)
                App.CloseApplication.Execute(null);
        }

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
                await App.DialogCoordinator.HideMetroDialogAsync(context, settingsDialog);
                string newValues = AppSettings.GetStamp();
                if (startValues != newValues)
                    MainWindowViewModel.StartupEventsWorker();
            });
            settingsDialog.Content = new SettingsDialog { DataContext = new SettingsDialogViewModel(closeCommand, index) };
            await App.DialogCoordinator.ShowMetroDialogAsync(context, settingsDialog);
        }

        public static async Task ShowUpdateDialog(object context, GitHubRelease release)
        {
            CustomDialog updateDialog = new()
            {
                Title = AppData.AppLocalization.GetLocalizedString("update_avialable"),
                DialogContentWidth = new GridLength(500)
            };
            RelayCommand closeCommand = new(async obj =>
            {
                await App.DialogCoordinator.HideMetroDialogAsync(context, updateDialog);
            });
            updateDialog.Content = new UpdateDialog { DataContext = new UpdateDialogViewModel(closeCommand, release) };
            await App.DialogCoordinator.ShowMetroDialogAsync(context, updateDialog);
        }
    }
}