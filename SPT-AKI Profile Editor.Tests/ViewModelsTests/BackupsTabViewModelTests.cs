using NUnit.Framework;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class BackupsTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            BackupsTabViewModel viewModel = new(new TestsDialogManager());
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void HasNeededData() => Assert.That(BackupsTabViewModel.BackupService, Is.Not.Null);
    }
}