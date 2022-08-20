using NUnit.Framework;
using SPT_AKI_Profile_Editor.Views;
using SPT_AKI_Profile_Editor.Views.ExtendedControls;
using System.Threading;

namespace SPT_AKI_Profile_Editor.Tests
{
    [Apartment(ApartmentState.STA)]
    internal class UserControlsTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var app = new App();
            app.InitializeComponent();
        }

        [Test]
        public void AboutTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new AboutTab());

        [Test]
        public void BackupsTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new BackupsTab());

        [Test]
        public void ClothingTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new ClothingTab());

        [Test]
        public void ExaminedItemsTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new ExaminedItemsTab());

        [Test]
        public void EquipmentConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new Equipment());

        [Test]
        public void InventoryConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new Inventory());

        [Test]
        public void ItemsAddingConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new ItemsAdding());

        [Test]
        public void SkillGridConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new SkillGrid());

        [Test]
        public void WeaponBuildsListConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new WeaponBuildsList());

        [Test]
        public void WeaponBuildViewConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new WeaponBuildView());

        [Test]
        public void FastModeConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new FastMode());

        [Test]
        public void HideoutTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new HideoutTab());

        [Test]
        public void InfoTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new InfoTab());

        [Test]
        public void IssuesDialogConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new IssuesDialog());

        [Test]
        public void LocalizationEditorConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new LocalizationEditor());

        [Test]
        public void MasteringTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new MasteringTab());

        [Test]
        public void MerchantsTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new MerchantsTab());

        [Test]
        public void MoneyDailogConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new MoneyDailog());

        [Test]
        public void NoDataPanelConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new NoDataPanel());

        [Test]
        public void QuestsTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new QuestsTab());

        [Test]
        public void ServerPathEditorConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new ServerPathEditor());

        [Test]
        public void SkillsTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new SkillsTab());

        [Test]
        public void StashTabConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new StashTab());

        [Test]
        public void WeaponBuildsConstructorDoesNotThrow() => Assert.DoesNotThrow(() => new WeaponBuilds());
    }
}