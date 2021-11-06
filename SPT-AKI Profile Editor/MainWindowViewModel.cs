using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    class MainWindowViewModel
    {
        public MainWindowViewModel(IDialogCoordinator instance) => dialogCoordinator = instance;
        public static AppLocalization AppLocalization => App.appLocalization;
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            await ShowSettingsDialog();
        });
        public RelayCommand InitializeViewModelCommand => new(async obj =>
        {
            await StartupEvents();
        });
        public static string WindowTitle
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"SPT-AKI Profile Editor {$" {version.Major}.{version.Minor}"}";
            }
        }
        private readonly IDialogCoordinator dialogCoordinator;
        private async Task ShowSettingsDialog()
        {
            CustomDialog settingsDialog = new()
            {
                Title = AppLocalization.GetLocalizedString("tab_settings_title"),
                DialogContentWidth = new GridLength(500)
            };
            RelayCommand closeCommand = new(async obj =>
            {
                await dialogCoordinator.HideMetroDialogAsync(this, settingsDialog);
                if (App.appSettings.NeedReload)
                {
                    //Reload events
                }
            });
            settingsDialog.Content = new SettingsDialog { DataContext = new SettingsDialogViewModel(dialogCoordinator, closeCommand) };
            await dialogCoordinator.ShowMetroDialogAsync(this, settingsDialog);
        }
        private async Task StartupEvents()
        {
            if (string.IsNullOrEmpty(App.appSettings.ServerPath)
            || !ExtMethods.PathIsServerFolder(App.appSettings)
            || !ExtMethods.ServerHaveProfiles(App.appSettings)
            || string.IsNullOrEmpty(App.appSettings.DefaultProfile))
                await ShowSettingsDialog();
        }
    }
}