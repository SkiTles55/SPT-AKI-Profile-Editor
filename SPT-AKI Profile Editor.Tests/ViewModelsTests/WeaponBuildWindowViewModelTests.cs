using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class WeaponBuildWindowViewModelTests
    {
        private static readonly TestsDialogManager dialogManager = new();
        private static readonly TestsWindowsDialogs windowsDialogs = new();
        private static readonly TestsWorker worker = new();

        [Test]
        public void InitializeCorrectlyForPmc()
        {
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(worker, true);
            Assert.Multiple(() =>
            {
                Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
                Assert.That(pmcWeaponBuild.Worker, Is.Not.Null, "Worker is null");
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(TestHelpers.GetTestName("WeaponBuildWindowViewModel", true)), "Wrong WindowTitle");
                Assert.That(pmcWeaponBuild.WeaponBuild, Is.Not.Null, "WeaponBuild is null");
                Assert.That(pmcWeaponBuild.WeaponBuild.Items.Length, Is.EqualTo(4), "WeaponBuild.Items.Length is not 4");
            });
        }

        [Test]
        public void InitializeCorrectlyForScav()
        {
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(worker, false);
            Assert.Multiple(() =>
            {
                Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
                Assert.That(pmcWeaponBuild.Worker, Is.Not.Null, "Worker is null");
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(TestHelpers.GetTestName("WeaponBuildWindowViewModel", false)), "Wrong WindowTitle");
                Assert.That(pmcWeaponBuild.WeaponBuild, Is.Not.Null, "WeaponBuild is null");
                Assert.That(pmcWeaponBuild.WeaponBuild.Items.Length, Is.EqualTo(6), "WeaponBuild.Items.Length is not 6");
            });
        }

        [Test]
        public void CanAddWeaponToWeaponBuildsFromPmc()
        {
            AppData.Profile.UserBuilds = new()
            {
                WeaponBuilds = new()
            };
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(worker, true);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.UserBuilds.WeaponBuilds.Any(x => x.Name == TestHelpers.GetTestName("WeaponBuildWindowViewModel", true)), Is.True);
        }

        [Test]
        public void CanAddWeaponToWeaponBuildsFromScav()
        {
            AppData.Profile.UserBuilds = new()
            {
                WeaponBuilds = new()
            };
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(worker, false);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.UserBuilds.WeaponBuilds.Any(x => x.Name == TestHelpers.GetTestName("WeaponBuildWindowViewModel", false)), Is.True);
        }

        [Test]
        public void CanRemoveFromPmc()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var weapon = AppData.Profile.Characters.Pmc.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel pmcWeaponBuild = new(weapon, AppData.Profile.Characters.Pmc.Inventory, null, null, dialogManager, worker);
            Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            pmcWeaponBuild.RemoveItem.Execute(null);
            Assert.That(() => AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Id == weapon.Id), Is.False);
        }

        [Test]
        public void CanRemoveFromScav()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var weapon = AppData.Profile.Characters.Scav.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel scavWeaponBuild = new(weapon, AppData.Profile.Characters.Scav.Inventory, null, null, dialogManager, worker);
            Assert.That(scavWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            scavWeaponBuild.RemoveItem.Execute(null);
            Assert.That(() => AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.Id == weapon.Id), Is.False);
        }

        [Test]
        public void CanExportPmcBuild()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var weapon = AppData.Profile.Characters.Pmc.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel pmcWeaponBuild = new(weapon, AppData.Profile.Characters.Pmc.Inventory, null, windowsDialogs, dialogManager, worker);
            Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            pmcWeaponBuild.ExportBuild.Execute(null);
            Assert.That(File.Exists(windowsDialogs.weaponBuildExportPath), "WeaponBuild not exported");
            WeaponBuild build = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(windowsDialogs.weaponBuildExportPath));
            Assert.That(build, Is.Not.Null, "Unable to read exported weapon build");
            Assert.That(build.RootTpl, Is.EqualTo(weapon.Tpl), "Exported wrong weapon");
            Assert.That(build.Type, Is.EqualTo(WeaponBuild.WeaponBuildType), "WeaponBuildType is wrong");
        }

        [Test]
        public void CanInitializeWorker()
        {
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(null, true);
            Assert.That(pmcWeaponBuild.Worker, Is.Not.Null);
            Assert.That(pmcWeaponBuild.Worker is Worker, Is.True);
        }

        private static WeaponBuildWindowViewModel TestViewModel(IWorker worker, bool isPmc)
        {
            TestHelpers.SetupTestCharacters("WeaponBuildWindowViewModel");
            InventoryItem item = new()
            {
                Id = TestHelpers.GetTestName("WeaponBuildWindowViewModel", isPmc),
                Tpl = TestHelpers.GetTestName("WeaponBuildWindowViewModel", isPmc)
            };
            return new(item,
                       isPmc ? AppData.Profile.Characters.Pmc.Inventory : AppData.Profile.Characters.Scav.Inventory,
                       null,
                       null,
                       dialogManager,
                       worker);
        }
    }
}