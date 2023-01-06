using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class ContainerWindowViewModelTests
    {
        private static readonly TestsDialogManager dialogManager = new();
        private static readonly TestsWorker worker = new();

        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void InitializeCorrectlyForPmc()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.PMC);
            Assert.Multiple(() =>
            {
                Assert.That(pmcContainer, Is.Not.Null, "ContainerWindowViewModel is null");
                Assert.That(pmcContainer.WindowTitle, Is.EqualTo(TestHelpers.GetTestName("ContainerWindowViewModel", StashEditMode.PMC)), "Wrong WindowTitle");
                Assert.That(pmcContainer.HasItems, Is.True, "HasItems is false");
                Assert.That(pmcContainer.Items, Is.Not.Null, "Items is not null");
                Assert.That(pmcContainer.Items.Count, Is.EqualTo(3), "Items.Count is not 3");
                Assert.That(pmcContainer.ItemsAddingAllowed, Is.False, "ItemsAddingAllowed is true");
                Assert.That(pmcContainer.CategoriesForItemsAdding.Count, Is.GreaterThan(0), "CategoriesForItemsAdding is empty");
            });
        }

        [Test]
        public void InitializeCorrectlyForScav()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.Scav);
            Assert.Multiple(() =>
            {
                Assert.That(pmcContainer, Is.Not.Null, "ContainerWindowViewModel is null");
                Assert.That(pmcContainer.WindowTitle, Is.EqualTo(TestHelpers.GetTestName("ContainerWindowViewModel", StashEditMode.Scav)), "Wrong WindowTitle");
                Assert.That(pmcContainer.HasItems, Is.True, "HasItems is false");
                Assert.That(pmcContainer.Items, Is.Not.Null, "Items is not null");
                Assert.That(pmcContainer.Items.Count, Is.EqualTo(5), "Items.Count is not 5");
                Assert.That(pmcContainer.ItemsAddingAllowed, Is.False, "ItemsAddingAllowed is true");
                Assert.That(pmcContainer.CategoriesForItemsAdding.Count, Is.GreaterThan(0), "CategoriesForItemsAdding is empty");
            });
        }

        [Test]
        public void CanOpenContainer()
        {
            var applicationManager = new TestsApplicationManager();
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.PMC, applicationManager);
            pmcContainer.OpenContainer.Execute(null);
            Assert.That(applicationManager.ContainerWindowOpened, Is.True);
        }

        [Test]
        public void CanInspectWeapon()
        {
            var applicationManager = new TestsApplicationManager();
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.PMC, applicationManager);
            pmcContainer.InspectWeapon.Execute(null);
            Assert.That(applicationManager.WeaponBuildWindowOpened, Is.True);
        }

        [Test]
        public void CanRemoveItem()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.PMC);
            var item = pmcContainer.Items[0];
            pmcContainer.RemoveItem.Execute(item.Id);
            Assert.That(pmcContainer.Items.IndexOf(item), Is.EqualTo(-1), "Item not removed");
        }

        [Test]
        public void CanRemoveAllItems()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(StashEditMode.PMC);
            Assert.That(pmcContainer.HasItems, Is.True, "ContainerWindowViewModel does not contains items");
            pmcContainer.RemoveAllItems.Execute(null);
            Assert.That(pmcContainer.HasItems, Is.False, "ContainerWindowViewModel contains items after RemoveAllItems");
        }

        [Test]
        public void CanAddItem()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var container = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.IsContainer && x.CanAddItems).FirstOrDefault();
            Assert.That(container, Is.Not.Null, "Cant find container");
            ContainerWindowViewModel pmcContainer = new(container, StashEditMode.PMC, null, null, dialogManager, worker);
            Assert.That(pmcContainer.ItemsAddingAllowed, Is.True, "Items adding not allowed for opened container");
            var painkiller = AppData.ServerDatabase.ItemsDB["544fb37f4bdc2dee738b4567"];
            pmcContainer.RemoveAllItems.Execute(null);
            pmcContainer.AddItem.Execute(painkiller);
            Assert.That(pmcContainer.Items.Count, Is.EqualTo(1), "Item not added");
            Assert.That(pmcContainer.Items.Where(x => x.Tpl == "544fb37f4bdc2dee738b4567").FirstOrDefault(), Is.Not.Null, "Item not added");
        }

        private static ContainerWindowViewModel TestViewModel(StashEditMode editMode, IApplicationManager applicationManager = null)
        {
            TestHelpers.SetupTestCharacters("ContainerWindowViewModel", editMode);
            InventoryItem item = new()
            {
                Id = TestHelpers.GetTestName("ContainerWindowViewModel", editMode),
                Tpl = TestHelpers.GetTestName("ContainerWindowViewModel", editMode)
            };
            return new(item, editMode, null, applicationManager, dialogManager, worker);
        }
    }
}