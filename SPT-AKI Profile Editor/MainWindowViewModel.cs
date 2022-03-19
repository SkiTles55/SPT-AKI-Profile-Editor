using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.IO;

namespace SPT_AKI_Profile_Editor
{
    public class MainWindowViewModel : BindableViewModel
    {
        public MainWindowViewModel()
        {
            App.DialogCoordinator = DialogCoordinator.Instance;
            App.Worker = new Worker(App.DialogCoordinator, this);
            Instance = this;
        }

        public static MainWindowViewModel Instance { get; set; }

        public static RelayCommand OpenFastModeCommand => new(obj =>
          {
              AppData.AppSettings.FastModeOpened = !AppData.AppSettings.FastModeOpened;
          });

        public static RelayCommand OpenFAQ => new(obj =>
          {
              var link = AppData.AppSettings.Language == "ru" ? "https://github.com/SkiTles55/SPT-AKI-Profile-Editor/blob/master/FAQ.md" : "https://github.com/SkiTles55/SPT-AKI-Profile-Editor/blob/master/ENGFAQ.md";
              ExtMethods.OpenUrl(link);
          });

        public static RelayCommand SaveButtonCommand => new(obj =>
          {
              SaveProfileAndReload();
          });

        public static string WindowTitle => UpdatesChecker.GetAppTitleWithVersion();

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
                 StartupEventsWorker();
         });

        public RelayCommand ReloadButtonCommand => new(async obj =>
         {
             if (await Dialogs.YesNoDialog(this,
                 "reload_profile_dialog_title",
                 "reload_profile_dialog_caption") == MessageDialogResult.Affirmative)
                 StartupEventsWorker();
         });

        public static void SaveProfileAndReload()
        {
            App.Worker.AddAction(new WorkerTask
            {
                Action = () =>
                {
                    AppData.BackupService.CreateBackup();
                    Profile.Save(Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList["dir_profiles"], AppData.AppSettings.DefaultProfile));
                },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("save_profile_dialog_title"),
                WorkerNotification = new()
                {
                    NotificationTitle = AppLocalization.GetLocalizedString("save_profile_dialog_title"),
                    NotificationDescription = AppLocalization.GetLocalizedString("save_profile_dialog_caption")
                }
            });
            StartupEventsWorker();
        }

        public static async void StartupEventsWorker()
        {
            if (ExtMethods.PathIsServerFolder(AppData.AppSettings) && ServerChecker.CheckProcess())
                await Dialogs.ShutdownCozServerRunned(Instance);
            App.Worker.AddAction(new WorkerTask
            {
                Action = AppData.StartupEvents,
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
            });
        }
    }
}