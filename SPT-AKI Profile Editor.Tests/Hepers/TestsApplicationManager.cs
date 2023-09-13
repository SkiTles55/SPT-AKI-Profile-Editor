using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
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
        public bool CloseApplicationExecuted = false;
        public bool LocalizationsDeleted = false;
        public bool SettingsDeleted = false;
        public bool ApplicationRestarted = false;

        public RelayCommand CloseApplication => new(obj => CloseApplicationExecuted = true);

        public void ChangeTheme() => ThemeChanged = true;

        public bool CheckProcess(string name = null, string path = null) => ServerRunned;

        public Task<GitHubRelease> CheckUpdate()
        {
            if (HasUpdate)
                return Task.FromResult(new GitHubRelease());
            return Task.FromResult<GitHubRelease>(null);
        }

        public void CloseItemViewWindows(List<string> idsList = null) => ItemViewWindowsClosed = true;

        public void DeleteLocalizations() => LocalizationsDeleted = true;

        public void DeleteSettings() => SettingsDeleted = true;

        public string GetAppTitleWithVersion() => "TestTitle";

        public IEnumerable<AccentItem> GetColorSchemes()
        {
            return new List<AccentItem>() {
                new AccentItem("test1", "test1", "test1"),
                new AccentItem("test2", "test2", "test2")
            };
        }

        public void OpenContainerWindow(InventoryItem item, CharacterInventory inventory) => ContainerWindowOpened = true;

        public void OpenUrl(string url) => LastOpenedUrl = url;

        public void OpenWeaponBuildWindow(InventoryItem item, CharacterInventory inventory) => WeaponBuildWindowOpened = true;

        public void RestartApplication() => ApplicationRestarted = true;
    }
}