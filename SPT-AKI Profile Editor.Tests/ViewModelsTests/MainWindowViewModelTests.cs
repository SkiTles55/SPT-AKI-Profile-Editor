using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class MainWindowViewModelTests
    {
        private MainWindowViewModel _viewModel;

        [OneTimeSetUp]
        public void Setup() => _viewModel = new();

        [Test]
        public void InitializeCorrectly()
        {
            Assert.That(_viewModel, Is.Not.Null);
            Assert.That(App.DialogCoordinator, Is.Not.Null);
            Assert.That(App.Worker, Is.Not.Null);
            Assert.That(MainWindowViewModel.WindowTitle, Is.Not.Empty);
            Assert.AreEqual(_viewModel, MainWindowViewModel.Instance);
        }

        [Test]
        public void CanOpenCloseFastMode()
        {
            MainWindowViewModel.OpenFastModeCommand.Execute(null);
            Assert.That(AppData.AppSettings.FastModeOpened, Is.True);
            MainWindowViewModel.OpenFastModeCommand.Execute(null);
            Assert.That(AppData.AppSettings.FastModeOpened, Is.False);
        }
    }
}