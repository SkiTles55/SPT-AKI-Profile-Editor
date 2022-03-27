using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Helpers;

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
    }
}