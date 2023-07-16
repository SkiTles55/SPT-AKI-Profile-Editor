using MahApps.Metro.Controls.Dialogs;
using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Views;
using System;
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
        public double LastProgress = 0;
        public bool YesNoDialogResult = true;

        public event EventHandler ProgressDialogCanceled;

        public Task HideProgressDialog()
        {
            return Task.CompletedTask;
        }

        public Task ShowAddMoneyDialog(AddableItem money, RelayCommand addCommand)
        {
            addCommand.Execute(null);
            return Task.CompletedTask;
        }

        public Task ShowIssuesDialog(RelayCommand saveCommand, IIssuesService issuesService)
        {
            IssuesDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowLocalizationEditorDialog(SettingsDialogViewModel settingsDialog, bool isEdit = true)
        {
            LocalizationEditorDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowOkMessageAsync(string title, string message)
        {
            LastOkMessage = message;
            return Task.CompletedTask;
        }

        public Task ShowProgressDialog(string title, string description, bool indeterminate = true, double progress = 0, bool cancelable = false, MetroDialogSettings dialogSettings = null)
        {
            LastProgress = progress;
            return Task.CompletedTask;
        }

        public Task ShowServerPathEditorDialog(IEnumerable<ServerPathEntry> paths, RelayCommand retryCommand, RelayCommand faqCommand)
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

        public Task ShowSettingsDialog(RelayCommand reloadCommand, RelayCommand faqCommand, int index = 0)
        {
            SettingsDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowUpdateDialog(GitHubRelease release)
        {
            UpdateDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShutdownCozServerRunned()
        {
            ShutdownCozServerRunnedOpened = true;
            return Task.CompletedTask;
        }

        public Task<bool> YesNoDialog(string title, string caption) => Task.FromResult(YesNoDialogResult);
    }
}