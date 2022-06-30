using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class WeaponBuildWindowViewModelTests
    {
        private WeaponBuildWindowViewModel _viewModel;

        [OneTimeSetUp]
        public void Setup()
        {
            InventoryItem item = new()
            {
                Id = "WeaponBuildWindowViewModel_Test",
                Tpl = "WeaponBuildWindowViewModel_Test"
            };
            _viewModel = new(item, StashEditMode.PMC, App.DialogCoordinator);
        }

        [Test]
        public void InitializeCorrectly()
        {
            Assert.That(_viewModel, Is.Not.Null);
            Assert.That(_viewModel.Worker, Is.Not.Null);
            Assert.AreEqual(_viewModel.WindowTitle, "WeaponBuildWindowViewModel_Test");
            Assert.That(_viewModel.WeaponBuild, Is.Not.Null);
        }
    }
}