using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        public static Profile Profile => AppData.Profile;
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            await Dialogs.ShowSettingsDialog(this);
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
                await Dialogs.ShowSettingsDialog(this);
            else
                App.StartupEventsWorker();
        });
        public static RelayCommand SaveButtonCommand => new(obj =>
        {
            SaveProfileAndReload();
        });
        public static RelayCommand ReloadButtonCommand => new(obj =>
        {
            App.StartupEventsWorker();
        });
        public static string WindowTitle
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"SPT-AKI Profile Editor {$" {version.Major}.{version.Minor}"}";
            }
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
            App.StartupEventsWorker();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}