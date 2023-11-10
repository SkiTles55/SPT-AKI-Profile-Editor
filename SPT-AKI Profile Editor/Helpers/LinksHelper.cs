using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class LinksHelper
    {
        public static string FAQ
        {
            get
            {
                var baseUrl = $"https://github.com/{AppData.AppSettings.repoAuthor}/{AppData.AppSettings.repoName}/blob/master/";
                return AppData.AppSettings.Language switch
                {
                    "ru" => baseUrl + "FAQ.md",
                    "ch" => baseUrl + "CHFAQ.md",
                    _ => baseUrl + "ENGFAQ.md",
                };
            }
        }

        public static string HelperModManual
        {
            get
            {
                var baseUrl = AppData.AppSettings.modHelperHowToUseUrl;
                return AppData.AppSettings.Language switch
                {
                    "ru" => baseUrl + "Ru.md",
                    "ch" => baseUrl + "CH.md",
                    _ => baseUrl + "ENG.md",
                };
            }
        }
    }
}