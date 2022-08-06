using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class AboutTabViewModel : BindableViewModel
    {
        public static AppSettings AppSettings => AppData.AppSettings;

        public static string RepositoryURL => $"https://github.com/{AppSettings.repoAuthor}/{AppSettings.repoName}/releases/latest";

        public static string AuthorURL => $"https://github.com/{AppSettings.repoAuthor}";
    }
}