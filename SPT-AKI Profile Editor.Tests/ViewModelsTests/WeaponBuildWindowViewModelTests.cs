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
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.PMC)));
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
                Assert.That(pmcWeaponBuild.WindowTitle, Is.EqualTo(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.Scav)));
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
            Assert.That(() => AppData.Profile.WeaponBuilds.ContainsKey(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.PMC)),
                Is.True.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        [Test]
        public void ScavWeaponBuildCanAddWeaponToWeaponBuilds()
        {
            AppData.Profile.WeaponBuilds = new();
            WeaponBuildWindowViewModel pmcWeaponBuild = TestViewModel(StashEditMode.Scav);
            pmcWeaponBuild.AddToWeaponBuilds.Execute(null);
            Assert.That(() => AppData.Profile.WeaponBuilds.ContainsKey(TestConstants.GetTestName("WeaponBuildWindowViewModel", StashEditMode.Scav)),
                Is.True.After(2).Seconds.PollEvery(250).MilliSeconds);
        }

        private static WeaponBuildWindowViewModel TestViewModel(StashEditMode editMode)
        {
            TestConstants.SetupTestCharacters("WeaponBuildWindowViewModel", editMode);
            InventoryItem item = new()
            {
                Id = TestConstants.GetTestName("WeaponBuildWindowViewModel", editMode),
                Tpl = TestConstants.GetTestName("WeaponBuildWindowViewModel", editMode)
            };
            return new(item, editMode, DialogCoordinator.Instance);
        }
    }
}