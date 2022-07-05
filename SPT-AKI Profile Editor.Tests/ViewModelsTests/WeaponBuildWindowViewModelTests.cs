using MahApps.Metro.Controls.Dialogs;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class WeaponBuildWindowViewModelTests
    {
        [Test]
        public void PmcWeaponBuildInitializeCorrectly()
        {
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.PMC);
            Assert.Multiple(() =>
            {
                Assert.That(pmcWeaponBuild, Is.Not.Null);
                Assert.That(pmcWeaponBuild.Worker, Is.Not.Null);
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(GetTestName(StashEditMode.PMC)));
                Assert.That(pmcWeaponBuild.WeaponBuild, Is.Not.Null);
                Assert.That(pmcWeaponBuild.WeaponBuild.Items.Length, Is.EqualTo(4));
            });
        }

        [Test]
        public void ScavWeaponBuildInitializeCorrectly()
        {
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.Scav);
            Assert.Multiple(() =>
            {
                Assert.That(pmcWeaponBuild, Is.Not.Null);
                Assert.That(pmcWeaponBuild.Worker, Is.Not.Null);
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(GetTestName(StashEditMode.Scav)));
                Assert.That(pmcWeaponBuild.WeaponBuild, Is.Not.Null);
                Assert.That(pmcWeaponBuild.WeaponBuild.Items.Length, Is.EqualTo(6));
            });
        }

        [Test]
        public void PmcWeaponBuildCanAddWeaponToWeaponBuilds()
        {
            AppData.Profile.WeaponBuilds = new();
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.PMC);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.WeaponBuilds.ContainsKey(GetTestName(StashEditMode.PMC)),
                Is.True.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        [Test]
        public void ScavWeaponBuildCanAddWeaponToWeaponBuilds()
        {
            AppData.Profile.WeaponBuilds = new();
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.Scav);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.WeaponBuilds.ContainsKey(GetTestName(StashEditMode.Scav)),
                Is.True.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        private static WeaponBuildWindowViewModel TestViewModel(StashEditMode editMode)
        {
            CharacterInventory pmcInventory = new()
            {
                Items = TestConstants.GenerateTestItems(3, GetTestName(editMode))
            };
            CharacterInventory scavInventory = new()
            {
                Items = TestConstants.GenerateTestItems(5, GetTestName(editMode))
            };
            Character pmc = new()
            {
                Inventory = pmcInventory,
            };
            Character scav = new()
            {
                Inventory = scavInventory,
            };
            ProfileCharacters characters = new()
            {
                Pmc = pmc,
                Scav = scav
            };
            InventoryItem item = new()
            {
                Id = GetTestName(editMode),
                Tpl = GetTestName(editMode)
            };
            AppData.Profile.Characters = characters;
            return new(item, editMode, DialogCoordinator.Instance);
        }

        private static string GetTestName(StashEditMode editMode)
        {
            return editMode switch
            {
                StashEditMode.PMC => "WeaponBuildWindowViewModel_Test_PMC",
                StashEditMode.Scav => "WeaponBuildWindowViewModel_Test_Scav",
                _ => "WeaponBuildWindowViewModel_Test_Unknown",
            };
        }
    }
}