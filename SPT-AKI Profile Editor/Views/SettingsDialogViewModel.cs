using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor
{
    class SettingsDialogViewModel : INotifyPropertyChanged
    {
        public SettingsDialogViewModel(IDialogCoordinator instance, RelayCommand command)
        {
            dialogCoordinator = instance;
            CloseCommand = command;
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

        public static IEnumerable<AccentItem> ColorSchemes => ThemeManager.Current.Themes
            .OrderBy(x => x.DisplayName)
            .Select(x => new AccentItem
            {
                Color = x.PrimaryAccentColor.ToString(),
                Name = x.DisplayName,
                Scheme = x.Name
            });
        public static AppLocalization AppLocalization => App.appLocalization;
        public static AppSettings AppSettings => App.appSettings;
        public static Dictionary<string, string> LocalizationsList => AppLocalization.Localizations;
        public static RelayCommand CloseCommand { get; set; }
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
        public static RelayCommand QuitCommand => App.CloseApplication;
        public RelayCommand ServerSelect => new(async obj =>
        {
            await ServerSelectDialog();
        });

        private static Visibility invalidServerLocationIcon = GetInvalidServerLocationIconVisibility();
        private static Visibility noAccountsIcon = GetNoAccountsIconVisibility();
        private static bool accountsBoxEnabled = GetAccountsBoxEnabled();
        private static Visibility closeButton = GetCloseButtonVisibility();
        private readonly IDialogCoordinator dialogCoordinator;
        private static readonly FolderBrowserDialog folderBrowserDialog = new()
        {
            Description = AppLocalization.Translations["server_select"],
            RootFolder = Environment.SpecialFolder.MyComputer,
            ShowNewFolderButton = false
        };
        private static MetroDialogSettings metroDialogSettings => new()
        {
            DefaultButtonFocus = MessageDialogResult.Affirmative,
            AffirmativeButtonText = AppLocalization.Translations["button_yes"],
            NegativeButtonText = AppLocalization.Translations["button_no"],
            AnimateHide = true,
            AnimateShow = true
        };

        private static Visibility GetNoAccountsIconVisibility() => ExtMethods.ServerHaveProfiles(AppSettings) ? Visibility.Hidden : Visibility.Visible;
        private static bool GetAccountsBoxEnabled() => ExtMethods.ServerHaveProfiles(AppSettings);
        private static Visibility GetInvalidServerLocationIconVisibility() => ExtMethods.PathIsServerFolder(AppSettings) ? Visibility.Hidden : Visibility.Visible;
        private static Visibility GetCloseButtonVisibility() => ExtMethods.ServerHaveProfiles(AppSettings) && ExtMethods.PathIsServerFolder(AppSettings) ? Visibility.Visible : Visibility.Hidden;
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
            return !pathOK && await dialogCoordinator.ShowMessageAsync(this,
                AppLocalization.Translations["invalid_server_location_caption"],
                AppLocalization.Translations["invalid_server_location_text"],
                MessageDialogStyle.AffirmativeAndNegative,
                metroDialogSettings) == MessageDialogResult.Affirmative;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
