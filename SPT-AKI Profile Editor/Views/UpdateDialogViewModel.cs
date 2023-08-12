using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    public class UpdateDialogViewModel : ClosableDialogViewModel
    {
        private readonly IApplicationManager applicationManager;
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IDialogManager _dialogManager;

        public UpdateDialogViewModel(IApplicationManager applicationManager,
                                     IWindowsDialogs windowsDialogs,
                                     GitHubRelease release,
                                     object context,
                                     IDialogManager dialogManager) : base(context)
        {
            this.applicationManager = applicationManager;
            _windowsDialogs = windowsDialogs;
            Release = release;
            _dialogManager = dialogManager;
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
                    await new FileDownloaderDialog(_dialogManager).Download(ReleaseFile.Url, path);
                }
            }
        }
    }
}