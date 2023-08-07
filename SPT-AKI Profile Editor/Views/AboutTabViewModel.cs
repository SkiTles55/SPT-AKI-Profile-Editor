using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.Windows;

namespace SPT_AKI_Profile_Editor.Views
{
    public class AboutTabViewModel : BindableViewModel
    {
        private readonly IApplicationManager _applicationManager;

        public AboutTabViewModel(IApplicationManager applicationManager) => _applicationManager = applicationManager;

        public static AppSettings AppSettings => AppData.AppSettings;

        public static string RepositoryURL => $"https://github.com/{AppSettings.repoAuthor}/{AppSettings.repoName}/releases/latest";

        public static string AuthorURL => $"https://github.com/{AppSettings.repoAuthor}";

        public static string YoomoneyUrl => AppSettings.yoomoneyUrl;

        public static string LtcWallet => AppSettings.ltcWallet;

        public static string SptAkiProjectUrl => AppSettings.sptAkiProjectUrl;

        public static RelayCommand CopyLtcWallet => new(obj => Clipboard.SetText(LtcWallet));

        public RelayCommand OpenAutorGitHubUrl => new(obj => OpenUrl(AuthorURL));

        public RelayCommand OpenRepositoryGitHubUrl => new(obj => OpenUrl(RepositoryURL));

        public RelayCommand OpenYoomoneyUrl => new(obj => OpenUrl(YoomoneyUrl));

        public RelayCommand OpenSteamUrl => new(obj => OpenUrl(AppSettings.steamTradeUrl));

        public RelayCommand OpenSptAkiProjectUrl => new(obj => OpenUrl(SptAkiProjectUrl));

        private void OpenUrl(string url) => _applicationManager.OpenUrl(url);
    }
}