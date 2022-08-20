using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class AboutTabViewModelTests
    {
        private readonly TestsApplicationManager _applicationManager = new();

        [Test]
        public void CanInitialize()
        {
            AboutTabViewModel viewModel = new(null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.OpenAutorGitHubUrl, Is.Not.Null);
            Assert.That(viewModel.OpenRepositoryGitHubUrl, Is.Not.Null);
            Assert.That(viewModel.OpenYoomoneyUrl, Is.Not.Null);
            Assert.That(viewModel.OpenSteamUrl, Is.Not.Null);
            Assert.That(viewModel.OpenSptAkiProjectUrl, Is.Not.Null);
        }

        [Test]
        public void HasNeededData()
        {
            Assert.That(AboutTabViewModel.AppSettings, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(AboutTabViewModel.RepositoryURL), Is.False);
            Assert.That(string.IsNullOrEmpty(AboutTabViewModel.AuthorURL), Is.False);
            Assert.That(string.IsNullOrEmpty(AboutTabViewModel.YoomoneyUrl), Is.False);
            Assert.That(string.IsNullOrEmpty(AboutTabViewModel.SptAkiProjectUrl), Is.False);
        }

        [Test]
        public void CanOpenAutorGitHubUrl()
        {
            _applicationManager.LastOpenedUrl = null;
            AboutTabViewModel viewModel = new(_applicationManager);
            viewModel.OpenAutorGitHubUrl.Execute(null);
            Assert.That(_applicationManager.LastOpenedUrl, Is.EqualTo(AboutTabViewModel.AuthorURL));
        }

        [Test]
        public void CanOpenRepositoryGitHubUrl()
        {
            _applicationManager.LastOpenedUrl = null;
            AboutTabViewModel viewModel = new(_applicationManager);
            viewModel.OpenRepositoryGitHubUrl.Execute(null);
            Assert.That(_applicationManager.LastOpenedUrl, Is.EqualTo(AboutTabViewModel.RepositoryURL));
        }

        [Test]
        public void CanOpenYoomoneyUrl()
        {
            _applicationManager.LastOpenedUrl = null;
            AboutTabViewModel viewModel = new(_applicationManager);
            viewModel.OpenYoomoneyUrl.Execute(null);
            Assert.That(_applicationManager.LastOpenedUrl, Is.EqualTo(AboutTabViewModel.YoomoneyUrl));
        }

        [Test]
        public void CanOpenSteamUrl()
        {
            _applicationManager.LastOpenedUrl = null;
            AboutTabViewModel viewModel = new(_applicationManager);
            viewModel.OpenSteamUrl.Execute(null);
            Assert.That(_applicationManager.LastOpenedUrl, Is.EqualTo(AppData.AppSettings.steamTradeUrl));
        }

        [Test]
        public void CanOpenSptAkiProjectUrl()
        {
            _applicationManager.LastOpenedUrl = null;
            AboutTabViewModel viewModel = new(_applicationManager);
            viewModel.OpenSptAkiProjectUrl.Execute(null);
            Assert.That(_applicationManager.LastOpenedUrl, Is.EqualTo(AboutTabViewModel.SptAkiProjectUrl));
        }
    }
}