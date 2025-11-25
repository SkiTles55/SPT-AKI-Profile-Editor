using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IHelperModManager
    {
        public string DbPath { get; }
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
        private readonly string helperDbPath;
        private readonly string modPath;
        private readonly Uri updateUrl;
        private readonly string updateSaveDirectory;
        private readonly string updatedVersionPath;
        private readonly string updatedModPackagePath;

        private readonly string versionFileName = "version";
        private readonly string modSourceDirName = "ModHelper";
        private readonly string modPackageFileName = "SPT-AKI Profile Editor.ModHelper.dll";

        private Version AvailableVersion = new();

        public HelperModManager(string updateUrl, string updateSaveDirectory, string modPath = "SPT\\user\\mods\\SPT-AKI Profile Editor.ModHelper")
        {
            this.updateUrl = new(updateUrl);
            this.updateSaveDirectory = updateSaveDirectory;
            this.modPath = modPath;
            helperDbPath = Path.Combine(modPath, "exportedDB");
            updatedVersionPath = Path.Combine(updateSaveDirectory, versionFileName);
            updatedModPackagePath = Path.Combine(updateSaveDirectory, modPackageFileName);
        }

        public HelperModStatus HelperModStatus => CheckModStatus();
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus != HelperModStatus.NotInstalled;
        public bool DbFilesExist => CheckDbStatus();
        public string DbPath => helperDbPath;
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

        private string GetFullModPath()
            => string.IsNullOrEmpty(AppData.AppSettings.ServerPath)
            ? null
            : Path.Combine(AppData.AppSettings.ServerPath, modPath);

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
            if (string.IsNullOrEmpty(AppData.AppSettings.ServerPath))
                return false;
            var fullDbPath = Path.Combine(AppData.AppSettings.ServerPath, helperDbPath);
            if (Directory.Exists(fullDbPath)
                && Directory.GetFiles(fullDbPath, "*.json").Length != 0
                && Directory.GetDirectories(fullDbPath).Length != 0)
                return true;
            return false;
        }

        private (string packagePath, string srcPath) GetFilesPaths()
        {
            if (HaveUpdatedFiles)
                return (updatedVersionPath, updatedModPackagePath);
            var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                           modSourceDirName,
                                           versionFileName);
            var srcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                       modSourceDirName,
                                       modPackageFileName);
            return (packagePath, srcPath);
        }
    }
}