using ControlzEx.Theming;
using ReleaseChecker.GitHub;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IApplicationManager
    {
        public RelayCommand CloseApplication { get; }

        public void OpenContainerWindow(object obj, StashEditMode editMode);

        public void OpenWeaponBuildWindow(object obj, StashEditMode editMode);

        public void CloseItemViewWindows(List<string> idsList = null);

        public void ChangeTheme();

        public void OpenUrl(string url);

        public bool CheckProcess(string name = null, string path = null);

        public Task<GitHubRelease> CheckUpdate();

        public string GetAppTitleWithVersion();
    }

    public class ApplicationManager : IApplicationManager
    {
        private static readonly GitHubChecker gitHubChecker = new(AppData.AppSettings.repoAuthor, AppData.AppSettings.repoName);

        public RelayCommand CloseApplication => new(obj => Application.Current.Shutdown());

        public void OpenContainerWindow(object obj, StashEditMode editMode)
        {
            if (obj == null || obj is not InventoryItem item)
                return;
            if (CheckForOpenedWindow(item.Id))
                return;
            ContainerWindow containerWindow = new(item, editMode);
            containerWindow.Show();
        }

        public void OpenWeaponBuildWindow(object obj, StashEditMode editMode)
        {
            if (obj == null || obj is not InventoryItem item)
                return;
            if (CheckForOpenedWindow(item.Id))
                return;
            WeaponBuildWindow weaponBuildWindow = new(item, editMode);
            weaponBuildWindow.Show();
        }

        public void CloseItemViewWindows(List<string> idsList = null)
        {
            // Skipping in nUnit tests
            if (Application.Current == null)
                return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                    if (window is ItemViewWindow containerWindow && (idsList == null || idsList.Contains(containerWindow.ItemId)))
                        containerWindow.Close();
            });
        }

        public void ChangeTheme() => ThemeManager.Current.ChangeTheme(Application.Current, AppData.AppSettings.ColorScheme);

        public void OpenUrl(string url)
        {
            ProcessStartInfo link = new(url)
            {
                UseShellExecute = true
            };
            Process.Start(link);
        }

        public bool CheckProcess(string name = null, string path = null)
        {
            if (string.IsNullOrEmpty(name))
                name = Path.GetFileNameWithoutExtension(AppData.AppSettings.FilesList[SPTServerFile.serverexe]);
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.FilesList[SPTServerFile.serverexe]);
            Process[] processesArray = Process.GetProcessesByName(name);
            return processesArray.Where(x => x.MainModule.FileName.ToLower() == path.ToLower()).Any();
        }

        public async Task<GitHubRelease> CheckUpdate()
        {
            try
            {
                var currentVersion = GetVersion();
                var latestRelease = await gitHubChecker.GetLatestReleaseAsync(true);
                if (latestRelease != null && new Version(latestRelease.Tag) > currentVersion)
                    return latestRelease;
                Logger.Log($"No updates available");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Log($"UpdatesChecker error : {ex.Message}");
                return null;
            }
        }

        public string GetAppTitleWithVersion()
        {
            Version version = GetVersion();
            return $"SPT-AKI Profile Editor {$" {version.Major}.{version.Minor}"}" + (version.Build != 0 ? "." + version.Build.ToString() : "");
        }

        private static bool CheckForOpenedWindow(string itemId)
        {
            foreach (Window window in Application.Current.Windows)
                if (window is ItemViewWindow openedWindow && openedWindow.ItemId == itemId)
                    return openedWindow.Activate();
            return false;
        }

        private static Version GetVersion() => Assembly.GetExecutingAssembly().GetName().Version;
    }
}