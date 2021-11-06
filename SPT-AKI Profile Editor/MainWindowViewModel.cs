using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    class MainWindowViewModel
    {
        public MainWindowViewModel(IDialogCoordinator instance) => dialogCoordinator = instance;
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            await ShowSettingsDialog();
        });
        public RelayCommand InitializeViewModelCommand => new(async obj =>
        {
            App.ChangeTheme();
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
                if (AppData.AppSettings.NeedReload)
                {
                    await StartupEvents();
                }
            });
            settingsDialog.Content = new SettingsDialog { DataContext = new SettingsDialogViewModel(dialogCoordinator, closeCommand) };
            await dialogCoordinator.ShowMetroDialogAsync(this, settingsDialog);
        }
        private async Task StartupEvents()
        {
            if (string.IsNullOrEmpty(AppData.AppSettings.ServerPath)
            || !ExtMethods.PathIsServerFolder(AppData.AppSettings)
            || !ExtMethods.ServerHaveProfiles(AppData.AppSettings)
            || string.IsNullOrEmpty(AppData.AppSettings.DefaultProfile))
                await ShowSettingsDialog();
            else
            {
                AppData.Profile.Load(Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList["dir_profiles"], AppData.AppSettings.DefaultProfile));
            }
        }
    }
}