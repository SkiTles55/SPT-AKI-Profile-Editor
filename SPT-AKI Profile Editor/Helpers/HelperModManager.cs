using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IHelperModManager
    {
        public string FullDbPath { get; }
        public HelperModStatus HelperModStatus { get; }
        public bool UpdateAvailable { get; }
        public bool IsInstalled { get; }
        public bool DbFilesExist { get; }

        public void InstallMod();

        public void RemoveMod();

        public void UpdateMod();

        public Task DownloadUpdates();
    }

    public class HelperModManager : IHelperModManager
    {
        // Returns the helper mod folder either relative to the server root or as an
        // absolute path. Path.Combine ignores the server root in the absolute case,
        // which is what lets tests point the manager at a directory of their own.
        private readonly Func<string> modPathResolver;
        private readonly Uri updateUrl;
        private readonly string updateSaveDirectory;
        private readonly string updatedVersionPath;
        private readonly string updatedModPackagePath;

        private readonly string versionFileName = "version";
        private readonly string modSourceDirName = "ModHelper";
        private readonly string modPackageFileName = "SPT-AKI Profile Editor.ModHelper.dll";

        private Version AvailableVersion = new();

        public HelperModManager(string updateUrl, string updateSaveDirectory, Func<string> modPathResolver = null)
        {
            this.updateUrl = new(updateUrl);
            this.updateSaveDirectory = updateSaveDirectory;
            this.modPathResolver = modPathResolver ?? DefaultModPathResolver;
            updatedVersionPath = Path.Combine(updateSaveDirectory, versionFileName);
            updatedModPackagePath = Path.Combine(updateSaveDirectory, modPackageFileName);
        }

        public HelperModStatus HelperModStatus => CheckModStatus();
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus != HelperModStatus.NotInstalled;
        public bool DbFilesExist => CheckDbStatus();
        public string FullDbPath => GetFullDbPath();
        private bool HaveUpdatedFiles => File.Exists(updatedVersionPath) && File.Exists(updatedModPackagePath);

        public void InstallMod()
        {
            var fullModPath = GetFullModPath();
            if (string.IsNullOrEmpty(fullModPath))
                return;
            if (!Directory.Exists(fullModPath))
                Directory.CreateDirectory(fullModPath);
            var (packagePath, srcPath) = GetFilesPaths();
            File.Copy(srcPath, Path.Combine(fullModPath, modPackageFileName), true);
            File.Copy(packagePath, Path.Combine(fullModPath, versionFileName), true);
        }

        public void RemoveMod()
        {
            var fullModPath = GetFullModPath();
            if (string.IsNullOrEmpty(fullModPath) || !Directory.Exists(fullModPath))
                return;
            Directory.Delete(fullModPath, true);
        }

        public void UpdateMod()
        {
            RemoveMod();
            InstallMod();
        }

        public async Task DownloadUpdates()
        {
            if (!Directory.Exists(updateSaveDirectory))
                Directory.CreateDirectory(updateSaveDirectory);
            if (File.Exists(updatedVersionPath))
                AvailableVersion = GetModVersion(updatedVersionPath);
            FileDownloader fileDownloader = new();
            try
            {
                var packageUrl = new Uri(updateUrl, versionFileName);
                await fileDownloader.DownloadFromUrl(packageUrl.AbsoluteUri, updatedVersionPath);
                var latestVersion = GetModVersion(updatedVersionPath);
                if (latestVersion > AvailableVersion)
                {
                    await fileDownloader.DownloadFromUrl(new Uri(updateUrl, modPackageFileName).AbsoluteUri,
                                                         updatedModPackagePath);

                    AvailableVersion = latestVersion;
                }
            }
            catch { }
        }

        private static Version GetModVersion(string filename)
        {
            try { return Version.Parse(File.ReadAllText(filename)); }
            catch { return null; }
        }

        private static string DefaultModPathResolver()
        {
            var settings = AppData.AppSettings;
            string modsPath = settings.DirsList.TryGetValue(SPTServerDir.mods, out var configuredPath)
                              && !string.IsNullOrEmpty(configuredPath)
                                  ? configuredPath
                                  : DefaultValues.GetDefaultDirsList(settings.ServerDirectory)[SPTServerDir.mods];
            return Path.Combine(modsPath, DefaultValues.ModHelperDirectoryName);
        }

        // The single place that may return null: every caller checks the result.
        private string GetFullModPath()
        {
            if (string.IsNullOrEmpty(AppData.AppSettings.ServerPath))
                return null;
            var modPath = modPathResolver();
            return string.IsNullOrEmpty(modPath) ? null : Path.Combine(AppData.AppSettings.ServerPath, modPath);
        }

        private string GetFullDbPath()
            => GetFullModPath() is { } modPath ? Path.Combine(modPath, DefaultValues.ModHelperDbDirectoryName) : null;

        private HelperModStatus CheckModStatus()
        {
            var fullModPath = GetFullModPath();
            if (string.IsNullOrEmpty(fullModPath) || !File.Exists(Path.Combine(fullModPath, modPackageFileName)))
                return HelperModStatus.NotInstalled;

            var installedModPackageJson = Path.Combine(fullModPath, versionFileName);
            if (!File.Exists(installedModPackageJson))
                return HelperModStatus.NotInstalled;

            if (AvailableVersion != null
                && AvailableVersion > GetModVersion(installedModPackageJson)
                && HaveUpdatedFiles)
                return HelperModStatus.UpdateAvailable;

            return HelperModStatus.Installed;
        }

        private bool CheckDbStatus()
        {
            var fullDbPath = GetFullDbPath();
            if (fullDbPath != null
                && Directory.Exists(fullDbPath)
                && Directory.GetFiles(fullDbPath, "*.json").Length != 0
                && Directory.GetDirectories(fullDbPath).Length != 0)
                return true;
            return false;
        }

        private (string packagePath, string srcPath) GetFilesPaths()
        {
            var bundledPackagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                  modSourceDirName,
                                                  versionFileName);
            var bundledModPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                              modSourceDirName,
                                              modPackageFileName);

            if (!HaveUpdatedFiles)
                return (bundledPackagePath, bundledModPath);

            var bundledVersion = GetModVersion(bundledPackagePath);
            var updatedVersion = GetModVersion(updatedVersionPath);

            // An update cache can survive an application upgrade. Do not let an
            // older cached helper replace the helper bundled with this build.
            if (bundledVersion == null || updatedVersion == null || updatedVersion > bundledVersion)
                return (updatedVersionPath, updatedModPackagePath);

            return (bundledPackagePath, bundledModPath);
        }
    }
}