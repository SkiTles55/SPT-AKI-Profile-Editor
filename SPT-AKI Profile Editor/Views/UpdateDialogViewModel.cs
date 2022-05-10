using MahApps.Metro.Controls.Dialogs;
using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor
{
    internal class UpdateDialogViewModel : BindableViewModel
    {
        public UpdateDialogViewModel(RelayCommand command, GitHubRelease release)
        {
            CloseCommand = command;
            Release = release;
        }

        public static RelayCommand CloseCommand { get; set; }

        public RelayCommand DownloadRelease => new(async obj => await Download());
        public RelayCommand OpenReleaseUrl => new(obj => ExtMethods.OpenUrl(Release.Url));
        public GitHubRelease Release { get; set; }
        public GithubReleaseFile ReleaseFile => Release.Files?.First();

        public string FormatedDate => Release.PublishDate.ToString("dd.MM.yyyy");

        private async Task Download()
        {
            if (ReleaseFile == null)
                return;
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.FileName = ReleaseFile.Name;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                BaseMetroDialog dialog = await App.DialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(MainWindowViewModel.Instance);
                await App.DialogCoordinator.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
                await FileDownloader.Download(ReleaseFile.Url, saveFileDialog.FileName);
            }
        }
    }
}