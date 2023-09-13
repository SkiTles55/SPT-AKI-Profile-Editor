using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class BuildsTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            BuildsTabViewModel viewModel = MakeSUT();
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.RemoveBuild, Is.Not.Null);
            Assert.That(viewModel.RemoveWeaponBuilds, Is.Not.Null);
            Assert.That(viewModel.RemoveEquipmentBuilds, Is.Not.Null);
            Assert.That(viewModel.AddBuildToStash, Is.Not.Null);
            Assert.That(viewModel.ExportBuild, Is.Not.Null);
            Assert.That(viewModel.ExportWeaponBuilds, Is.Not.Null);
            Assert.That(viewModel.ExportEquipmentBuilds, Is.Not.Null);
            Assert.That(viewModel.ImportWeaponBuilds, Is.Not.Null);
            Assert.That(viewModel.ImportEquipmentBuilds, Is.Not.Null);
        }

        [Test]
        public void CanAddBuildToStash()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            BuildsTabViewModel viewModel = MakeSUT();
            var build = AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            var count = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == build.RootTpl).Count();
            viewModel.AddBuildToStash.Execute(build);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == build.RootTpl).Count(), Is.GreaterThan(count));
        }

        [Test]
        public void CanRemoveWeaponBuild()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            BuildsTabViewModel viewModel = MakeSUT();
            var build = AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            viewModel.RemoveBuild.Execute(build);
            Assert.That(AppData.Profile.UserBuilds.WeaponBuilds.Where(x => x.Id == build.Id).Count(), Is.Zero);
        }

        [Test]
        public void CanRemoveEquipmentBuild()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasEquipmentBuilds)
                AppData.Profile.UserBuilds.ImportEquipmentBuildFromFile(TestHelpers.equipmentBuild);
            BuildsTabViewModel viewModel = MakeSUT();
            var build = AppData.Profile.UserBuilds.EquipmentBuilds.FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            viewModel.RemoveBuild.Execute(build);
            Assert.That(AppData.Profile.UserBuilds.EquipmentBuilds.Where(x => x.Id == build.Id).Count(), Is.Zero);
        }

        [Test]
        public void CanRemoveWeaponBuilds()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            Assert.That(AppData.Profile.UserBuilds.HasWeaponBuilds, Is.True);
            BuildsTabViewModel viewModel = MakeSUT();
            viewModel.RemoveWeaponBuilds.Execute(null);
            Assert.That(AppData.Profile.UserBuilds.HasWeaponBuilds, Is.False);
        }

        [Test]
        public void CanRemoveEquipmentBuilds()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasEquipmentBuilds)
                AppData.Profile.UserBuilds.ImportEquipmentBuildFromFile(TestHelpers.equipmentBuild);
            Assert.That(AppData.Profile.UserBuilds.HasEquipmentBuilds, Is.True);
            BuildsTabViewModel viewModel = MakeSUT();
            viewModel.RemoveEquipmentBuilds.Execute(null);
            Assert.That(AppData.Profile.UserBuilds.HasEquipmentBuilds, Is.False);
        }

        [Test]
        public void CanExportWeaponBuildToFile()
        {
            TestsWindowsDialogs windowsDialogs = new();
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            BuildsTabViewModel viewModel = MakeSUT(windowsDialogs);
            var build = AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            viewModel.ExportBuild.Execute(build);
            Assert.That(File.Exists(windowsDialogs.weaponBuildExportPath), "WeaponBuild not exported");
            WeaponBuild exportedBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(windowsDialogs.weaponBuildExportPath));
            Assert.That(exportedBuild, Is.Not.Null, "Unable to read exported weapon build");
            Assert.That(exportedBuild.RootTpl, Is.EqualTo(build.RootTpl), "Exported wrong weapon");
            Assert.That(exportedBuild.Type, Is.EqualTo(WeaponBuild.WeaponBuildType), "WeaponBuildType is wrong");
        }

        [Test]
        public void CanExportEquipmentBuildToFile()
        {
            TestsWindowsDialogs windowsDialogs = new();
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasEquipmentBuilds)
                AppData.Profile.UserBuilds.ImportEquipmentBuildFromFile(TestHelpers.equipmentBuild);
            BuildsTabViewModel viewModel = MakeSUT(windowsDialogs);
            var build = AppData.Profile.UserBuilds.EquipmentBuilds.FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            viewModel.ExportBuild.Execute(build);
            Assert.That(File.Exists(windowsDialogs.equipmentBuildExportPath), "EquipmentBuild not exported");
            EquipmentBuild exportedBuild = JsonConvert.DeserializeObject<EquipmentBuild>(File.ReadAllText(windowsDialogs.equipmentBuildExportPath));
            Assert.That(exportedBuild, Is.Not.Null, "Unable to read exported equipment build");
            Assert.That(exportedBuild.Name, Is.EqualTo(build.Name), "Exported wrong equipment build");
            Assert.That(exportedBuild.Type, Is.EqualTo(EquipmentBuild.EquipmentBuildType), "EquipmentBuildType is wrong");
        }

        [Test]
        public void CanExportWeaponBuilds()
        {
            TestsWindowsDialogs windowsDialogs = GetWindowsDialogs(FolderBrowserDialogMode.weaponBuildsExport);
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            BuildsTabViewModel viewModel = MakeSUT(windowsDialogs);
            var expectedCount = AppData.Profile.UserBuilds.WeaponBuilds.Count;
            viewModel.ExportWeaponBuilds.Execute(null);
            Assert.That(Directory.EnumerateFiles(windowsDialogs.weaponBuildsExportPath).Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void CanExportEquipmentBuilds()
        {
            TestsWindowsDialogs windowsDialogs = GetWindowsDialogs(FolderBrowserDialogMode.equipmentBuildsExport);
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasEquipmentBuilds)
                AppData.Profile.UserBuilds.ImportEquipmentBuildFromFile(TestHelpers.equipmentBuild);
            BuildsTabViewModel viewModel = MakeSUT(windowsDialogs);
            var expectedCount = AppData.Profile.UserBuilds.EquipmentBuilds.Count;
            viewModel.ExportEquipmentBuilds.Execute(null);
            Assert.That(Directory.EnumerateFiles(windowsDialogs.equipmentBuildsExportPath).Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void CanImportWeaponBuilds()
        {
            TestsWindowsDialogs windowsDialogs = GetWindowsDialogs(FolderBrowserDialogMode.weaponBuildsExport);
            TestHelpers.LoadDatabaseAndProfile();
            BuildsTabViewModel viewModel = MakeSUT(windowsDialogs);
            var startCount = AppData.Profile.UserBuilds.WeaponBuilds.Count;
            viewModel.ImportWeaponBuilds.Execute(null);
            Assert.That(AppData.Profile.UserBuilds.WeaponBuilds.Count, Is.EqualTo(startCount + 2));
        }

        [Test]
        public void CanImportEquipmentBuilds()
        {
            TestsWindowsDialogs windowsDialogs = GetWindowsDialogs(FolderBrowserDialogMode.equipmentBuildsExport);
            TestHelpers.LoadDatabaseAndProfile();
            BuildsTabViewModel viewModel = MakeSUT(windowsDialogs);
            var startCount = AppData.Profile.UserBuilds.EquipmentBuilds.Count;
            viewModel.ImportEquipmentBuilds.Execute(null);
            Assert.That(AppData.Profile.UserBuilds.EquipmentBuilds.Count, Is.EqualTo(startCount + 2));
        }

        private static TestsWindowsDialogs GetWindowsDialogs(FolderBrowserDialogMode mode)
            => new() { folderBrowserDialogMode = mode };

        private static BuildsTabViewModel MakeSUT(TestsWindowsDialogs windowsDialogs = null)
            => new(new TestsDialogManager(), new TestsWorker(), windowsDialogs, null);
    }
}