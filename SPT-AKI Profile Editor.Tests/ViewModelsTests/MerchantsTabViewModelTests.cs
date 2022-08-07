using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class MerchantsTabViewModelTests
    {
        [OneTimeSetUp]
        public void Setup() => TestConstants.LoadDatabaseAndProfile();

        [Test]
        public void CanInitialize()
        {
            MerchantsTabViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void HasNeededData() => Assert.That(MerchantsTabViewModel.SetAllMaxCommand, Is.Not.Null);

        [Test]
        public void CanExecuteSetAllMaxCommand()
        {
            MerchantsTabViewModel.SetAllMaxCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.All(x => x.LoyaltyLevel == x.MaxLevel), Is.True);
        }
    }
}