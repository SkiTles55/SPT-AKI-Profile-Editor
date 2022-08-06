using NUnit.Framework;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class InfoTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            InfoTabViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void HasNeededData()
        {
            Assert.That(InfoTabViewModel.Sides, Is.Not.Empty);
        }
    }
}