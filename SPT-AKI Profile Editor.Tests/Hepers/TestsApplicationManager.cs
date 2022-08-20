using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsApplicationManager : IApplicationManager
    {
        public bool WeaponBuildWindowOpened = false;
        public bool ContainerWindowOpened = false;
        public bool ThemeChanged = false;
        public bool ItemViewWindowsClosed = false;
        public string LastOpenedUrl = null;
        public bool ServerRunned = false;
        public bool HasUpdate = false;

        public RelayCommand CloseApplication => throw new NotImplementedException();

        public void ChangeTheme() => ThemeChanged = true;

        public bool CheckProcess(string name = null, string path = null) => ServerRunned;

        public Task<GitHubRelease> CheckUpdate()
        {
            if (HasUpdate)
                return Task.FromResult(new GitHubRelease());
            return Task.FromResult<GitHubRelease>(null);
        }

        public void CloseItemViewWindows(List<string> idsList = null) => ItemViewWindowsClosed = true;

        public string GetAppTitleWithVersion() => "TestTitle";

        public void OpenContainerWindow(object obj, StashEditMode editMode) => ContainerWindowOpened = true;

        public void OpenUrl(string url) => LastOpenedUrl = url;

        public void OpenWeaponBuildWindow(object obj, StashEditMode editMode) => WeaponBuildWindowOpened = true;
    }
}