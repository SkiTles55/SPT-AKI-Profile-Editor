using MahApps.Metro.Controls.Dialogs;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class ContainerWindowViewModelTests
    {
        [Test]
        public void PmcWeaponBuildInitializeCorrectly()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.PMC);
            Assert.Multiple(() =>
            {
                Assert.That(pmcContainer, Is.Not.Null, "ContainerWindowViewModel is null");
                Assert.That(pmcContainer.Worker, Is.Not.Null, "Worker is null");
                Assert.That(pmcContainer.WindowTitle, Is.EqualTo(TestConstants.GetTestName("ContainerWindowViewModel", StashEditMode.PMC)), "Wrong WindowTitle");
                Assert.That(pmcContainer.HasItems, Is.True, "HasItems is false");
                Assert.That(pmcContainer.Items.Count, Is.EqualTo(3), "Items.Count is not 3");
                Assert.That(pmcContainer.ItemsAddingAllowed, Is.False, "ItemsAddingAllowed is true");
            });
        }

        [Test]
        public void ScavWeaponBuildInitializeCorrectly()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.Scav);
            Assert.Multiple(() =>
            {
                Assert.That(pmcContainer, Is.Not.Null, "ContainerWindowViewModel is null");
                Assert.That(pmcContainer.Worker, Is.Not.Null, "Worker is null");
                Assert.That(pmcContainer.WindowTitle, Is.EqualTo(TestConstants.GetTestName("ContainerWindowViewModel", StashEditMode.Scav)), "Wrong WindowTitle");
                Assert.That(pmcContainer.HasItems, Is.True, "HasItems is false");
                Assert.That(pmcContainer.Items.Count, Is.EqualTo(5), "Items.Count is not 5");
                Assert.That(pmcContainer.ItemsAddingAllowed, Is.False, "ItemsAddingAllowed is true");
            });
        }

        private static ContainerWindowViewModel TestViewModel(StashEditMode editMode)
        {
            TestConstants.SetupTestCharacters("ContainerWindowViewModel", editMode);
            InventoryItem item = new()
            {
                Id = TestConstants.GetTestName("ContainerWindowViewModel", editMode),
                Tpl = TestConstants.GetTestName("ContainerWindowViewModel", editMode)
            };
            return new(item, editMode, DialogCoordinator.Instance);
        }
    }
}