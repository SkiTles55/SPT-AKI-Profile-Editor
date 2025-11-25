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
        public event EventHandler ProgressDialogCanceled;

        public Task<bool> YesNoDialog(string title, string caption);

        public Task<YesNoDontAskAgainDialogResult> YesNoDontAskAgainDialog(string title,
                                                                           string yesText,
                                                                           string noText,
                                                                           string message,
                                                                           bool dontAskAgain);

        public Task ShutdownCozServerRunned();

        public Task ShowSettingsDialog(RelayCommand reloadCommand,
                                       RelayCommand faqCommand,
                                       IWorker worker,
                                       IHelperModManager modManager,
                                       int index = 0);

        public Task ShowUpdateDialog(GitHubRelease release);

        public Task ShowIssuesDialog(RelayCommand saveCommand, IIssuesService issuesService);

        public Task ShowLocalizationEditorDialog(SettingsDialogViewModel settingsDialog, bool isEdit = true);

        public Task ShowServerPathEditorDialog(IEnumerable<ServerPathEntry> paths,
                                               RelayCommand retryCommand,
                                               RelayCommand faqCommand);

        public Task ShowOkMessageAsync(string title, string message);

        public Task ShowAddMoneyDialog(AddableItem money, RelayCommand addCommand);

        public Task ShowAllItemsDialog(RelayCommand addCommand, bool stashSelectorVisible);

        public Task ShowProgressDialog(string title,
                                       string description,
                                       bool indeterminate = true,
                                       double progress = 0,
                                       bool cancelable = false,
                                       MetroDialogSettings dialogSettings = null);

        public Task HideProgressDialog();

        public Task OpenServerSelectHelpAsync(AppSettings appSettings);
    }

    public class MetroDialogManager(object viewModel, IDialogCoordinator dialogCoordinator) : IDialogManager
    {
        private ProgressDialogController progressDialog;

        public event EventHandler ProgressDialogCanceled;

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
            await dialogCoordinator.ShowMessageAsync(viewModel,
                                                     AppData.AppLocalization.GetLocalizedString(title),
                                                     AppData.AppLocalization.GetLocalizedString(caption),
                                                     MessageDialogStyle.AffirmativeAndNegative,
                                                     YesNoDialogSettings) == MessageDialogResult.Affirmative;

        public async Task ShutdownCozServerRunned()
        {
            if (await dialogCoordinator.ShowMessageAsync(viewModel,
                                                         AppData.AppLocalization.GetLocalizedString("app_quit"),
                                                         AppData.AppLocalization.GetLocalizedString("server_runned"),
                                                         MessageDialogStyle.Affirmative,
                                                         ShutdownDialogSettings) == MessageDialogResult.Affirmative)
                App.ApplicationManager.CloseApplication.Execute(null);
        }

        public async Task ShowSettingsDialog(RelayCommand reloadCommand,
                                             RelayCommand faqCommand,
                                             IWorker worker,
                                             IHelperModManager modManager,
                                             int index = 0)
        {
            string startValues = AppData.AppSettings.GetStamp();
            CustomDialog settingsDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("tab_settings_title"), 600);
            RelayCommand closeCommand = new(async obj =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, settingsDialog);
                string newValues = AppData.AppSettings.GetStamp();
                if (startValues != newValues)
                    reloadCommand.Execute(null);
            });
            var settingsDialogViewModel = new SettingsDialogViewModel(closeCommand,
                                                                      this,
                                                                      App.WindowsDialogs,
                                                                      App.ApplicationManager,
                                                                      modManager,
                                                                      faqCommand,
                                                                      worker,
                                                                      index);
            await ShowCustomDialog<SettingsDialog>(viewModel, settingsDialog, settingsDialogViewModel);
        }

        public async Task ShowUpdateDialog(GitHubRelease release)
        {
            CustomDialog updateDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("update_avialable"), 500);
            await ShowCustomDialog<UpdateDialog>(viewModel,
                                                 updateDialog,
                                                 new UpdateDialogViewModel(App.ApplicationManager,
                                                                           App.WindowsDialogs,
                                                                           release,
                                                                           viewModel,
                                                                           this));
        }

        public async Task<YesNoDontAskAgainDialogResult> YesNoDontAskAgainDialog(string title,
                                                                                 string yesText,
                                                                                 string noText,
                                                                                 string message,
                                                                                 bool dontAskAgain)
        {
            CustomDialog dialog = CustomDialog(title, 500);
            var dialogVM = new YesNoDontAskAgainDialogViewModel(yesText,
                                                                noText,
                                                                message,
                                                                dontAskAgain,
                                                                viewModel);
            await ShowCustomDialog<YesNoDontAskAgainDialog>(viewModel, dialog, dialogVM);
            return await dialogVM.DialogResult;
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

        public async Task ShowServerPathEditorDialog(IEnumerable<ServerPathEntry> paths, RelayCommand retryCommand, RelayCommand faqCommand)
        {
            CustomDialog pathEditorDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"), 500);
            await ShowCustomDialog<ServerPathEditor>(viewModel,
                                                     pathEditorDialog,
                                                     new ServerPathEditorViewModel(paths, retryCommand, faqCommand, viewModel));
        }

        public async Task ShowAddMoneyDialog(AddableItem money, RelayCommand addCommand)
        {
            CustomDialog addMoneyDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("tab_stash_dialog_money"), 500);
            await ShowCustomDialog<MoneyDailog>(viewModel, addMoneyDialog, new MoneyDailogViewModel(money, addCommand, viewModel));
        }

        public async Task ShowAllItemsDialog(RelayCommand addCommand, bool stashSelectorVisible)
        {
            CustomDialog allItemsDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("tab_stash_all_items"), 500);
            await ShowCustomDialog<AllItemsDialog>(viewModel,
                                                   allItemsDialog,
                                                   new AllItemsDialogViewModel(addCommand, stashSelectorVisible, viewModel));
        }

        public async Task ShowOkMessageAsync(string title, string message)
        {
            await dialogCoordinator.ShowMessageAsync(viewModel, title,
                message, MessageDialogStyle.Affirmative, OkDialogSettings);
        }

        public async Task ShowProgressDialog(string title,
                                             string description,
                                             bool indeterminate = true,
                                             double progress = 0,
                                             bool cancelable = false,
                                             MetroDialogSettings dialogSettings = null)
        {
            if (progressDialog != null && progressDialog.IsOpen)
                UpdateProgressDialog(title, description);
            else
                await CreateProgressDialog(title, description, cancelable, dialogSettings);
            SetProgress(indeterminate, progress);
        }

        public async Task HideProgressDialog()
        {
            if (progressDialog?.IsOpen ?? false)
                await progressDialog?.CloseAsync();
            progressDialog = null;
        }

        public async Task OpenServerSelectHelpAsync(AppSettings appSettings)
        {
            CustomDialog helpDialog = CustomDialog(AppData.AppLocalization.GetLocalizedString("server_select_help"), 500);
            await ShowCustomDialog<ServerSelectHelpDialog>(viewModel, helpDialog, new ServerSelectHelpDialogViewModel(appSettings, viewModel));
        }

        private static CustomDialog CustomDialog(string title, double width) => new()
        {
            Title = title,
            DialogContentWidth = new GridLength(width)
        };

        private async Task ShowCustomDialog<T>(object context, CustomDialog dialog, BindableViewModel viewModel) where T : UserControl
        {
            T control = Activator.CreateInstance<T>();
            control.DataContext = viewModel;
            dialog.Content = control;
            await dialogCoordinator.ShowMetroDialogAsync(context, dialog);
        }

        private void UpdateProgressDialog(string title, string description)
        {
            progressDialog?.SetTitle(title);
            progressDialog?.SetMessage(description);
        }

        private async Task CreateProgressDialog(string title, string description, bool cancelable, MetroDialogSettings dialogSettings)
        {
            progressDialog = await dialogCoordinator.ShowProgressAsync(viewModel,
                                                                       title,
                                                                       description,
                                                                       cancelable,
                                                                       dialogSettings);
            progressDialog.Canceled += ProgressDialogCanceled;
        }

        private void SetProgress(bool indeterminate, double progress)
        {
            if (indeterminate)
                progressDialog?.SetIndeterminate();
            else
                progressDialog?.SetProgress(progress);
        }
    }
}