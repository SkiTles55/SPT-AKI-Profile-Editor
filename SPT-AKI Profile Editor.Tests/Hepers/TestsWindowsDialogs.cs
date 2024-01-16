using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal enum FolderBrowserDialogMode
    {
        weaponBuildsExport,
        equipmentBuildsExport,
        serverFolder,
        wrongServerFolder,
        profileProgressExport
    }

    internal class TestsWindowsDialogs : IWindowsDialogs
    {
        public readonly string weaponBuildExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                    "testBuildExport.json");

        public readonly string equipmentBuildExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                    "equipmentBuildExport.json");

        public readonly string weaponBuildsExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                     "TestWeaponBuildsExport");

        public readonly string equipmentBuildsExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                     "TestEquipmentBuildsExport");

        public readonly string profileProgressExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                    "testProfileProgressExport.json");

        public FolderBrowserDialogMode folderBrowserDialogMode = FolderBrowserDialogMode.weaponBuildsExport;

        public bool SaveProfileProgressDialogCalled { get; internal set; } = false;
        public bool OpenBuildDialogCalled { get; internal set; } = false;

        public TestsWindowsDialogs()
        {
            PrepareTestPaths(weaponBuildExportPath, weaponBuildsExportPath);
            PrepareTestPaths(equipmentBuildExportPath, equipmentBuildsExportPath);
        }

        public (bool success, string path) FolderBrowserDialog(bool showNewFolderButton = true,
                                                               string startPath = null,
                                                               string description = null)
        {
            return folderBrowserDialogMode switch
            {
                FolderBrowserDialogMode.weaponBuildsExport => (true, weaponBuildsExportPath),
                FolderBrowserDialogMode.equipmentBuildsExport => (true, equipmentBuildsExportPath),
                FolderBrowserDialogMode.serverFolder => (true, TestHelpers.serverPath),
                FolderBrowserDialogMode.wrongServerFolder => (true, TestHelpers.wrongServerPath),
                _ => throw new NotImplementedException(),
            };
        }

        public (bool success, string path, string[] paths) OpenBuildDialog(bool multiselect = true)
        {
            OpenBuildDialogCalled = true;
            return folderBrowserDialogMode switch
            {
                FolderBrowserDialogMode.weaponBuildsExport
                => (true, null, new string[2] { TestHelpers.weaponBuild, TestHelpers.weaponBuild }),
                FolderBrowserDialogMode.equipmentBuildsExport
                => (true, null, new string[2] { TestHelpers.equipmentBuild, TestHelpers.equipmentBuild }),
                FolderBrowserDialogMode.profileProgressExport => (true, TestHelpers.profileProgress, null),
                _ => throw new NotImplementedException(),
            };
        }

        public (bool success, string path) SaveFileDialog(string fileName, string filter = null)
        {
            throw new NotImplementedException();
        }

        public (bool success, string path) SaveWeaponBuildDialog(string name) => (true, weaponBuildExportPath);

        public (bool success, string path) SaveEquipmentBuildDialog(string name) => (true, equipmentBuildExportPath);

        public (bool success, string path) SaveProfileProgressDialog(string name)
        {
            SaveProfileProgressDialogCalled = true;
            return (true, profileProgressExportPath);
        }

        private static void PrepareTestPaths(string filePath, string directoryPath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);

            Directory.CreateDirectory(directoryPath);
        }
    }
}