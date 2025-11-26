using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System;
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
            PrepareProfileWithStartedCrafts();
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
            PrepareProfileWithStartedCrafts();
            HideoutTabViewModel viewModel = new(new TestsDialogManager());
            viewModel.SetAllCraftsFinishedCommand.Execute(null);
            var haveUnfinished = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.Where(x => !x.IsFinished).Any();
            Assert.That(haveUnfinished, Is.False, "Profile have unfinished crafts");
        }

        [Test]
        public void CanRemoveStartedCraft()
        {
            PrepareProfileWithStartedCrafts();
            var craftId = AppData.Profile.Characters.Pmc.Hideout.Production?.Keys.FirstOrDefault();
            Assert.That(craftId, Is.Not.Null, "Started craft not founded");
            HideoutTabViewModel viewModel = new(new TestsDialogManager());
            viewModel.RemoveStartedCraftCommand.Execute(craftId);
            var craft = AppData.Profile.Characters.Pmc.Hideout.Production?.Keys.FirstOrDefault(x => x == craftId);
            Assert.That(craft, Is.Null, "Started craft not removed");
        }

        [Test]
        public void CanRemoveAllStartedCrafts()
        {
            PrepareProfileWithStartedCrafts();
            var craftId = AppData.Profile.Characters.Pmc.Hideout.Production?.Keys.FirstOrDefault();
            Assert.That(craftId, Is.Not.Null, "Started craft not founded");
            HideoutTabViewModel viewModel = new(new TestsDialogManager());
            viewModel.RemoveAllStartedCraftsCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Production?.Count, Is.Not.Null.And.Zero, "All strted crafts removed");
        }

        private static void PrepareProfileWithStartedCrafts()
        {
            TestHelpers.LoadDatabaseAndProfile();
            if (AppData.Profile.Characters.Pmc.Hideout.Production == null)
                AppData.Profile.Characters.Pmc.Hideout.Production = [];
            if (AppData.Profile.Characters.Pmc.Hideout.Production.Count > 0)
                return;
            var testProduction = new StartedHideoutProduction(recipeId: "5d1c819a86f7743f8362cf3f",
                                                              progress: 1200,
                                                              productionTime: 7200,
                                                              startTimestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 1200);
            AppData.Profile.Characters.Pmc.Hideout.Production.Add("test", testProduction);
        }
    }
}