using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ServerPathEditorViewModel : ClosableDialogViewModel
    {
        private readonly RelayCommand retryCommand;

        public ServerPathEditorViewModel(IEnumerable<ServerPathEntry> paths,
                                         RelayCommand retryCommand)
        {
            Paths = paths;
            this.retryCommand = retryCommand;
        }

        public IEnumerable<ServerPathEntry> Paths { get; }
        public RelayCommand RetryCommand => new(obj => CloseAndRunRetryCommand());

        private async void CloseAndRunRetryCommand()
        {
            await CloseDialog();
            retryCommand.Execute(Paths);
        }
    }
}