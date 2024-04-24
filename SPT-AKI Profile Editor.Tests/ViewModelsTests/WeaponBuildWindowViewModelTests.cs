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
        public void InitializeCorrectlyForPmc() => InitializeAndCheck(true, 4);

        [Test]
        public void InitializeCorrectlyForScav() => InitializeAndCheck(false, 6);

        [Test]
        public void CanAddWeaponToWeaponBuildsFromPmc()
        {
            AppData.Profile.UserBuilds = new() { WeaponBuilds = new() };
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(worker, true);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.UserBuilds.WeaponBuilds.Any(x => x.Name == TestHelpers.GetTestName("WeaponBuildWindowViewModel", true)), Is.True);
        }

        [Test]
        public void CanAddWeaponToWeaponBuildsFromScav()
        {
            AppData.Profile.UserBuilds = new() { WeaponBuilds = new() };
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(worker, false);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.UserBuilds.WeaponBuilds.Any(x => x.Name == TestHelpers.GetTestName("WeaponBuildWindowViewModel", false)), Is.True);
        }

        [Test]
        public void CanRemoveFromPmc()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var weapon = AppData.Profile.Characters.Pmc.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel pmcWeaponBuild = new(weapon, AppData.Profile.Characters.Pmc.Inventory, null, null, true, dialogManager, worker);
            Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            pmcWeaponBuild.RemoveItem.Execute(null);
            Assert.That(() => AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Id == weapon.Id), Is.False);
        }

        [Test]
        public void CanRemoveFromScav()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var weapon = AppData.Profile.Characters.Scav.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel scavWeaponBuild = new(weapon, AppData.Profile.Characters.Scav.Inventory, null, null, true, dialogManager, worker);
            Assert.That(scavWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            scavWeaponBuild.RemoveItem.Execute(null);
            Assert.That(() => AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.Id == weapon.Id), Is.False);
        }

        [Test]
        public void CanExportPmcBuild()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var weapon = AppData.Profile.Characters.Pmc.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel pmcWeaponBuild = new(weapon, AppData.Profile.Characters.Pmc.Inventory, null, windowsDialogs, true, dialogManager, worker);
            Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            pmcWeaponBuild.ExportBuild.Execute(null);
            Assert.That(File.Exists(windowsDialogs.weaponBuildExportPath), "WeaponBuild not exported");
            WeaponBuild build = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(windowsDialogs.weaponBuildExportPath));
            Assert.That(build, Is.Not.Null, "Unable to read exported weapon build");
            Assert.That(build.RootTpl, Is.EqualTo(weapon.Tpl), "Exported wrong weapon");
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
                       true,
                       dialogManager,
                       worker);
        }

        private static void InitializeAndCheck(bool isPmc, int expectedCount)
        {
            WeaponBuildWindowViewModel weaponBuild = TestViewModel(worker, isPmc);
            Assert.Multiple(() =>
            {
                Assert.That(weaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
                Assert.That(weaponBuild.Worker, Is.Not.Null, "Worker is null");
                Assert.That(weaponBuild.WindowTitle, Is.EqualTo(TestHelpers.GetTestName("WeaponBuildWindowViewModel", isPmc)), "Wrong WindowTitle");
                Assert.That(weaponBuild.WeaponBuild, Is.Not.Null, "WeaponBuild is null");
                Assert.That(weaponBuild.WeaponBuild.Items.Length, Is.EqualTo(expectedCount), $"WeaponBuild.Items.Length is not {expectedCount}");
            });
        }
    }
}