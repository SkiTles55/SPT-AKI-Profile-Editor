using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class ExaminedItemsTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            ExaminedItemsTabViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void CanExamineAll()
        {
            TestConstants.LoadDatabaseAndProfile();
            var expected = AppData.Profile.Characters.Pmc.ExaminedItems.Count();
            ExaminedItemsTabViewModel.ExamineAllCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.ExaminedItems.Count(), Is.Not.EqualTo(expected));
        }
    }
}