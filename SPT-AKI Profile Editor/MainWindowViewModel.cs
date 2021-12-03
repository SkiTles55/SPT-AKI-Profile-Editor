using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            App.dialogCoordinator = DialogCoordinator.Instance;
            App.worker = new Worker(App.dialogCoordinator, this)
            {
                ErrorTitle = AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                ErrorConfirm = AppLocalization.GetLocalizedString("save_profile_dialog_ok")
            };
        }
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            await ShowSettingsDialog();
        });
        public RelayCommand InitializeViewModelCommand => new(async obj =>
        {
            App.ChangeTheme();
            if (UpdatesChecker.CheckUpdate())
            {
                if (await Dialogs.YesNoDialog(this,
                "update_avialable",
                "update_caption") == MessageDialogResult.Affirmative)
                    ExtMethods.OpenUrl(AppSettings.RepositoryLink);
            }
            if (string.IsNullOrEmpty(AppData.AppSettings.ServerPath)
            || !ExtMethods.PathIsServerFolder(AppData.AppSettings)
            || !ExtMethods.ServerHaveProfiles(AppData.AppSettings)
            || string.IsNullOrEmpty(AppData.AppSettings.DefaultProfile))
                await ShowSettingsDialog();
            else
                StartupEventsWorker();
        });
        public static RelayCommand SaveButtonCommand => new(obj =>
        {
            SaveProfileAndReload();
        });
        public static RelayCommand ReloadButtonCommand => new(obj =>
        {
            StartupEventsWorker();
        });
        public static string WindowTitle
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"SPT-AKI Profile Editor {$" {version.Major}.{version.Minor}"}";
            }
        }
        
        private async Task ShowSettingsDialog()
        {
            string startValues = AppSettings.GetStamp();
            CustomDialog settingsDialog = new()
            {
                Title = AppLocalization.GetLocalizedString("tab_settings_title"),
                DialogContentWidth = new GridLength(600)
            };
            RelayCommand closeCommand = new(async obj =>
            {
                await App.dialogCoordinator.HideMetroDialogAsync(this, settingsDialog);
                string newValues = AppSettings.GetStamp();
                if (startValues != newValues)
                    StartupEventsWorker();
            });
            settingsDialog.Content = new SettingsDialog { DataContext = new SettingsDialogViewModel(closeCommand) };
            await App.dialogCoordinator.ShowMetroDialogAsync(this, settingsDialog);
        }
        private static void SaveProfileAndReload()
        {
            App.worker.AddAction(new WorkerTask
            {
                Action = () =>
                {
                    AppData.BackupService.CreateBackup();
                    AppData.Profile.Save(Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList["dir_profiles"], AppData.AppSettings.DefaultProfile));
                },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("save_profile_dialog_title")
            });
            StartupEventsWorker();
        }
        private static void StartupEventsWorker()
        {
            App.worker.AddAction(new WorkerTask
            {
                Action = AppData.StartupEvents,
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}