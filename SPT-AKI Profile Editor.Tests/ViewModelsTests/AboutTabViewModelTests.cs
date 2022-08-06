using NUnit.Framework;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class AboutTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            AboutTabViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
        }
        [Test]
        public void HasNeededData()
        {
            Assert.That(AboutTabViewModel.AppSettings, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(AboutTabViewModel.RepositoryURL), Is.False);
            Assert.That(string.IsNullOrEmpty(AboutTabViewModel.AuthorURL), Is.False);
        }
    }
}