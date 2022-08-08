using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class ScavStashTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            ScavStashTabViewModel viewModel = new(null, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.RemoveItem, Is.Not.Null);
            Assert.That(viewModel.RemoveAllEquipment, Is.Not.Null);
        }

        [Test]
        public void HasNeededData()
        {
            Assert.That(ScavStashTabViewModel.OpenContainer, Is.Not.Null);
            Assert.That(ScavStashTabViewModel.InspectWeapon, Is.Not.Null);
        }

        [Test]
        public void CanRemoveItem()
        {
            TestHelpers.LoadDatabaseAndProfile();
            ScavStashTabViewModel viewModel = new(new TestsDialogManager(), null);
            var item = AppData.Profile.Characters.Scav.Inventory.Items.Where(x => x.IsWeapon).FirstOrDefault();
            Assert.That(item, Is.Not.Null);
            viewModel.RemoveItem.Execute(item.Id);
            Assert.That(AppData.Profile.Characters.Scav.Inventory.Items.Where(x => x.Id == item.Id).FirstOrDefault(), Is.Null, "Item is not removed");
        }

        [Test]
        public void CanRemoveAllEquipment()
        {
            var dialogManager = new TestsDialogManager();
            TestHelpers.LoadDatabaseAndProfile();
            ScavStashTabViewModel viewModel = new(dialogManager, new TestsWorker());
            viewModel.RemoveAllEquipment.Execute(null);
            Assert.That(AppData.Profile.Characters.Scav.Inventory.HasEquipment, Is.False, "All equipment not removed");
        }
    }
}