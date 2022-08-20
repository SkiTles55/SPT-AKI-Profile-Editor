using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsWindowsDialogs : IWindowsDialogs
    {
        public readonly string weaponBuildExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testBuildExport.json");

        public TestsWindowsDialogs()
        {
            if (File.Exists(weaponBuildExportPath))
                File.Delete(weaponBuildExportPath);
        }

        public (bool success, string path) FolderBrowserDialog(bool showNewFolderButton = true, string startPath = null, string description = null)
        {
            throw new NotImplementedException();
        }

        public (bool success, string path, string[] paths) OpenWeaponBuildDialog()
        {
            throw new NotImplementedException();
        }

        public (bool success, string path) SaveFileDialog(string fileName, string filter = null)
        {
            throw new NotImplementedException();
        }

        public (bool success, string path) SaveWeaponBuildDialog(string name) => (true, weaponBuildExportPath);
    }
}