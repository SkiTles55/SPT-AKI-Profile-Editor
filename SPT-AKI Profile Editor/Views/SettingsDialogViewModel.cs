using ControlzEx.Theming;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    class SettingsDialogViewModel
    {
        public static string CurrentLocalization
        {
            get => App.appSettings.Language;
            set
            {
                App.appSettings.Language = value;
                App.appSettings.Save();
                App.appLocalization.LoadLocalization(App.appSettings.Language);
            }
        }
        public static string ServerPath { get; set; }
        public static List<string> Profiles { get; set; }
        public static string DefaultProfile { get; set; }
        public static string ColorScheme
        {
            get => App.appSettings.ColorScheme;
            set
            {
                App.appSettings.ColorScheme = value;
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
        public SettingsDialogViewModel(RelayCommand command) => CloseCommand = command;
        public static AppLocalization AppLocalization => App.appLocalization;
        public static Dictionary<string, string> LocalizationsList => AppLocalization.Localizations;
        public static RelayCommand CloseCommand { get; set; }
        public static Visibility InvalidServerLocationIcon => ExtMethods.PathIsServerBase(App.appSettings) ? Visibility.Collapsed : Visibility.Visible;
        public static Visibility NoAccontsIcon => ExtMethods.ServerHaveProfiles(App.appSettings) ? Visibility.Collapsed : Visibility.Visible;
        public static RelayCommand QuitCommand => App.CloseApplication;
    }
}
