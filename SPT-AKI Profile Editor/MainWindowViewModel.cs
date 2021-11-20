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
            dialogCoordinator = DialogCoordinator.Instance;
            worker = new Worker(dialogCoordinator, this)
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
            if (string.IsNullOrEmpty(AppData.AppSettings.ServerPath)
            || !ExtMethods.PathIsServerFolder(AppData.AppSettings)
            || !ExtMethods.ServerHaveProfiles(AppData.AppSettings)
            || string.IsNullOrEmpty(AppData.AppSettings.DefaultProfile))
                await ShowSettingsDialog();
            else
                StartupEventsWorker();
        });
        public RelayCommand SaveButtonCommand => new(obj =>
        {
            SaveProfileAndReload();
        });
        public RelayCommand ReloadButtonCommand => new(obj =>
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
        private readonly IDialogCoordinator dialogCoordinator;
        private readonly Worker worker;
        private async Task ShowSettingsDialog()
        {
            string startValues = AppSettings.GetStamp();
            CustomDialog settingsDialog = new()
            {
                Title = AppLocalization.GetLocalizedString("tab_settings_title"),
                DialogContentWidth = new GridLength(500)
            };
            RelayCommand closeCommand = new(async obj =>
            {
                await dialogCoordinator.HideMetroDialogAsync(this, settingsDialog);
                string newValues = AppSettings.GetStamp();
                if (startValues != newValues)
                    StartupEventsWorker();
            });
            settingsDialog.Content = new SettingsDialog { DataContext = new SettingsDialogViewModel(dialogCoordinator, closeCommand) };
            await dialogCoordinator.ShowMetroDialogAsync(this, settingsDialog);
        }
        private void SaveProfileAndReload()
        {
            worker.AddAction(new WorkerTask
            {
                Action = () => { AppData.Profile.Save(Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList["dir_profiles"], AppData.AppSettings.DefaultProfile)); },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("save_profile_dialog_title")
            });
            StartupEventsWorker();
        }
        private void StartupEventsWorker()
        {
            worker.AddAction(new WorkerTask
            {
                Action = StartupEvents,
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
            });
        }
        private void StartupEvents()
        {
            AppData.LoadDatabase();
            AppData.Profile.Load(Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList["dir_profiles"], AppData.AppSettings.DefaultProfile));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}