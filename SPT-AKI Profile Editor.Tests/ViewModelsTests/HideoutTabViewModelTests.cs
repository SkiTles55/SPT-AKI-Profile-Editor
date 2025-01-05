using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class HideoutTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            HideoutTabViewModel viewModel = new(new TestsDialogManager());
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void CanSetAllAreasMax()
        {
            TestHelpers.LoadDatabaseAndProfile();
            HideoutTabViewModel.SetAllMaxCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Areas.All(x => x.Level == x.MaxLevel),
                        Is.True);
        }

        [Test]
        public void CanAddAllCrafts()
        {
            TestHelpers.LoadDatabaseAndProfile();
            HideoutTabViewModel.AddAllCrafts.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions.All(x => x.Added), Is.True);
        }

        [Test]
        public void CanFinishCraft()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var craftId = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault(x => !x.IsFinished)?.RecipeId;
            Assert.That(craftId, Is.Not.Null, "Started craft not founded");
            HideoutTabViewModel viewModel = new(new TestsDialogManager());
            viewModel.SetCraftFinishedCommand.Execute(craftId);
            var craft = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault(x => x.RecipeId == craftId);
            Assert.That(craft, Is.Not.Null, "Edited craft not founded");
            Assert.That(craft.IsFinished, Is.True, "Craft not finished");
        }

        [Test]
        public void CanFinishAllCrafts()
        {
            TestHelpers.LoadDatabaseAndProfile();
            HideoutTabViewModel viewModel = new(new TestsDialogManager());
            viewModel.SetAllCraftsFinishedCommand.Execute(null);
            var haveUnfinished = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.Where(x => !x.IsFinished).Any();
            Assert.That(haveUnfinished, Is.False, "Profile have unfinished crafts");
        }

        [Test]
        public void CanRemeoveCraft()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var craftId = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault()?.RecipeId;
            Assert.That(craftId, Is.Not.Null, "Craft not founded");
            HideoutTabViewModel viewModel = new(new TestsDialogManager());
            viewModel.RemoveStartedCraftCommand.Execute(craftId);
            var craft = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault(x => x.RecipeId == craftId);
            Assert.That(craft, Is.Null, "Craft not removed");
        }
    }
}