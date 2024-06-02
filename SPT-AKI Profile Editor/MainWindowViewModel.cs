using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor
{
    public class MainWindowViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;
        private readonly ICleaningService _cleaningService;
        private readonly IHelperModManager _helperModManager;
        private readonly ServerConfigs _serverConfigs;

        public MainWindowViewModel(IApplicationManager applicationManager,
                                   IWindowsDialogs windowsDialogs,
                                   IHelperModManager helperModManager,
                                   ServerConfigs serverConfigs,
                                   IDialogManager dialogManager = null,
                                   IWorker worker = null,
                                   ICleaningService cleaningService = null)
        {
            _dialogManager = dialogManager ?? new MetroDialogManager(this, App.DialogCoordinator);
            _helperModManager = helperModManager;
            _windowsDialogs = windowsDialogs;
            _applicationManager = applicationManager;
            _worker = worker ?? new Worker(_dialogManager);
            _cleaningService = cleaningService;
            _serverConfigs = serverConfigs;
            ViewModels = new(_dialogManager,
                             _worker,
                             _applicationManager,
                             _windowsDialogs,
                             SaveButtonCommand,
                             ReloadCommand,
                             OpenFAQ,
                             _cleaningService,
                             _serverConfigs);
        }

        public static RelayCommand OpenFastModeCommand => new(obj => ChangeMode());

        public string WindowTitle => _applicationManager.GetAppTitleWithVersion();

        public RelayCommand OpenFAQ => new(obj => OpenFAQUrl());

        public RelayCommand OpenDiscord => new(obj => _applicationManager.OpenDiscord());

        public ViewModelsFactory ViewModels { get; }

        public RelayCommand SaveButtonCommand => new(obj => SaveProfileAndReload());

        public RelayCommand OpenSettingsCommand
            => new(async obj => await _dialogManager.ShowSettingsDialog(ReloadCommand,
                                                                        OpenFAQ,
                                                                        _worker,
                                                                        _helperModManager));

        public RelayCommand InitializeViewModelCommand => new(async obj => await InitializeViewModel());

        public RelayCommand ReloadButtonCommand => new(async obj => await Reload());

        private RelayCommand ReloadCommand => new(obj => StartupEventsWorker());

        public async void SaveProfileAndReload()
        {
            if (AppData.AppSettings.IssuesAction != IssuesAction.AlwaysIgnore)
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
            }
            SaveAction();
        }

        public async void StartupEventsWorker()
        {
            if (AppData.AppSettings.PathIsServerFolder() && _applicationManager.CheckProcess())
                await _dialogManager.ShutdownCozServerRunned();
            _applicationManager.CloseItemViewWindows();
            _worker.AddTask(ProgressTask(() => AppData.StartupEvents(_cleaningService),
                                         AppLocalization.GetLocalizedString("progress_dialog_caption")));
        }

        public async Task<bool> ConfirmShutdown() => await _dialogManager.YesNoDialog(AppLocalization.GetLocalizedString("app_quit"),
                                                                                      AppLocalization.GetLocalizedString("reload_profile_dialog_caption"));

        private static void ChangeMode() => AppData.AppSettings.FastModeOpened = !AppData.AppSettings.FastModeOpened;

        private static bool NeedShowSettings()
            => string.IsNullOrEmpty(AppData.AppSettings.ServerPath)
            || !AppData.AppSettings.PathIsServerFolder()
            || !AppData.AppSettings.ServerHaveProfiles()
            || string.IsNullOrEmpty(AppData.AppSettings.DefaultProfile);

        private void SaveProfileAction()
        {
            AppData.BackupService.CreateBackup();
            var results = Profile.Save(Path.Combine(AppData.AppSettings.ServerPath,
                                                    AppData.AppSettings.DirsList[SPTServerDir.profiles],
                                                    AppData.AppSettings.DefaultProfile));
            ShowSaveResults(results);
        }

        private async void ShowSaveResults(List<SaveException> exceptions)
        {
            if (exceptions.Any())
            {
                string messageKey = exceptions.HaveAllErrors()
                    ? "profile_not_saved_dialog_caption"
                    : "profile_saved_with_errors_dialog_caption";
                await _dialogManager.ShowOkMessageAsync(AppLocalization.GetLocalizedString(messageKey),
                                                        exceptions.GetLocalizedDescription());
                return;
            }

            await _dialogManager.ShowOkMessageAsync(AppLocalization.GetLocalizedString("save_profile_dialog_title"),
                                                        AppLocalization.GetLocalizedString("save_profile_dialog_caption"));
        }

        private void OpenFAQUrl() => _applicationManager.OpenUrl(LinksHelper.FAQ);

        private void SaveAction()
        {
            _worker.AddTask(ProgressTask(() => SaveProfileAction(),
                                         AppLocalization.GetLocalizedString("save_profile_dialog_title")));
            StartupEventsWorker();
        }

        private async Task InitializeViewModel()
        {
            _applicationManager.ChangeTheme();
            if (NeedShowSettings())
                await _dialogManager.ShowSettingsDialog(ReloadCommand,
                                                        OpenFAQ,
                                                        _worker,
                                                        _helperModManager);
            else
                StartupEventsWorker();
            if (AppData.AppSettings.CheckUpdates == true)
            {
                await CheckForUpdates();
                _ = _helperModManager.DownloadUpdates();
            }
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