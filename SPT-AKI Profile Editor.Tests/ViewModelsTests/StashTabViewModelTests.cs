using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class StashTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            StashTabViewModel viewModel = new(new TestsDialogManager(), null, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.AddItem, Is.Not.Null);
            Assert.That(viewModel.ScavStashTabViewModel, Is.Not.Null);
            Assert.That(viewModel.RemoveItem, Is.Not.Null);
            Assert.That(viewModel.RemoveAllItems, Is.Not.Null);
            Assert.That(viewModel.RemoveAllEquipment, Is.Not.Null);
            Assert.That(viewModel.AddMoney, Is.Not.Null);
            Assert.That(viewModel.OpenContainer, Is.Not.Null);
            Assert.That(viewModel.InspectWeapon, Is.Not.Null);
        }

        [Test]
        public void HasNeededData() => Assert.That(StashTabViewModel.AppSettings, Is.Not.Null);

        [Test]
        public void CanAddItem()
        {
            TestHelpers.LoadDatabaseAndProfile();
            StashTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            var sick = AppData.ServerDatabase.ItemsDB["5c0a840b86f7742ffa4f2482"];
            var sicksCount = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == "5c0a840b86f7742ffa4f2482").Count();
            viewModel.AddItem.Execute(sick);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == "5c0a840b86f7742ffa4f2482").Count(), Is.GreaterThan(sicksCount));
        }

        [Test]
        public void CanRemoveItem()
        {
            TestHelpers.LoadDatabaseAndProfile();
            StashTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            var itemId = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.FirstOrDefault().Id;
            Assert.That(string.IsNullOrEmpty(itemId), Is.False);
            viewModel.RemoveItem.Execute(itemId);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Id == itemId).FirstOrDefault(), Is.Null);
        }

        [Test]
        public void CanRemoveAllItems()
        {
            TestHelpers.LoadDatabaseAndProfile();
            StashTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(), Is.True);
            viewModel.RemoveAllItems.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(), Is.False);
        }

        [Test]
        public void CanRemoveAllEquipment()
        {
            TestHelpers.LoadDatabaseAndProfile();
            StashTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.HasEquipment, Is.True);
            viewModel.RemoveAllEquipment.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.HasEquipment, Is.False);
        }

        [Test]
        public void CanAddMoney()
        {
            TestHelpers.LoadDatabaseAndProfile();
            StashTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            var dollarsCount = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == AppData.AppSettings.MoneysDollarsTpl).Sum(x => x.Upd.StackObjectsCount ?? 0);
            viewModel.AddMoney.Execute(AppData.AppSettings.MoneysDollarsTpl);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == AppData.AppSettings.MoneysDollarsTpl).Sum(x => x.Upd.StackObjectsCount ?? 0), Is.GreaterThan(dollarsCount));
        }
    }
}