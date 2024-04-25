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
        public void CanAddWeaponToWeaponBuildsFromPmc() => AddWeaponToWeaponBuildsAndCheck(true);

        [Test]
        public void CanAddWeaponToWeaponBuildsFromScav() => AddWeaponToWeaponBuildsAndCheck(false);

        [Test]
        public void CanRemoveFromPmc() => CreateViewModelAndCheckRemoving(true);

        [Test]
        public void CanRemoveFromScav() => CreateViewModelAndCheckRemoving(false);

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

        private static void AddWeaponToWeaponBuildsAndCheck(bool isPmc)
        {
            AppData.Profile.UserBuilds = new() { WeaponBuilds = new() };
            WeaponBuildWindowViewModel weaponBuild = TestViewModel(worker, isPmc);
            weaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.UserBuilds.WeaponBuilds.Any(x => x.Name == TestHelpers.GetTestName("WeaponBuildWindowViewModel", isPmc)), Is.True);
        }

        private static void CreateViewModelAndCheckRemoving(bool isPmc)
        {
            TestHelpers.LoadDatabaseAndProfile();
            var inventory = isPmc ? AppData.Profile.Characters.Pmc.Inventory : AppData.Profile.Characters.Scav.Inventory;
            var weapon = inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel weaponBuild = new(weapon, inventory, null, null, true, dialogManager, worker);
            Assert.That(weaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            weaponBuild.RemoveItem.Execute(null);
            Assert.That(() => inventory.Items.Any(x => x.Id == weapon.Id), Is.False);
        }
    }
}