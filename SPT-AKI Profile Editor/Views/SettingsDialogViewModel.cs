using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SPT_AKI_Profile_Editor.Views
{
    public class SettingsDialogViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IApplicationManager _applicationManager;
        private readonly RelayCommand _faqCommand;
        private readonly IWorker _worker;
        private int selectedTab;

        public SettingsDialogViewModel(RelayCommand closeCommand,
                                       IDialogManager dialogManager,
                                       IWindowsDialogs windowsDialogs,
                                       IApplicationManager applicationManager,
                                       IHelperModManager helperModManager,
                                       RelayCommand faqCommand,
                                       IWorker worker,
                                       int index = 0)
        {
            CloseCommand = closeCommand;
            SelectedTab = index;
            AppSettings = AppData.AppSettings;
            _dialogManager = dialogManager;
            _windowsDialogs = windowsDialogs;
            _applicationManager = applicationManager;
            HelperModManager = helperModManager;
            _faqCommand = faqCommand;
            _worker = worker;
        }

        public static SolidColorBrush SuccessColor => HelperModStatusNameColorConverter.SuccessColor;

        public RelayCommand ResetLocalizations => new(obj => _applicationManager.DeleteLocalizations());

        public RelayCommand OpenHowToUseHelperMod => new(obj => OpenUrl(LinksHelper.HelperModManual));

        public IEnumerable<AccentItem> ColorSchemes => _applicationManager.GetColorSchemes();

        public RelayCommand CloseCommand { get; private set; }

        public RelayCommand OpenAppData => new(obj => _applicationManager.OpenUrl(DefaultValues.AppDataFolder));

        public RelayCommand ServerSelectHelp => new(obj => _dialogManager.OpenServerSelectHelpAsync(AppSettings));

        public RelayCommand QuitCommand => _applicationManager.CloseApplication;

        public RelayCommand ResetAndReload => new(async obj =>
        {
            if (obj is RelayCommand command)
            {
                try
                {
                    command.Execute(null);
                    _applicationManager.RestartApplication();
                }
                catch (Exception ex)
                {
                    await _dialogManager.ShowOkMessageAsync(AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                                                            ex.Message);
                }
            }
        });

        public RelayCommand ResetSettings => new(obj => _applicationManager.DeleteSettings());

        public RelayCommand InstallMod => new(obj => _worker.AddTask(ProgressTask(() =>
        {
            HelperModManager.InstallMod();
            UsingModHelper = true;
        })));

        public RelayCommand ReinstallMod => new(obj => _worker.AddTask(ProgressTask(() =>
        {
            HelperModManager.RemoveMod();
            HelperModManager.InstallMod();
            OnPropertyChanged(nameof(HelperModManager));
        })));

        public RelayCommand UpdateMod => new(obj => _worker.AddTask(ProgressTask(() =>
        {
            HelperModManager.UpdateMod();
            OnPropertyChanged(nameof(HelperModManager));
        })));

        public RelayCommand RemoveMod => new(obj => _worker.AddTask(ProgressTask(() =>
        {
            HelperModManager.RemoveMod();
            UsingModHelper = false;
        })));

        public AppSettings AppSettings { get; }

        public IHelperModManager HelperModManager { get; }

        public int SelectedTab
        {
            get => selectedTab;
            set
            {
                selectedTab = value;
                OnPropertyChanged("SelectedTab");
            }
        }

        public string CurrentLocalization
        {
            get => AppSettings.Language;
            set
            {
                AppSettings.Language = value;
                OnPropertyChanged("CurrentLocalization");
                AppLocalization.LoadLocalization(AppSettings.Language);
                OnPropertyChanged(nameof(HelperModManager));
            }
        }

        public bool UsingModHelper
        {
            get => AppSettings.UsingModHelper;
            set
            {
                AppSettings.UsingModHelper = value;
                OnPropertyChanged(nameof(UsingModHelper));
                OnPropertyChanged(nameof(HelperModManager));
            }
        }

        public string ServerPath
        {
            get => AppSettings.ServerPath;
            set
            {
                AppSettings.ServerPath = value;
                OnPropertyChanged(nameof(ServerPath));
                OnPropertyChanged(nameof(ServerPathValid));
                OnPropertyChanged(nameof(ServerHasAccounts));
                OnPropertyChanged(nameof(HelperModManager));
            }
        }

        public string ColorScheme
        {
            get => AppSettings.ColorScheme;
            set
            {
                AppSettings.ColorScheme = value;
                OnPropertyChanged(nameof(ColorScheme));
                _applicationManager.ChangeTheme();
            }
        }

        public bool ServerPathValid => AppSettings.PathIsServerFolder();

        public bool ServerHasAccounts => AppSettings.ServerHaveProfiles();

        public RelayCommand ServerSelect => new(async obj => await ServerSelectDialog());

        public RelayCommand OpenLocalizationEditor
            => new(async obj => await _dialogManager.ShowLocalizationEditorDialog(this, (bool)obj));

        private RelayCommand ServerPathEditorRetryCommand => new(async obj =>
        {
            if (obj is IEnumerable<ServerPathEntry> pathList)
            {
                AppSettings.FilesList = pathList.Where(x => x.Key.StartsWith(SPTServerFile.prefix))
                                            .ToDictionary(x => x.Key, y => y.Path);
                AppSettings.DirsList = pathList.Where(x => x.Key.StartsWith(SPTServerDir.prefix))
                                               .ToDictionary(x => x.Key, y => y.Path);
                AppSettings.Save();
                await ServerSelectDialog();
            }
        });

        private async Task ServerSelectDialog()
        {
            string startPath = null;
            if (!string.IsNullOrEmpty(ServerPath) && Directory.Exists(ServerPath))
                startPath = ServerPath;
            var (success, path) = _windowsDialogs.FolderBrowserDialog(false,
                                                                      startPath,
                                                                      AppLocalization.GetLocalizedString("server_select"));
            if (success)
            {
                var checkResult = AppSettings.CheckServerPath(path);
                if (checkResult?.All(x => x.IsFounded) == true)
                {
                    ServerPath = path;
                    return;
                }
                await _dialogManager.ShowServerPathEditorDialog(checkResult, ServerPathEditorRetryCommand, _faqCommand);
            }
        }

        private void OpenUrl(string url) => _applicationManager.OpenUrl(url);
    }
}