using ControlzEx.Theming;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
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
        private int selectedTab;

        public SettingsDialogViewModel(RelayCommand command, int index = 0)
        {
            CloseCommand = command;
            SelectedTab = index;
        }

        public static IEnumerable<AccentItem> ColorSchemes => ThemeManager.Current.Themes
            .OrderBy(x => x.DisplayName)
            .Select(x => new AccentItem(x));

        public static Dictionary<string, string> LocalizationsList => AppLocalization.Localizations;
        public static RelayCommand CloseCommand { get; set; }
        public static RelayCommand QuitCommand => App.CloseApplication;
        public static RelayCommand OpenAppData => new(obj => ExtMethods.OpenUrl(DefaultValues.AppDataFolder));
        public static RelayCommand ResetSettings => new(obj => File.Delete(AppSettings.configurationFile));
        public static RelayCommand ResetLocalizations => new(obj => Directory.Delete(AppLocalization.localizationsDir, true));

        public static RelayCommand ResetAndReload => new(async obj =>
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
                await Dialogs.ShowOkMessageAsync(MainWindowViewModel.Instance, AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"), ex.Message);
            }
        });

        public AppSettings AppSettings => AppData.AppSettings;

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
                App.ChangeTheme();
            }
        }

        public bool ServerPathValid => AppSettings.PathIsServerFolder();

        public bool ServerHasAccounts => AppSettings.ServerHaveProfiles();

        public RelayCommand ServerSelect => new(async obj => await ServerSelectDialog());

        public RelayCommand OpenLocalizationEditor => new(async obj => await Dialogs.ShowLocalizationEditorDialog(this, (bool)obj));

        private static void ReloadApplication()
        {
            System.Windows.Forms.Application.Restart();
            Environment.Exit(0);
        }

        private async Task ServerSelectDialog()
        {
            var folderBrowserDialog = WindowsDialogs.FolderBrowserDialog(false, AppLocalization.GetLocalizedString("server_select"));
            bool pathOK = false;
            do
            {
                if (!string.IsNullOrWhiteSpace(ServerPath) && Directory.Exists(ServerPath))
                    folderBrowserDialog.SelectedPath = ServerPath;
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    pathOK = false;
                if (AppSettings.PathIsServerFolder(folderBrowserDialog.SelectedPath))
                    pathOK = true;
            } while (await PathIsNotServerFolder(pathOK));
            if (pathOK)
                ServerPath = folderBrowserDialog.SelectedPath;
        }

        private async Task<bool> PathIsNotServerFolder(bool pathOK) => !pathOK && await Dialogs.YesNoDialog(this, "invalid_server_location_caption", "invalid_server_location_text");
    }
}