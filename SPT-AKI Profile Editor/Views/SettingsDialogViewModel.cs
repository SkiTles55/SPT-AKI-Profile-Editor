using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor
{
    internal class SettingsDialogViewModel : BindableViewModel
    {
        private static readonly FolderBrowserDialog folderBrowserDialog = new()
        {
            Description = AppLocalization.GetLocalizedString("server_select"),
            RootFolder = Environment.SpecialFolder.MyComputer,
            ShowNewFolderButton = false
        };

        private static Visibility invalidServerLocationIcon = GetInvalidServerLocationIconVisibility();

        private static Visibility noAccountsIcon = GetNoAccountsIconVisibility();

        private static bool accountsBoxEnabled = GetAccountsBoxEnabled();

        private static Visibility closeButton = GetCloseButtonVisibility();

        private int selectedTab;

        public SettingsDialogViewModel(RelayCommand command, int index = 0)
        {
            CloseCommand = command;
            SelectedTab = index;
        }

        public static IEnumerable<AccentItem> ColorSchemes => ThemeManager.Current.Themes
            .OrderBy(x => x.DisplayName)
            .Select(x => new AccentItem
            {
                Color = x.PrimaryAccentColor.ToString(),
                Name = x.DisplayName,
                Scheme = x.Name
            });

        public static AppSettings AppSettings => AppData.AppSettings;

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
                InvalidServerLocationIcon = GetInvalidServerLocationIconVisibility();
                CloseButton = GetCloseButtonVisibility();
                NoAccountsIcon = GetNoAccountsIconVisibility();
                AccountsBoxEnabled = GetAccountsBoxEnabled();
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

        public Visibility InvalidServerLocationIcon
        {
            get => invalidServerLocationIcon;
            set
            {
                invalidServerLocationIcon = value;
                OnPropertyChanged("InvalidServerLocationIcon");
            }
        }

        public Visibility NoAccountsIcon
        {
            get => noAccountsIcon;
            set
            {
                noAccountsIcon = value;
                OnPropertyChanged("NoAccountsIcon");
            }
        }

        public bool AccountsBoxEnabled
        {
            get => accountsBoxEnabled;
            set
            {
                accountsBoxEnabled = value;
                OnPropertyChanged("AccountsBoxEnabled");
            }
        }

        public Visibility CloseButton
        {
            get => closeButton;
            set
            {
                closeButton = value;
                OnPropertyChanged("CloseButton");
            }
        }

        public RelayCommand ServerSelect => new(async obj => await ServerSelectDialog());

        private static void ReloadApplication()
        {
            System.Windows.Forms.Application.Restart();
            Environment.Exit(0);
        }

        private static Visibility GetNoAccountsIconVisibility() =>
            ExtMethods.ServerHaveProfiles(AppSettings) ? Visibility.Hidden : Visibility.Visible;

        private static bool GetAccountsBoxEnabled() =>
            ExtMethods.ServerHaveProfiles(AppSettings);

        private static Visibility GetInvalidServerLocationIconVisibility() =>
            ExtMethods.PathIsServerFolder(AppSettings) ? Visibility.Hidden : Visibility.Visible;

        private static Visibility GetCloseButtonVisibility() =>
            ExtMethods.ServerHaveProfiles(AppSettings) && ExtMethods.PathIsServerFolder(AppSettings) ? Visibility.Visible : Visibility.Collapsed;

        private async Task ServerSelectDialog()
        {
            bool pathOK = false;
            do
            {
                if (!string.IsNullOrWhiteSpace(ServerPath) && Directory.Exists(ServerPath))
                    folderBrowserDialog.SelectedPath = ServerPath;
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    pathOK = false;
                if (ExtMethods.PathIsServerFolder(AppSettings, folderBrowserDialog.SelectedPath))
                    pathOK = true;
            } while (await PathIsNotServerFolder(pathOK));
            if (pathOK)
                ServerPath = folderBrowserDialog.SelectedPath;
        }

        private async Task<bool> PathIsNotServerFolder(bool pathOK)
        {
            return !pathOK && await Dialogs.YesNoDialog(this, "invalid_server_location_caption", "invalid_server_location_text");
        }
    }
}