using MahApps.Metro.Controls.Dialogs;
using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IDialogManager
    {
        public Task<bool> YesNoDialog(string title, string caption);

        public Task ShutdownCozServerRunned();

        public Task ShowSettingsDialog(int index = 0);

        public Task ShowUpdateDialog(GitHubRelease release);

        public Task ShowIssuesDialog(RelayCommand saveCommand, IIssuesService issuesService);

        public Task ShowLocalizationEditorDialog(SettingsDialogViewModel settingsDialog, bool isEdit = true);

        public Task ShowServerPathEditorDialog(IEnumerable<ServerPathEntry> paths, RelayCommand retryCommand);

        public Task ShowOkMessageAsync(string title, string message);

        public Task ShowAddMoneyDialog(AddableItem money, RelayCommand addCommand);
    }

    public class MetroDialogManager : IDialogManager
    {
        private readonly object viewModel;

        public MetroDialogManager(object viewModel) => this.viewModel = viewModel;

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

        public async Task<bool> YesNoDialog(string title, string caption) =>
            await App.DialogCoordinator.ShowMessageAsync(viewModel,
                                                         AppData.AppLocalization.GetLocalizedString(title),
                                                         AppData.AppLocalization.GetLocalizedString(caption),
                                                         MessageDialogStyle.AffirmativeAndNegative,
                                                         YesNoDialogSettings) == MessageDialogResult.Affirmative;

        public async Task ShutdownCozServerRunned()
        {
            if (await App.DialogCoordinator.ShowMessageAsync(viewModel,
                                                             AppData.AppLocalization.GetLocalizedString("app_quit"),
                                                             AppData.AppLocalization.GetLocalizedString("server_runned"),
                                                             MessageDialogStyle.Affirmative,
                                                             ShutdownDialogSettings) == MessageDialogResult.Affirmative)
                App.ApplicationManager.CloseApplication.Execute(null);
        }

        public async Task ShowSettingsDialog(int index = 0)
        {
            string startValues = AppSettings.GetStamp();
            CustomDialog settingsDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("tab_settings_title"), 600);
            RelayCommand closeCommand = new(async obj =>
            {
                await App.DialogCoordinator.HideMetroDialogAsync(viewModel, settingsDialog);
                string newValues = AppSettings.GetStamp();
                if (startValues != newValues)
                    MainWindowViewModel.Instance.StartupEventsWorker();
            });
            await ShowCustomDialog<SettingsDialog>(viewModel, settingsDialog, new SettingsDialogViewModel(closeCommand, this, App.WindowsDialogs, App.ApplicationManager, index));
        }

        public async Task ShowUpdateDialog(GitHubRelease release)
        {
            CustomDialog updateDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("update_avialable"), 500);
            await ShowCustomDialog<UpdateDialog>(viewModel,
                                                 updateDialog,
                                                 new UpdateDialogViewModel(App.ApplicationManager, App.WindowsDialogs, release, viewModel, this));
        }

        public async Task ShowIssuesDialog(RelayCommand saveCommand, IIssuesService issuesService)
        {
            CustomDialog issuesDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("profile_issues_title"), 500);
            await ShowCustomDialog<IssuesDialog>(viewModel,
                                                 issuesDialog,
                                                 new IssuesDialogViewModel(saveCommand, issuesService, viewModel));
        }

        public async Task ShowLocalizationEditorDialog(SettingsDialogViewModel settingsDialog, bool isEdit = true)
        {
            CustomDialog lEditorDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("localization_editor_title"), 500);
            await ShowCustomDialog<LocalizationEditor>(viewModel,
                                                       lEditorDialog,
                                                       new LocalizationEditorViewModel(isEdit, settingsDialog, viewModel));
        }

        public async Task ShowServerPathEditorDialog(IEnumerable<ServerPathEntry> paths, RelayCommand retryCommand)
        {
            CustomDialog pathEditorDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"), 500);
            await ShowCustomDialog<ServerPathEditor>(viewModel,
                                                     pathEditorDialog,
                                                     new ServerPathEditorViewModel(paths, retryCommand, MainWindowViewModel.Instance.OpenFAQ, viewModel));
        }

        public async Task ShowAddMoneyDialog(AddableItem money, RelayCommand addCommand)
        {
            CustomDialog addMoneyDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("tab_stash_dialog_money"), 500);
            await ShowCustomDialog<MoneyDailog>(viewModel, addMoneyDialog, new MoneyDailogViewModel(money, addCommand, viewModel));
        }

        public async Task ShowOkMessageAsync(string title, string message)
        {
            await App.DialogCoordinator.ShowMessageAsync(viewModel, title,
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