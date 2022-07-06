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
                Assert.That(pmcContainer, Is.Not.Null);
                Assert.That(pmcContainer.Worker, Is.Not.Null);
                Assert.That(pmcContainer.WindowTitle, Is.EqualTo(TestConstants.GetTestName("ContainerWindowViewModel", StashEditMode.PMC)));
                Assert.That(pmcContainer.HasItems, Is.True);
                Assert.That(pmcContainer.Items.Count, Is.EqualTo(3));
                Assert.That(pmcContainer.ItemsAddingAllowed, Is.False);
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