using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ServerPathEditorViewModel(IEnumerable<ServerPathEntry> paths,
        RelayCommand retryCommand,
        RelayCommand faqCommand,
        object context) : ClosableDialogViewModel(context)
    {
        public IEnumerable<ServerPathEntry> Paths { get; } = paths;
        public RelayCommand RetryCommand => new(obj => CloseAndRunRetryCommand());
        public RelayCommand FAQCommand { get; } = faqCommand;

        private async void CloseAndRunRetryCommand()
        {
            await CloseDialog();
            retryCommand.Execute(Paths);
        }
    }
}