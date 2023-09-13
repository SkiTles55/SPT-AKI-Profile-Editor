using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
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
        private static readonly string backpackTpl = "545cdae64bdc2d39198b4568";
        private static string BackpackName => AppData.ServerDatabase.LocalesGlobal[backpackTpl.Name()];

        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void InitializeCorrectlyForPmc() => InitializeCorrectly(TestViewModel(true), 3);

        [Test]
        public void InitializeCorrectlyForScav() => InitializeCorrectly(TestViewModel(false), 5);

        [Test]
        public void CanOpenContainer()
        {
            var applicationManager = new TestsApplicationManager();
            ContainerWindowViewModel pmcContainer = TestViewModel(true, applicationManager);
            pmcContainer.OpenContainer.Execute(pmcContainer.Items.FirstOrDefault());
            Assert.That(applicationManager.ContainerWindowOpened, Is.True);
        }

        [Test]
        public void CanInspectWeapon()
        {
            var applicationManager = new TestsApplicationManager();
            ContainerWindowViewModel pmcContainer = TestViewModel(true, applicationManager);
            pmcContainer.InspectWeapon.Execute(pmcContainer.Items.FirstOrDefault());
            Assert.That(applicationManager.WeaponBuildWindowOpened, Is.True);
        }

        [Test]
        public void CanRemoveItem()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(true);
            var item = pmcContainer.Items[0];
            pmcContainer.RemoveItem.Execute(item.Id);
            Assert.That(pmcContainer.Items.IndexOf(item), Is.EqualTo(-1), "Item not removed");
        }

        [Test]
        public void CanRemoveAllItems()
        {
            ContainerWindowViewModel pmcContainer = TestViewModel(true);
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
            ContainerWindowViewModel pmcContainer = new(container,
                                                        AppData.Profile.Characters.Pmc.Inventory,
                                                        null,
                                                        null,
                                                        dialogManager,
                                                        worker);
            Assert.That(pmcContainer.ItemsAddingAllowed, Is.True, "Items adding not allowed for opened container");
            Assert.That(pmcContainer.ItemsAddingBlocked, Is.False, "Items adding blocked for opened container");
            var painkiller = AppData.ServerDatabase.ItemsDB["544fb37f4bdc2dee738b4567"];
            pmcContainer.RemoveAllItems.Execute(null);
            pmcContainer.AddItem.Execute(painkiller);
            Assert.That(pmcContainer.Items.Count, Is.EqualTo(1), "Item not added");
            Assert.That(pmcContainer.Items.Where(x => x.Tpl == "544fb37f4bdc2dee738b4567").FirstOrDefault(), Is.Not.Null, "Item not added");
        }

        [Test]
        public void CanShowAllItems()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var container = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.IsContainer && x.CanAddItems).FirstOrDefault();
            TestsDialogManager dialogManager = new();
            ContainerWindowViewModel pmcContainer = new(container,
                                                        AppData.Profile.Characters.Pmc.Inventory,
                                                        null,
                                                        null,
                                                        dialogManager,
                                                        null);
            pmcContainer.ShowAllItems.Execute(null);
            Assert.That(dialogManager.AllItemsDialogOpened, Is.True, "AllItems dialog not showed");
        }

        private static void InitializeCorrectly(ContainerWindowViewModel container, int expectedCount)
        {
            Assert.Multiple(() =>
            {
                Assert.That(container, Is.Not.Null, "ContainerWindowViewModel is null");
                Assert.That(container.WindowTitle, Is.EqualTo(BackpackName), "Wrong WindowTitle");
                Assert.That(container.HasItems, Is.True, "HasItems is false");
                Assert.That(container.Items, Is.Not.Null, "Items is not null");
                Assert.That(container.Items.Count, Is.EqualTo(expectedCount), $"Items.Count is not {expectedCount}");
                Assert.That(container.ItemsAddingAllowed, Is.True, "ItemsAddingAllowed is false");
                Assert.That(container.CategoriesForItemsAdding.Count, Is.GreaterThan(0), "CategoriesForItemsAdding is empty");
            });
        }

        private static ContainerWindowViewModel TestViewModel(bool isPmcItem,
                                                              IApplicationManager applicationManager = null)
        {
            TestHelpers.SetupTestCharacters("ContainerWindowViewModel");
            InventoryItem item = new()
            {
                Id = TestHelpers.GetTestName("ContainerWindowViewModel", isPmcItem),
                Tpl = backpackTpl
            };
            return new(item,
                       isPmcItem ? AppData.Profile.Characters.Pmc.Inventory : AppData.Profile.Characters.Scav.Inventory,
                       null,
                       applicationManager,
                       dialogManager,
                       worker);
        }
    }
}