using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ServerPathEditorViewModel : ClosableDialogViewModel
    {
        public ServerPathEditorViewModel(IEnumerable<ServerPathEntry> paths,
                                         RelayCommand retryCommand)
        {
            Paths = paths;
            RetryCommand = retryCommand;
        }

        public IEnumerable<ServerPathEntry> Paths { get; }
        public RelayCommand RetryCommand { get; }
    }
}