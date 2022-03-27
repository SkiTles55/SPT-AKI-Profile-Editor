using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;

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
        public GitHubRelease Release { get; set; }
        public GithubReleaseFile ReleaseFile => Release.Files?.First();

        public string FormatedDate => Release.PublishDate.ToString("dd.MM.yyyy");
    }
}