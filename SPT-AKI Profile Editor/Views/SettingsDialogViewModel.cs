using ControlzEx.Theming;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor
{
    public class SettingsDialogViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IApplicationManager _applicationManager;
        private int selectedTab;

        public SettingsDialogViewModel(RelayCommand command, IDialogManager dialogManager, IApplicationManager applicationManager, int index = 0)
        {
            CloseCommand = command;
            SelectedTab = index;
            AppSettings = AppData.AppSettings;
            _dialogManager = dialogManager;
            _applicationManager = applicationManager;
        }

        public static IEnumerable<AccentItem> ColorSchemes => ThemeManager.Current.Themes
            .OrderBy(x => x.DisplayName)
            .Select(x => new AccentItem(x));

        public static RelayCommand CloseCommand { get; set; }
        public static RelayCommand ResetLocalizations => new(obj => Directory.Delete(AppLocalization.localizationsDir, true));
        public RelayCommand OpenAppData => new(obj => _applicationManager.OpenUrl(DefaultValues.AppDataFolder));
        public RelayCommand QuitCommand => _applicationManager.CloseApplication;

        public RelayCommand ResetAndReload => new(async obj =>
        {
            try
            {
                if (obj is RelayCommand command)
                {
                    command.Execute(null);
                    ReloadApplication();
                }
            }
            catch (Exception ex)
            {
                await _dialogManager.ShowOkMessageAsync(MainWindowViewModel.Instance,
                                                 AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                                                 ex.Message);
            }
        });

        public RelayCommand ResetSettings => new(obj => File.Delete(AppSettings.configurationFile));
        public AppSettings AppSettings { get; }

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
            }
        }

        public string ServerPath
        {
            get => AppSettings.ServerPath;
            set
            {
                AppSettings.ServerPath = value;
                OnPropertyChanged("ServerPath");
                OnPropertyChanged("ServerPathValid");
                OnPropertyChanged("ServerHasAccounts");
            }
        }

        public string ColorScheme
        {
            get => AppSettings.ColorScheme;
            set
            {
                AppSettings.ColorScheme = value;
                OnPropertyChanged("ColorScheme");
                _applicationManager.ChangeTheme();
            }
        }

        public bool ServerPathValid => AppSettings.PathIsServerFolder();

        public bool ServerHasAccounts => AppSettings.ServerHaveProfiles();

        public RelayCommand ServerSelect => new(async obj => await ServerSelectDialog());

        public RelayCommand OpenLocalizationEditor => new(async obj => await _dialogManager.ShowLocalizationEditorDialog(this, (bool)obj));

        private RelayCommand ServerPathEditorRetryCommand => new(async obj =>
        {
            if (obj is not IEnumerable<ServerPathEntry> pathList)
                return;
            AppSettings.FilesList = pathList.Where(x => x.Key.StartsWith(SPTServerFile.prefix))
                                            .ToDictionary(x => x.Key, y => y.Path);
            AppSettings.DirsList = pathList.Where(x => x.Key.StartsWith(SPTServerDir.prefix))
                                           .ToDictionary(x => x.Key, y => y.Path);
            AppSettings.Save();
            await ServerSelectDialog();
        });

        private static void ReloadApplication()
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private async Task ServerSelectDialog()
        {
            var folderBrowserDialog = WindowsDialogs.FolderBrowserDialog(false, AppLocalization.GetLocalizedString("server_select"));
            if (!string.IsNullOrEmpty(ServerPath) && Directory.Exists(ServerPath))
                folderBrowserDialog.SelectedPath = ServerPath;
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            var checkResult = AppSettings.CheckServerPath(folderBrowserDialog.SelectedPath);
            if (checkResult?.All(x => x.IsFounded) == true)
            {
                ServerPath = folderBrowserDialog.SelectedPath;
                return;
            }
            await _dialogManager.ShowServerPathEditorDialog(this, checkResult, ServerPathEditorRetryCommand);
        }
    }
}