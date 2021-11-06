using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor
{
    public static class AppData
    {
        public static AppSettings AppSettings;
        public static AppLocalization AppLocalization;

        static AppData()
        {
            AppSettings = new AppSettings();
            AppSettings.Load();
            AppLocalization = new AppLocalization(AppSettings.Language);
        }
    }
}