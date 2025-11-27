using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class AchievementsViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            AchievementsViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void CanReceiveAllAchievements()
        {
            PrepareProfileWithUnreceivedAchievements();
            AchievementsViewModel.ReceiveAllCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements.All(x => x.IsReceived),
                        Is.True, "All achievements should be received");
        }

        [Test]
        public void CanReceiveAllPropertyReturnsTrueWhenThereAreUnreceivedAchievements()
        {
            PrepareProfileWithUnreceivedAchievements();
            AchievementsViewModel viewModel = new();
            viewModel.ApplyFilter();
            Assert.That(viewModel.CanReceiveAll, Is.True,
                        "CanReceiveAll should be true when there are unreceived achievements");
        }

        [Test]
        public void CanReceiveAllPropertyReturnsFalseWhenAllAchievementsReceived()
        {
            PrepareProfileWithAllAchievementsReceived();
            AchievementsViewModel viewModel = new();
            viewModel.ApplyFilter();
            Assert.That(viewModel.CanReceiveAll, Is.False,
                        "CanReceiveAll should be false when all achievements are received");
        }

        [Test]
        public void CanFilterAchievementsByName()
        {
            PrepareProfileWithAchievements();
            AchievementsViewModel viewModel = new()
            {
                AchievementFilter = "Test Achievement"
            };
            viewModel.ApplyFilter();
            var filteredAchievements = viewModel.Achievements;
            Assert.That(filteredAchievements.All(x => x.LocalizedName.Contains("Test Achievement", System.StringComparison.CurrentCultureIgnoreCase)),
                        Is.True, "All filtered achievements should contain the filter text");
        }

        [Test]
        public void CanClearFilter()
        {
            PrepareProfileWithAchievements();
            AchievementsViewModel viewModel = new()
            {
                AchievementFilter = "Test Achievement"
            };
            viewModel.ApplyFilter();
            viewModel.AchievementFilter = string.Empty;
            Assert.That(viewModel.Achievements.Count,
                        Is.EqualTo(AppData.Profile.Characters.Pmc.AllAchievements.Count),
                        "After clearing filter, all achievements should be visible");
        }

        [Test]
        public void AchievementsCollectionUpdatesWhenFilterChanges()
        {
            PrepareProfileWithAchievements();
            AchievementsViewModel viewModel = new();
            viewModel.ApplyFilter();
            int initialCount = viewModel.Achievements.Count;
            viewModel.AchievementFilter = "NonExistentAchievement";
            Assert.That(viewModel.Achievements.Count, Is.LessThan(initialCount),
                        "Achievements count should decrease after applying filter");
        }

        private static void PrepareProfileWithUnreceivedAchievements()
        {
            TestHelpers.LoadDatabaseAndProfile();

            // Добавляем несколько неполученных достижений
            if (AppData.Profile.Characters.Pmc.AllAchievements.Count == 0)
            {
                Dictionary<string, long> achievements = [];
                AppData.Profile.Characters.Pmc.AllAchievements.Add(new CharacterAchievement("test_achievement_1", achievements, null, "all"));
                AppData.Profile.Characters.Pmc.AllAchievements.Add(new CharacterAchievement("test_achievement_2", achievements, null, "all"));
            }
            else
            {
                // Убеждаемся, что есть хотя бы одно неполученное достижение
                foreach (var achievement in AppData.Profile.Characters.Pmc.AllAchievements)
                {
                    achievement.IsReceived = false;
                }
            }
        }

        private static void PrepareProfileWithAllAchievementsReceived()
        {
            PrepareProfileWithAchievements();
            foreach (var achievement in AppData.Profile.Characters.Pmc.AllAchievements)
            {
                achievement.IsReceived = true;
            }
        }

        private static void PrepareProfileWithAchievements()
        {
            TestHelpers.LoadDatabaseAndProfile();

            if (AppData.Profile.Characters.Pmc.AllAchievements.Count == 0)
            {
                Dictionary<string, long> achievements = [];
                achievements.Add("test_achievement_2", 456897897446);
                AppData.Profile.Characters.Pmc.AllAchievements.Add(new CharacterAchievement("test_achievement_1", achievements, null, "all"));
                AppData.Profile.Characters.Pmc.AllAchievements.Add(new CharacterAchievement("test_achievement_2", achievements, null, "all"));
                AppData.Profile.Characters.Pmc.AllAchievements.Add(new CharacterAchievement("test_achievement_3", achievements, null, "all"));
            }
        }
    }
}