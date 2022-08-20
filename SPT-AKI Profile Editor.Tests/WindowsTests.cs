using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Threading;

namespace SPT_AKI_Profile_Editor.Tests
{
    [Apartment(ApartmentState.STA)]
    internal class WindowsTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.SetupApp();

        [Test]
        public void MainWindowConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new MainWindow());

        [Test]
        public void WeaponBuildWindowConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new WeaponBuildWindow(TestInventoryItem(StashEditMode.PMC), StashEditMode.PMC));

        [Test]
        public void ContainerWindowConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new ContainerWindow(TestInventoryItem(StashEditMode.PMC), StashEditMode.PMC));

        private static InventoryItem TestInventoryItem(StashEditMode editMode) => new()
        {
            Id = TestHelpers.GetTestName("WeaponBuildWindow", editMode),
            Tpl = TestHelpers.GetTestName("WeaponBuildWindow", editMode)
        };
    }
}