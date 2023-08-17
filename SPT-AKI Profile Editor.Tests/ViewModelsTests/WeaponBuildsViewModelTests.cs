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
    internal class WeaponBuildsViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), null, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.RemoveBuild, Is.Not.Null);
            Assert.That(viewModel.RemoveBuilds, Is.Not.Null);
            Assert.That(viewModel.AddBuildToStash, Is.Not.Null);
            Assert.That(viewModel.ExportBuild, Is.Not.Null);
            Assert.That(viewModel.ExportBuilds, Is.Not.Null);
            Assert.That(viewModel.ImportBuilds, Is.Not.Null);
        }

        [Test]
        public void CanAddBuildToStash()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            var build = AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            var count = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == build.RootTpl).Count();
            viewModel.AddBuildToStash.Execute(build);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == build.RootTpl).Count(), Is.GreaterThan(count));
        }

        [Test]
        public void CanRemoveBuild()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            var buildId = AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault().Id;
            Assert.That(string.IsNullOrEmpty(buildId), Is.False);
            viewModel.RemoveBuild.Execute(buildId);
            Assert.That(AppData.Profile.UserBuilds.WeaponBuilds.Where(x => x.Id == buildId).Count(), Is.Zero);
        }

        [Test]
        public void CanRemoveBuilds()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            Assert.That(AppData.Profile.UserBuilds.HasWeaponBuilds, Is.True);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            viewModel.RemoveBuilds.Execute(null);
            Assert.That(AppData.Profile.UserBuilds.HasWeaponBuilds, Is.False);
        }

        [Test]
        public void CanExportBuildToFile()
        {
            TestsWindowsDialogs windowsDialogs = new();
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), windowsDialogs);
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
        public void CanExportBuilds()
        {
            TestsWindowsDialogs windowsDialogs = new()
            {
                folderBrowserDialogMode = FolderBrowserDialogMode.weaponBuildsExport
            };
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.UserBuilds.HasWeaponBuilds)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), windowsDialogs);
            var expectedCount = AppData.Profile.UserBuilds.WeaponBuilds.Count;
            viewModel.ExportBuilds.Execute(null);
            Assert.That(Directory.EnumerateFiles(windowsDialogs.weaponBuildsExportPath).Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void CanImportBuilds()
        {
            TestsWindowsDialogs windowsDialogs = new();
            TestHelpers.LoadDatabaseAndProfile();
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), windowsDialogs);
            var startCount = AppData.Profile.UserBuilds.WeaponBuilds.Count;
            viewModel.ImportBuilds.Execute(null);
            Assert.That(AppData.Profile.UserBuilds.WeaponBuilds.Count, Is.EqualTo(startCount + 2));
        }
    }
}