using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    public class UpdateDialogViewModel(IApplicationManager applicationManager,
        IWindowsDialogs windowsDialogs,
        GitHubRelease release,
        object context,
        IDialogManager dialogManager) : ClosableDialogViewModel(context)
    {
        public RelayCommand DownloadRelease => new(async obj => await Download());
        public RelayCommand OpenReleaseUrl => new(obj => applicationManager.OpenUrl(Release.Url));
        public GitHubRelease Release { get; } = release;
        public GithubReleaseFile ReleaseFile => Release.Files?.First();

        public string FormatedDate => Release.PublishDate.ToString("dd.MM.yyyy");

        private async Task Download()
        {
            if (ReleaseFile != null)
            {
                var (success, path) = windowsDialogs.SaveFileDialog(ReleaseFile.Name);
                if (success)
                {
                    await CloseDialog();
                    await new FileDownloaderDialog(dialogManager).Download(ReleaseFile.Url, path);
                }
            }
        }
    }
}