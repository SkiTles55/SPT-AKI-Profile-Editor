using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            App.DialogCoordinator = DialogCoordinator.Instance;
            App.Worker = new Worker(App.DialogCoordinator, this);
        }
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static Profile Profile => AppData.Profile;
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            await Dialogs.ShowSettingsDialog(this);
        });
        public static RelayCommand OpenFastModeCommand => new(obj =>
        {
            AppData.AppSettings.FastModeOpened = !AppData.AppSettings.FastModeOpened;
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
        public RelayCommand ReloadButtonCommand => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this,
                "reload_profile_dialog_title",
                "reload_profile_dialog_caption") == MessageDialogResult.Affirmative)
                App.StartupEventsWorker();
        });
        public static string WindowTitle => UpdatesChecker.GetAppTitleWithVersion();
        public static void SaveProfileAndReload()
        {
            App.Worker.AddAction(new WorkerTask
            {
                Action = () =>
                {
                    AppData.BackupService.CreateBackup();
                    AppData.Profile.Save(Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList["dir_profiles"], AppData.AppSettings.DefaultProfile));
                },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("save_profile_dialog_title"),
                WorkerNotification = new ()
                {
                    NotificationTitle = AppLocalization.GetLocalizedString("save_profile_dialog_title"),
                    NotificationDescription = AppLocalization.GetLocalizedString("save_profile_dialog_caption")
                } 
            });
            App.StartupEventsWorker();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}