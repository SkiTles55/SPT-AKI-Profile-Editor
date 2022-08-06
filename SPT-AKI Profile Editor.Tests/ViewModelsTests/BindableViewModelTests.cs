using NUnit.Framework;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class BindableViewModelTests
    {
        [Test]
        public void HasNeededData()
        {
            Assert.That(BindableViewModel.AppLocalization, Is.Not.Null);
            Assert.That(BindableViewModel.ServerDatabase, Is.Not.Null);
            Assert.That(BindableViewModel.Profile, Is.Not.Null);
            Assert.That(BindableViewModel.GridFilters, Is.Not.Null);
        }
    }
}