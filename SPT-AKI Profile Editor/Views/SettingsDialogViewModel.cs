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
            get => App.appSettings.Language;
            set
            {
                App.appSettings.Language = value;
                OnPropertyChanged("CurrentLocalization");
                App.appSettings.Save();
                App.appLocalization.LoadLocalization(App.appSettings.Language);
            }
        }
        public string ServerPath
        {
            get => App.appSettings.ServerPath;
            set
            {
                App.appSettings.ServerPath = value;
                OnPropertyChanged("ServerPath");
                InvalidServerLocationIcon = GetInvalidServerLocationIconVisibility();
                NoAccontsIcon = GetNoAccontsIconVisibility();
                CloseButton = GetCloseButtonVisibility();
                App.appSettings.NeedReload = true;
                App.appSettings.Save();
            }
        }
        public static List<string> Profiles { get; set; }
        public static string DefaultProfile { get; set; }
        public string ColorScheme
        {
            get => App.appSettings.ColorScheme;
            set
            {
                App.appSettings.ColorScheme = value;
                OnPropertyChanged("ColorScheme");
                App.appSettings.Save();
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
        public Visibility NoAccontsIcon
        {
            get => noAccontsIcon;
            set
            {
                noAccontsIcon = value;
                OnPropertyChanged("NoAccontsIcon");
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
        private static Visibility noAccontsIcon = GetNoAccontsIconVisibility();
        private static Visibility closeButton = GetCloseButtonVisibility();
        private IDialogCoordinator dialogCoordinator;
        private static FolderBrowserDialog folderBrowserDialog = new()
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

        private static Visibility GetNoAccontsIconVisibility() => ExtMethods.ServerHaveProfiles(App.appSettings) ? Visibility.Collapsed : Visibility.Visible;
        private static Visibility GetInvalidServerLocationIconVisibility() => ExtMethods.PathIsServerFolder(App.appSettings) ? Visibility.Collapsed : Visibility.Visible;
        private static Visibility GetCloseButtonVisibility() => ExtMethods.ServerHaveProfiles(App.appSettings) && ExtMethods.PathIsServerFolder(App.appSettings) ? Visibility.Visible : Visibility.Collapsed;
        private async Task ServerSelectDialog()
        {
            bool pathOK = false;
            do
            {
                if (!string.IsNullOrWhiteSpace(ServerPath) && Directory.Exists(ServerPath))
                    folderBrowserDialog.SelectedPath = ServerPath;
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    pathOK = false;
                if (ExtMethods.PathIsServerFolder(App.appSettings, folderBrowserDialog.SelectedPath))
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
