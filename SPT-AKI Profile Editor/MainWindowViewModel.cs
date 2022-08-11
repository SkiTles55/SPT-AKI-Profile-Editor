using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Views;
using System.IO;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor
{
    public class MainWindowViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;

        public MainWindowViewModel(IDialogManager dialogManager,
                                   IWorker worker = null,
                                   IApplicationManager applicationManager = null)
        {
            _dialogManager = dialogManager;
            _worker = worker ?? new Worker(App.DialogCoordinator, this, _dialogManager);
            Instance = this;
            _applicationManager = applicationManager ?? App.ApplicationManager;
        }

        public static MainWindowViewModel Instance { get; set; }

        public static RelayCommand OpenFastModeCommand => new(obj => ChangeMode());

        public static RelayCommand OpenFAQ => new(obj => OpenFAQUrl());

        public static string WindowTitle => UpdatesChecker.GetAppTitleWithVersion();

        public BackupsTabViewModel BackupsTabViewModel => new(_dialogManager, _worker);

        public StashTabViewModel StashTabViewModel => new(_dialogManager, _worker, _applicationManager);

        public WeaponBuildsViewModel WeaponBuildsViewModel => new(_dialogManager, _worker);

        public RelayCommand SaveButtonCommand => new(obj => SaveProfileAndReload());

        public RelayCommand OpenSettingsCommand => new(async obj => await _dialogManager.ShowSettingsDialog(this));

        public RelayCommand InitializeViewModelCommand => new(async obj => await InitializeViewModel());

        public RelayCommand ReloadButtonCommand => new(async obj => await Reload());

        public async void SaveProfileAndReload()
        {
            AppData.IssuesService.GetIssues();
            if (AppData.IssuesService.HasIssues)
            {
                switch (AppData.AppSettings.IssuesAction)
                {
                    case IssuesAction.AlwaysShow:
                        RelayCommand saveCommand = new(obj => SaveAction());
                        await _dialogManager.ShowIssuesDialog(Instance, saveCommand, AppData.IssuesService);
                        return;

                    case IssuesAction.AlwaysFix:
                        AppData.IssuesService.FixAllIssues();
                        break;
                }
            }
            SaveAction();
        }

        public async void StartupEventsWorker()
        {
            if (AppData.AppSettings.PathIsServerFolder() && ServerChecker.CheckProcess())
                await _dialogManager.ShutdownCozServerRunned(Instance);
            _applicationManager.CloseItemViewWindows();
            _worker.AddTask(new WorkerTask
            {
                Action = AppData.StartupEvents,
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
            });
        }

        private static WorkerTask SaveProfileTask()
        {
            return new WorkerTask
            {
                Action = () =>
                {
                    AppData.BackupService.CreateBackup();
                    Profile.Save(Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList[SPTServerDir.profiles], AppData.AppSettings.DefaultProfile));
                },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("save_profile_dialog_title"),
                WorkerNotification = new()
                {
                    NotificationTitle = AppLocalization.GetLocalizedString("save_profile_dialog_title"),
                    NotificationDescription = AppLocalization.GetLocalizedString("save_profile_dialog_caption")
                }
            };
        }

        private static void ChangeMode() => AppData.AppSettings.FastModeOpened = !AppData.AppSettings.FastModeOpened;

        private static void OpenFAQUrl()
        {
            var link = $"https://github.com/{AppData.AppSettings.repoAuthor}/{AppData.AppSettings.repoName}/blob/master/{(AppData.AppSettings.Language == "ru" ? "FAQ" : "ENGFAQ")}.md";
            ExtMethods.OpenUrl(link);
        }

        private void SaveAction()
        {
            _worker.AddTask(SaveProfileTask());
            StartupEventsWorker();
        }

        private async Task InitializeViewModel()
        {
            _applicationManager.ChangeTheme();
            if (string.IsNullOrEmpty(AppData.AppSettings.ServerPath)
            || !AppData.AppSettings.PathIsServerFolder()
            || !AppData.AppSettings.ServerHaveProfiles()
            || string.IsNullOrEmpty(AppData.AppSettings.DefaultProfile))
                await _dialogManager.ShowSettingsDialog(this);
            else
                StartupEventsWorker();
            if (AppData.AppSettings.CheckUpdates == true)
                await CheckForUpdates();
        }

        private async Task CheckForUpdates()
        {
            var release = await UpdatesChecker.CheckUpdate();
            if (release != null)
                await _dialogManager.ShowUpdateDialog(this, release);
        }

        private async Task Reload()
        {
            if (await _dialogManager.YesNoDialog(this, "reload_profile_dialog_title", "reload_profile_dialog_caption"))
                StartupEventsWorker();
        }
    }
}