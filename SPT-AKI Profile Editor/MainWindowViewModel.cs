using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor
{
    public class MainWindowViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;

        public MainWindowViewModel(IApplicationManager applicationManager,
                                   IWindowsDialogs windowsDialogs,
                                   IDialogManager dialogManager = null,
                                   IWorker worker = null)
        {
            _dialogManager = dialogManager ?? new MetroDialogManager(this);
            _windowsDialogs = windowsDialogs;
            _applicationManager = applicationManager;
            _worker = worker ?? new Worker(App.DialogCoordinator, this, _dialogManager);
            ViewModels = new(_dialogManager, _worker, _applicationManager, _windowsDialogs, SaveButtonCommand);
            Instance = this;
        }

        public static MainWindowViewModel Instance { get; set; }

        public static RelayCommand OpenFastModeCommand => new(obj => ChangeMode());

        public string WindowTitle => _applicationManager.GetAppTitleWithVersion();

        public RelayCommand OpenFAQ => new(obj => OpenFAQUrl());

        public ViewModelsFactory ViewModels { get; }

        public RelayCommand SaveButtonCommand => new(obj => SaveProfileAndReload());

        public RelayCommand OpenSettingsCommand => new(async obj => await _dialogManager.ShowSettingsDialog());

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
                        await _dialogManager.ShowIssuesDialog(saveCommand, AppData.IssuesService);
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
            if (AppData.AppSettings.PathIsServerFolder() && _applicationManager.CheckProcess())
                await _dialogManager.ShutdownCozServerRunned();
            _applicationManager.CloseItemViewWindows();
            _worker.AddTask(new WorkerTask
            {
                Action = AppData.StartupEvents,
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
            });
        }

        public async Task<bool> ConfirmShutdown() => await _dialogManager.YesNoDialog(AppLocalization.GetLocalizedString("app_quit"),
                                                                                      AppLocalization.GetLocalizedString("reload_profile_dialog_caption"));

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

        private void OpenFAQUrl()
        {
            var link = $"https://github.com/{AppData.AppSettings.repoAuthor}/{AppData.AppSettings.repoName}/blob/master/{(AppData.AppSettings.Language == "ru" ? "FAQ" : "ENGFAQ")}.md";
            _applicationManager.OpenUrl(link);
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
                await _dialogManager.ShowSettingsDialog();
            else
                StartupEventsWorker();
            if (AppData.AppSettings.CheckUpdates == true)
                await CheckForUpdates();
        }

        private async Task CheckForUpdates()
        {
            var release = await _applicationManager.CheckUpdate();
            if (release != null)
                await _dialogManager.ShowUpdateDialog(release);
        }

        private async Task Reload()
        {
            if (await _dialogManager.YesNoDialog("reload_profile_dialog_title", "reload_profile_dialog_caption"))
                StartupEventsWorker();
        }
    }
}