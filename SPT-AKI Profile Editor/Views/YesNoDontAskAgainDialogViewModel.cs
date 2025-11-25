using SPT_AKI_Profile_Editor.Helpers;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    public class YesNoDontAskAgainDialogResult(bool affirmative, bool dontAskAgain)
    {
        public bool Affirmative { get; } = affirmative;
        public bool DontAskAgain { get; } = dontAskAgain;
    }

    public class YesNoDontAskAgainDialogViewModel(string yesText,
        string noText,
        string message,
        bool dontAskAgain,
        object context) : ClosableDialogViewModel(context)
    {
        private readonly TaskCompletionSource<YesNoDontAskAgainDialogResult> dialogResult = new();

        public Task<YesNoDontAskAgainDialogResult> DialogResult => dialogResult.Task;
        public string YesText { get; } = yesText;
        public string NoText { get; } = noText;
        public string Message { get; } = message;
        public bool DontAskAgain { get; set; } = dontAskAgain;

        public RelayCommand YesCommand => new(async _ => await CloseDialog(true));

        public RelayCommand NoCommand => new(async _ => await CloseDialog(false));

        private async Task CloseDialog(bool result)
        {
            await CloseDialog();
            dialogResult.SetResult(new YesNoDontAskAgainDialogResult(result, DontAskAgain));
        }
    }
}