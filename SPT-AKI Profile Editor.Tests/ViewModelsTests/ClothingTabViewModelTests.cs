using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class ClothingTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            ClothingTabViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void CanAcquireAll()
        {
            TestConstants.LoadDatabaseAndProfile();
            ClothingTabViewModel.AcquireAllCommand.Execute(null);
            Assert.That(AppData.ServerDatabase.TraderSuits.All(x => x.Boughted), Is.True);
        }
    }
}