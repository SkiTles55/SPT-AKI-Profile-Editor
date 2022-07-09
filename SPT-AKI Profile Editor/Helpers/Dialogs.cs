using MahApps.Metro.Controls.Dialogs;
using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        private static MetroDialogSettings OkDialogSettings => new()
        {
            AffirmativeButtonText = AppData.AppLocalization.GetLocalizedString("save_profile_dialog_ok"),
            AnimateShow = true,
            AnimateHide = true
        };

        public static async Task<bool> YesNoDialog(object context, string title, string caption) =>
            await App.DialogCoordinator.ShowMessageAsync(context,
                AppData.AppLocalization.GetLocalizedString(title),
                AppData.AppLocalization.GetLocalizedString(caption),
                MessageDialogStyle.AffirmativeAndNegative,
                YesNoDialogSettings) == MessageDialogResult.Affirmative;

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
            CustomDialog settingsDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("tab_settings_title"), 600);
            RelayCommand closeCommand = new(async obj =>
            {
                await App.DialogCoordinator.HideMetroDialogAsync(context, settingsDialog);
                string newValues = AppSettings.GetStamp();
                if (startValues != newValues)
                    MainWindowViewModel.StartupEventsWorker();
            });
            await ShowCustomDialog<SettingsDialog>(context, settingsDialog, new SettingsDialogViewModel(closeCommand, index));
        }

        public static async Task ShowUpdateDialog(object context, GitHubRelease release)
        {
            CustomDialog updateDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("update_avialable"), 500);
            RelayCommand closeCommand = new(async obj =>
            {
                await App.DialogCoordinator.HideMetroDialogAsync(context, updateDialog);
            });
            await ShowCustomDialog<UpdateDialog>(context, updateDialog, new UpdateDialogViewModel(closeCommand, release));
        }

        public static async Task ShowIssuesDialog(object context, RelayCommand saveCommand)
        {
            CustomDialog issuesDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("profile_issues_title"), 500);
            await ShowCustomDialog<IssuesDialog>(context, issuesDialog, new IssuesDialogViewModel(saveCommand));
        }

        public static async Task ShowLocalizationEditorDialog(object context, AppLocalization appLocalization)
        {
            CustomDialog lEditorDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("profile_issues_title"), 500);
            await ShowCustomDialog<LocalizationEditor>(context, lEditorDialog, new LocalizationEditorViewModel(appLocalization));
        }

        public static async Task ShowOkMessageAsync(object context, string title, string message)
        {
            await App.DialogCoordinator.ShowMessageAsync(context, title,
                message, MessageDialogStyle.Affirmative, OkDialogSettings);
        }

        private static CustomDialog CustomDialog(string title, double width) => new()
        {
            Title = title,
            DialogContentWidth = new GridLength(width)
        };

        private static async Task ShowCustomDialog<T>(object context, CustomDialog dialog, BindableViewModel viewModel) where T : UserControl
        {
            T control = (T)Activator.CreateInstance(typeof(T));
            control.DataContext = viewModel;
            dialog.Content = control;
            await App.DialogCoordinator.ShowMetroDialogAsync(context, dialog);
        }
    }
}