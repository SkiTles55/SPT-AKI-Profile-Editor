using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ServerPathEditorViewModel : ClosableDialogViewModel
    {
        public ServerPathEditorViewModel(IEnumerable<(string key, string path, bool result)> paths,
                                         RelayCommand retryCommand)
        {
            Paths = paths.Select(x => new ServerPathEntry(x));
            RetryCommand = retryCommand;
        }

        public IEnumerable<ServerPathEntry> Paths { get; }
        public RelayCommand RetryCommand { get; }

        public class ServerPathEntry
        {
            public ServerPathEntry((string key, string path, bool result) resultEntry)
            {
                Key = resultEntry.key;
                Path = resultEntry.path;
                IsFounded = resultEntry.result;
            }

            private string Key { get; }
            private string Path { get; set; }
            private bool IsFounded { get; }
        }
    }
}