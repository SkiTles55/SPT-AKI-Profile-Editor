using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class MainWindowViewModelTests
    {
        private static readonly TestsDialogManager dialogManager = new();
        private MainWindowViewModel _viewModel;

        [OneTimeSetUp]
        public void Setup() => _viewModel = new(dialogManager);

        [Test]
        public void InitializeCorrectly()
        {
            Assert.That(_viewModel, Is.Not.Null, "MainWindowViewModel is null");
            Assert.That(App.DialogCoordinator, Is.Not.Null, "DialogCoordinator is null");
            Assert.That(App.Worker, Is.Not.Null, "Worker is null");
            Assert.That(MainWindowViewModel.WindowTitle, Is.Not.Empty, "WindowTitle is empty");
            Assert.AreEqual(_viewModel, MainWindowViewModel.Instance, "MainWindowViewModel.Instance is not MainWindowViewModel");
        }

        [Test]
        public void CanOpenCloseFastMode()
        {
            MainWindowViewModel.OpenFastModeCommand.Execute(null);
            Assert.That(AppData.AppSettings.FastModeOpened, Is.True, "FastModeOpened is false");
            MainWindowViewModel.OpenFastModeCommand.Execute(null);
            Assert.That(AppData.AppSettings.FastModeOpened, Is.False, "FastModeOpened is true");
        }
    }
}