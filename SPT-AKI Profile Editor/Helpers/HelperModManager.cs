using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.ModHelper;
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
        private readonly string updatedPackageJsonPath;
        private readonly string updatedModScriptSourcePath;

        private readonly string srcDirName = "src";
        private readonly string modScriptFileName = "mod.ts";
        private readonly string packageJsonFileName = "package.json";
        private readonly string modSourceDirName = "ModHelper";
        private readonly string modScriptSourceFileName = "mod.ts-source";

        public HelperModManager(string updateUrl, string updateSaveDirectory, string modPath = "user\\mods\\ProfileEditorHelper")
        {
            this.updateUrl = new(updateUrl);
            this.updateSaveDirectory = updateSaveDirectory;
            this.modPath = modPath;
            helperDbPath = Path.Combine(modPath, "exportedDB");
            updatedPackageJsonPath = Path.Combine(updateSaveDirectory, packageJsonFileName);
            updatedModScriptSourcePath = Path.Combine(updateSaveDirectory, modScriptSourceFileName);
        }

        public HelperModStatus HelperModStatus => CheckModStatus();
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus != HelperModStatus.NotInstalled;
        public bool DbFilesExist => CheckDbStatus();
        public string DbPath => helperDbPath;
        private Version AvailableVersion { get; set; } = new();

        private bool HaveUpdatedFiles => File.Exists(updatedPackageJsonPath) && File.Exists(updatedModScriptSourcePath);

        public void InstallMod()
        {
            var fullModPath = GetFullModPath();
            if (!Directory.Exists(fullModPath))
                Directory.CreateDirectory(fullModPath);
            var srcPath = Path.Combine(fullModPath, srcDirName);
            if (!Directory.Exists(srcPath))
                Directory.CreateDirectory(srcPath);
            var paths = GetFilesPaths();
            File.Copy(paths.srcPath, Path.Combine(srcPath, modScriptFileName), true);
            File.Copy(paths.packagePath, Path.Combine(fullModPath, packageJsonFileName), true);
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
            if (File.Exists(updatedPackageJsonPath))
                AvailableVersion = GetModVersion(GetModPackageInfo(updatedPackageJsonPath));
            FileDownloader fileDownloader = new();
            try
            {
                var packageUrl = new Uri(updateUrl, packageJsonFileName);
                await fileDownloader.DownloadFromUrl(packageUrl.AbsoluteUri, updatedPackageJsonPath);
                var latestVersion = GetModVersion(GetModPackageInfo(updatedPackageJsonPath));
                if (latestVersion > AvailableVersion)
                {
                    await fileDownloader.DownloadFromUrl(new Uri(updateUrl, modScriptSourceFileName).AbsoluteUri,
                                                         updatedModScriptSourcePath);

                    AvailableVersion = latestVersion;
                }
            }
            catch { }
        }

        private static ModPackageInfo GetModPackageInfo(string filename)
            => JsonConvert.DeserializeObject<ModPackageInfo>(File.ReadAllText(filename));

        private static Version GetModVersion(ModPackageInfo modPackage)
            => Version.Parse(modPackage.Version);

        private string GetFullModPath() => Path.Combine(AppData.AppSettings.ServerPath, modPath);

        private HelperModStatus CheckModStatus()
        {
            var fullModPath = GetFullModPath();
            if (!File.Exists(Path.Combine(fullModPath, srcDirName, modScriptFileName)))
                return HelperModStatus.NotInstalled;

            var installedModPackageJson = Path.Combine(fullModPath, packageJsonFileName);
            if (!File.Exists(installedModPackageJson))
                return HelperModStatus.NotInstalled;

            if (AvailableVersion != null
                && AvailableVersion > GetModVersion(GetModPackageInfo(installedModPackageJson))
                && HaveUpdatedFiles)
                return HelperModStatus.UpdateAvailable;

            return HelperModStatus.Installed;
        }

        private bool CheckDbStatus()
        {
            var fullDbPath = Path.Combine(AppData.AppSettings.ServerPath, helperDbPath);
            if (Directory.Exists(fullDbPath)
                && Directory.GetFiles(fullDbPath, "*.json").Any()
                && Directory.GetDirectories(fullDbPath).Any())
                return true;
            return false;
        }

        private (string packagePath, string srcPath) GetFilesPaths()
        {
            if (HaveUpdatedFiles)
                return (updatedPackageJsonPath, updatedModScriptSourcePath);
            var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                           modSourceDirName,
                                           packageJsonFileName);
            var srcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                       modSourceDirName,
                                       modScriptSourceFileName);
            return (packagePath, srcPath);
        }
    }
}