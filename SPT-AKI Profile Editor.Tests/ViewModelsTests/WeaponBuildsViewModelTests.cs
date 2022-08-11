using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class WeaponBuildsViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), null);
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
            if (!AppData.Profile.HasWeaponBuilds)
                AppData.Profile.ImportBuildFromFile(TestHelpers.weaponBuild);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker());
            var build = AppData.Profile.WeaponBuilds.Values.FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            var count = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == build.RootTpl).Count();
            viewModel.AddBuildToStash.Execute(build);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == build.RootTpl).Count(), Is.GreaterThan(count));
        }

        [Test]
        public void CanRemoveBuild()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.HasWeaponBuilds)
                AppData.Profile.ImportBuildFromFile(TestHelpers.weaponBuild);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker());
            var buildId = AppData.Profile.WeaponBuilds.Keys.FirstOrDefault();
            Assert.That(string.IsNullOrEmpty(buildId), Is.False);
            viewModel.RemoveBuild.Execute(buildId);
            Assert.That(AppData.Profile.WeaponBuilds.Where(x => x.Key == buildId).Count(), Is.Zero);
        }

        [Test]
        public void CanRemoveBuilds()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (!AppData.Profile.HasWeaponBuilds)
                AppData.Profile.ImportBuildFromFile(TestHelpers.weaponBuild);
            Assert.That(AppData.Profile.HasWeaponBuilds, Is.True);
            WeaponBuildsViewModel viewModel = new(new TestsDialogManager(), new TestsWorker());
            viewModel.RemoveBuilds.Execute(null);
            Assert.That(AppData.Profile.HasWeaponBuilds, Is.False);
        }
    }
}