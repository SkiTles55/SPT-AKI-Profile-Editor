using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor
{
    public class UpdateDialogViewModel : ClosableDialogViewModel
    {
        private readonly IApplicationManager applicationManager;

        public UpdateDialogViewModel(IApplicationManager applicationManager, GitHubRelease release)
        {
            this.applicationManager = applicationManager;
            Release = release;
        }

        public RelayCommand DownloadRelease => new(async obj => await Download());
        public RelayCommand OpenReleaseUrl => new(obj => applicationManager.OpenUrl(Release.Url));
        public GitHubRelease Release { get; }
        public GithubReleaseFile ReleaseFile => Release.Files?.First();

        public string FormatedDate => Release.PublishDate.ToString("dd.MM.yyyy");

        private async Task Download()
        {
            if (ReleaseFile == null)
                return;
            var saveFileDialog = WindowsDialogs.SaveFileDialog(ReleaseFile.Name);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                await CloseDialog();
                await FileDownloader.Download(ReleaseFile.Url, saveFileDialog.FileName);
            }
        }
    }
}