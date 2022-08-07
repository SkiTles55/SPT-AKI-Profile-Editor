using MahApps.Metro.Controls.Dialogs;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class WeaponBuildWindowViewModelTests
    {
        private static readonly TestsDialogManager dialogManager = new();

        [Test]
        public void InitializeCorrectlyForPmc()
        {
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.PMC);
            Assert.Multiple(() =>
            {
                Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
                Assert.That(pmcWeaponBuild.Worker, Is.Not.Null, "Worker is null");
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.PMC)), "Wrong WindowTitle");
                Assert.That(pmcWeaponBuild.WeaponBuild, Is.Not.Null, "WeaponBuild is null");
                Assert.That(pmcWeaponBuild.WeaponBuild.Items.Length, Is.EqualTo(4), "WeaponBuild.Items.Length is not 4");
            });
        }

        [Test]
        public void InitializeCorrectlyForScav()
        {
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.Scav);
            Assert.Multiple(() =>
            {
                Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
                Assert.That(pmcWeaponBuild.Worker, Is.Not.Null, "Worker is null");
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.Scav)), "Wrong WindowTitle");
                Assert.That(pmcWeaponBuild.WeaponBuild, Is.Not.Null, "WeaponBuild is null");
                Assert.That(pmcWeaponBuild.WeaponBuild.Items.Length, Is.EqualTo(6), "WeaponBuild.Items.Length is not 6");
            });
        }

        [Test]
        public void CanAddWeaponToWeaponBuildsFromPmc()
        {
            AppData.Profile.WeaponBuilds = new();
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.PMC);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.WeaponBuilds.ContainsKey(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.PMC)),
                Is.True.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        [Test]
        public void CanAddWeaponToWeaponBuildsFromScav()
        {
            AppData.Profile.WeaponBuilds = new();
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.Scav);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.WeaponBuilds.ContainsKey(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.Scav)),
                Is.True.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        [Test]
        public void CanRemoveFromPmc()
        {
            AppData.LoadDatabase();
            AppData.Profile.Load(TestConstants.profileFile);
            var weapon = AppData.Profile.Characters.Pmc.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel pmcWeaponBuild = new(weapon, StashEditMode.PMC, DialogCoordinator.Instance, dialogManager);
            Assert.That(pmcWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            pmcWeaponBuild.RemoveItem.Execute(null);
            Assert.That(() => AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Id == weapon.Id),
                Is.False.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        [Test]
        public void CanRemoveFromScav()
        {
            AppData.LoadDatabase();
            AppData.Profile.Load(TestConstants.profileFile);
            var weapon = AppData.Profile.Characters.Scav.Inventory.Items.First(x => x.IsWeapon);
            WeaponBuildWindowViewModel scavWeaponBuild = new(weapon, StashEditMode.Scav, DialogCoordinator.Instance, dialogManager);
            Assert.That(scavWeaponBuild, Is.Not.Null, "WeaponBuildWindowViewModel is null");
            scavWeaponBuild.RemoveItem.Execute(null);
            Assert.That(() => AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.Id == weapon.Id),
                Is.False.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        private static WeaponBuildWindowViewModel TestViewModel(StashEditMode editMode)
        {
            TestConstants.SetupTestCharacters("WeaponBuildWindowViewModel", editMode);
            InventoryItem item = new()
            {
                Id = TestConstants.GetTestName("WeaponBuildWindowViewModel", editMode),
                Tpl = TestConstants.GetTestName("WeaponBuildWindowViewModel", editMode)
            };
            return new(item, editMode, DialogCoordinator.Instance, dialogManager);
        }
    }
}