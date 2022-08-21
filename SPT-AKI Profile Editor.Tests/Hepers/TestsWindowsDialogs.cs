using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal enum FolderBrowserDialogMode
    {
        weaponBuildsExport,
        serverFolder,
        wrongServerFolder
    }

    internal class TestsWindowsDialogs : IWindowsDialogs
    {
        public readonly string weaponBuildExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                    "testBuildExport.json");

        public readonly string weaponBuildsExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                     "TestWeaponBuildsExport");

        public FolderBrowserDialogMode folderBrowserDialogMode = FolderBrowserDialogMode.weaponBuildsExport;

        public TestsWindowsDialogs()
        {
            if (File.Exists(weaponBuildExportPath))
                File.Delete(weaponBuildExportPath);
            if (Directory.Exists(weaponBuildsExportPath))
                Directory.Delete(weaponBuildsExportPath, true);
            Directory.CreateDirectory(weaponBuildsExportPath);
        }

        public (bool success, string path) FolderBrowserDialog(bool showNewFolderButton = true,
                                                               string startPath = null,
                                                               string description = null)
        {
            return folderBrowserDialogMode switch
            {
                FolderBrowserDialogMode.weaponBuildsExport => (true, weaponBuildsExportPath),
                FolderBrowserDialogMode.serverFolder => (true, TestHelpers.serverPath),
                FolderBrowserDialogMode.wrongServerFolder => (true, TestHelpers.wrongServerPath),
                _ => throw new NotImplementedException(),
            };
        }

        public (bool success, string path, string[] paths) OpenWeaponBuildDialog()
        {
            return (true, null, new string[2] { TestHelpers.weaponBuild, TestHelpers.weaponBuild });
        }

        public (bool success, string path) SaveFileDialog(string fileName, string filter = null)
        {
            throw new NotImplementedException();
        }

        public (bool success, string path) SaveWeaponBuildDialog(string name) => (true, weaponBuildExportPath);
    }
}