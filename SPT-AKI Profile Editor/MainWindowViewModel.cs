using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Reflection;

namespace SPT_AKI_Profile_Editor
{
    class MainWindowViewModel
    {
        public MainWindowViewModel(IDialogCoordinator instance) => dialogCoordinator = instance;
        public static AppLocalization AppLocalization => App.appLocalization;
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            CustomDialog settingsDialog = new() { Title = App.appLocalization.GetLocalizedString("tab_settings_title") };
            RelayCommand closeCommand = new(async obj =>
            {
                await dialogCoordinator.HideMetroDialogAsync(this, settingsDialog);
            });
            settingsDialog.Content = new SettingsDialog { DataContext = new SettingsDialogViewModel(dialogCoordinator, closeCommand) };
            await dialogCoordinator.ShowMetroDialogAsync(this, settingsDialog);
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
    }
}