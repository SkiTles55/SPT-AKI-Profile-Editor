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
            AppData.AppSettings.ServerPath = TestConstants.serverPath;
            AppData.LoadDatabase();
            AppData.Profile.Load(TestConstants.profileFile);
            var expected = AppData.Profile.Characters.Pmc.ExaminedItems.Count();
            AppData.Profile.Characters.Pmc.ExamineAll();
            ExaminedItemsTabViewModel.ExamineAllCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.ExaminedItems.Count(), Is.Not.EqualTo(expected));
        }
    }
}