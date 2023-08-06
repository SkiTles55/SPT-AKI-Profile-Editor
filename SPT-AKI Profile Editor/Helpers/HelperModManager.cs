using SPT_AKI_Profile_Editor.Core;
using System.IO;

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
    }

    public class HelperModManager : IHelperModManager
    {
        private readonly string helperDbPath;
        private readonly string modPath;

        public HelperModManager(string modPath = "user\\mods\\ProfileEditorHelper")
        {
            this.modPath = modPath;
            this.helperDbPath = Path.Combine(modPath, "exportedDB");
        }

        public HelperModStatus HelperModStatus => CheckModStatus();
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus == HelperModStatus.Installed;

        public bool DbFilesExist => false;

        public string DbPath => helperDbPath;

        public void InstallMod()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveMod()
        {
            var fullModPath = Path.Combine(AppData.AppSettings.ServerPath, modPath);
            Directory.Delete(fullModPath, true);
        }

        public void UpdateMod()
        {
            throw new System.NotImplementedException();
        }

        private HelperModStatus CheckModStatus()
        {
            var fullModPath = Path.Combine(AppData.AppSettings.ServerPath, modPath);
            if (File.Exists(Path.Combine(fullModPath, "src", "mod.ts"))
                && File.Exists(Path.Combine(fullModPath, "package.json")))
                return HelperModStatus.Installed;
            return HelperModStatus.NotInstalled;
        }
    }
}