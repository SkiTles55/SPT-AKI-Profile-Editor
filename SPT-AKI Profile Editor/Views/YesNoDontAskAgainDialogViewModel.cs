using SPT_AKI_Profile_Editor.Helpers;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    public class YesNoDontAskAgainDialogResult
    {
        public YesNoDontAskAgainDialogResult(bool affirmative, bool dontAskAgain)
        {
            Affirmative = affirmative;
            DontAskAgain = dontAskAgain;
        }

        public bool Affirmative { get; }
        public bool DontAskAgain { get; }
    }

    public class YesNoDontAskAgainDialogViewModel : ClosableDialogViewModel
    {
        private readonly TaskCompletionSource<YesNoDontAskAgainDialogResult> dialogResult = new();

        public YesNoDontAskAgainDialogViewModel(string yesText,
                                                string noText,
                                                string message,
                                                bool dontAskAgain,
                                                object context) : base(context)
        {
            YesText = yesText;
            NoText = noText;
            Message = message;
            DontAskAgain = dontAskAgain;
        }

        public Task<YesNoDontAskAgainDialogResult> DialogResult => dialogResult.Task;
        public string YesText { get; }
        public string NoText { get; }
        public string Message { get; }
        public bool DontAskAgain { get; set; }

        public RelayCommand YesCommand => new(async _ => await CloseDialog(true));

        public RelayCommand NoCommand => new(async _ => await CloseDialog(false));

        private async Task CloseDialog(bool result)
        {
            await CloseDialog();
            dialogResult.SetResult(new YesNoDontAskAgainDialogResult(result, DontAskAgain));
        }
    }
}