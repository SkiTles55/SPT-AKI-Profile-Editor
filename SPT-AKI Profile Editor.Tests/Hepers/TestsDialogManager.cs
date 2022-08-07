using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsDialogManager : IDialogManager
    {
        public bool LocalizationEditorDialogOpened = false;
        public bool SettingsDialogOpened = false;

        public Task ShowIssuesDialog(object context, RelayCommand saveCommand, IIssuesService issuesService)
        {
            throw new NotImplementedException();
        }

        public Task ShowLocalizationEditorDialog(object context, bool isEdit = true)
        {
            LocalizationEditorDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowOkMessageAsync(object context, string title, string message)
        {
            throw new NotImplementedException();
        }

        public Task ShowServerPathEditorDialog(object context, IEnumerable<ServerPathEntry> paths, RelayCommand retryCommand)
        {
            throw new NotImplementedException();
        }

        public Task ShowSettingsDialog(object context, int index = 0)
        {
            SettingsDialogOpened = true;
            return Task.CompletedTask;
        }

        public Task ShowUpdateDialog(object context, GitHubRelease release)
        {
            throw new NotImplementedException();
        }

        public Task ShutdownCozServerRunned(object context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> YesNoDialog(object context, string title, string caption) => Task.FromResult(true);
    }
}