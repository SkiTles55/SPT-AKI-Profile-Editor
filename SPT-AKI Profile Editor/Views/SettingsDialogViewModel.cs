using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    class SettingsDialogViewModel
    {
        public SettingsDialogViewModel(RelayCommand command) => CloseCommand = command;
        public static string CurrentLocalization
        {
            get { return App.appSettings.Language; }
            set
            {
                App.appSettings.Language = value;
                App.appSettings.Save();
                App.appLocalization.LoadLocalization(App.appSettings.Language);
            }
        }
        public static AppLocalization AppLocalization => App.appLocalization;
        public static List<string> Profiles { get; set; }
        public static Dictionary<string, string> LocalizationsList => AppLocalization.Localizations;
        public static RelayCommand CloseCommand { get; set; }
        public static Visibility InvalidServerLocationIcon => ExtMethods.PathIsServerBase(App.appSettings) ? Visibility.Collapsed : Visibility.Visible;
        public static Visibility NoAccontsIcon => ExtMethods.ServerHaveProfiles(App.appSettings) ? Visibility.Collapsed : Visibility.Visible;
        public static RelayCommand QuitCommand => App.CloseApplication;
    }
}
