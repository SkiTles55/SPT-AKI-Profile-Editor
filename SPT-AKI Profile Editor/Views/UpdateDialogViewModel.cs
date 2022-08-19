using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor
{
    public class UpdateDialogViewModel : ClosableDialogViewModel
    {
        private readonly IApplicationManager applicationManager;
        private readonly IWindowsDialogs _windowsDialogs;

        public UpdateDialogViewModel(IApplicationManager applicationManager, IWindowsDialogs windowsDialogs, GitHubRelease release)
        {
            this.applicationManager = applicationManager;
            _windowsDialogs = windowsDialogs;
            Release = release;
        }

        public RelayCommand DownloadRelease => new(async obj => await Download());
        public RelayCommand OpenReleaseUrl => new(obj => applicationManager.OpenUrl(Release.Url));
        public GitHubRelease Release { get; }
        public GithubReleaseFile ReleaseFile => Release.Files?.First();

        public string FormatedDate => Release.PublishDate.ToString("dd.MM.yyyy");

        private async Task Download()
        {
            if (ReleaseFile != null)
            {
                var (success, path) = _windowsDialogs.SaveFileDialog(ReleaseFile.Name);
                if (success)
                {
                    await CloseDialog();
                    await FileDownloader.Download(ReleaseFile.Url, path);
                }
            }
        }
    }
}