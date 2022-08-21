using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsDialogManager : IDialogManager
    {
        public bool LocalizationEditorDialogOpened = false;
        public bool SettingsDialogOpened = false;
        public bool UpdateDialogOpened = false;
        public bool ShutdownCozServerRunnedOpened = false;
        public bool IssuesDialogOpened = false;
        public bool ServerPathEditorDialogOpened = false;
        public bool ShouldExecuteServerPathEditorRetryCommand = false;
        public string LastOkMessage = null;

        public Task ShowAddMoneyDialog(object context, AddableItem money, RelayCommand addCommand)
        {
            addCommand.Execute(null);
            return Task.CompletedTask;
        }

        public Task ShowIssuesDialog(object context, RelayCommand saveCommand, IIssuesService issuesService)
        {
            IssuesDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowLocalizationEditorDialog(object context, bool isEdit = true)
        {
            LocalizationEditorDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowOkMessageAsync(object context, string title, string message)
        {
            LastOkMessage = message;
            return Task.CompletedTask;
        }

        public Task ShowServerPathEditorDialog(object context, IEnumerable<ServerPathEntry> paths, RelayCommand retryCommand)
        {
            ServerPathEditorDialogOpened = true;
            if (ShouldExecuteServerPathEditorRetryCommand)
            {
                ShouldExecuteServerPathEditorRetryCommand = false;
                foreach (var path in paths)
                    if (path.Key == SPTServerFile.serverexe)
                        path.Path = "Test.exe";
                retryCommand.Execute(paths);
            }
            return Task.CompletedTask;
        }

        public Task ShowSettingsDialog(object context, int index = 0)
        {
            SettingsDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowUpdateDialog(object context, GitHubRelease release)
        {
            UpdateDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShutdownCozServerRunned(object context)
        {
            ShutdownCozServerRunnedOpened = true;
            return Task.CompletedTask;
        }

        public Task<bool> YesNoDialog(object context, string title, string caption) => Task.FromResult(true);
    }
}