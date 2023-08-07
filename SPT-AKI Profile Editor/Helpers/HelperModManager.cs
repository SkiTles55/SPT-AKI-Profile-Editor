using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.ModHelper;
using System;
using System.IO;
using System.Linq;

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

        public void DownloadUpdates();
    }

    public class HelperModManager : IHelperModManager
    {
        private readonly string helperDbPath;
        private readonly string modPath;
        private readonly Uri updateUrl;
        private readonly string updateSaveDirectory;

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
        }

        public HelperModStatus HelperModStatus => CheckModStatus();
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus == HelperModStatus.Installed;
        public bool DbFilesExist => CheckDbStatus();
        public string DbPath => helperDbPath;
        private Version AvailableVersion { get; set; } = new();

        public void InstallMod()
        {
            var fullModPath = GetFullModPath();
            if (!Directory.Exists(fullModPath))
                Directory.CreateDirectory(fullModPath);
            var srcPath = Path.Combine(fullModPath, srcDirName);
            if (!Directory.Exists(srcPath))
                Directory.CreateDirectory(srcPath);
            var srcFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                           modSourceDirName,
                                           modScriptSourceFileName);
            File.Copy(srcFilePath, Path.Combine(srcPath, modScriptFileName), true);
            File.Copy(GetPackageJsonPath(), Path.Combine(fullModPath, packageJsonFileName), true);
        }

        public void RemoveMod()
        {
            var fullModPath = GetFullModPath();
            Directory.Delete(fullModPath, true);
        }

        public void UpdateMod()
        {
            throw new NotImplementedException();
        }

        public async void DownloadUpdates()
        {
            if (!Directory.Exists(updateSaveDirectory))
                Directory.CreateDirectory(updateSaveDirectory);
            FileDownloader fileDownloader = new();
            try
            {
                var packageUrl = new Uri(updateUrl, packageJsonFileName);
                var savePackagePath = Path.Combine(updateSaveDirectory, packageJsonFileName);
                await fileDownloader.DownloadFromUrl(packageUrl.AbsoluteUri, savePackagePath);
                await fileDownloader.DownloadFromUrl(new Uri(updateUrl, modScriptSourceFileName).AbsoluteUri,
                                                     Path.Combine(updateSaveDirectory, modScriptSourceFileName));

                AvailableVersion = GetModVersion(GetModPackageInfo(savePackagePath));
            }
            catch { }
        }

        private static ModPackage GetModPackageInfo(string filename)
            => JsonConvert.DeserializeObject<ModPackage>(File.ReadAllText(filename));

        private static Version GetModVersion(ModPackage modPackage)
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
                && AvailableVersion > GetModVersion(GetModPackageInfo(installedModPackageJson)))
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

        private string GetPackageJsonPath()
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            modSourceDirName,
                            packageJsonFileName);
    }
}