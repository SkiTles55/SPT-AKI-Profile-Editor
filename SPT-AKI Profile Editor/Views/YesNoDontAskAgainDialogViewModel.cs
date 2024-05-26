using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class YesNoDontAskAgainDialogViewModel : ClosableDialogViewModel
    {
        private string questionTag;
        private AppSettings appSettings;

        public YesNoDontAskAgainDialogViewModel(string yesText,
                                                string noText,
                                                string message,
                                                RelayCommand yesCommand,
                                                string questionTag,
                                                AppSettings appSettings,
                                                object context) : base(context)
        {
            YesText = yesText;
            NoText = noText;
            Message = message;
            YesCommand = yesCommand;
            this.questionTag = questionTag;
            this.appSettings = appSettings;
        }

        public string YesText { get; }
        public string NoText { get; }
        public string Message { get; }
        public RelayCommand YesCommand { get; }
        public bool DontAskAgain { get; set; } = false;
    }
}