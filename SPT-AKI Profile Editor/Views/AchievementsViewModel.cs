using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class AchievementsViewModel : PmcBindableViewModel
    {
        private ObservableCollection<CharacterAchievement> achievements = [];
        private string achievementFilter;
        private string rarityFilter;

        public static RelayCommand ReceiveAllCommand => new(obj => Profile.Characters?.Pmc?.ReceiveAllAchievements());

        public ObservableCollection<CharacterAchievement> Achievements
        {
            get => achievements;
            set
            {
                achievements = value;
                OnPropertyChanged(nameof(Achievements));
                OnPropertyChanged(nameof(CanReceiveAll));
            }
        }

        public bool CanReceiveAll => Achievements.Any(x => !x.IsReceived);

        public string AchievementFilter
        {
            get => achievementFilter;
            set
            {
                achievementFilter = value;
                OnPropertyChanged(nameof(AchievementFilter));
                ApplyFilter();
            }
        }

        public string RarityFilter
        {
            get => rarityFilter;
            set
            {
                rarityFilter = value;
                OnPropertyChanged(nameof(RarityFilter));
                ApplyFilter();
            }
        }

        public override void ApplyFilter()
        {
            ObservableCollection<CharacterAchievement> filteredItems;
            if (Profile?.Characters?.Pmc?.AllAchievements == null || Profile.Characters.Pmc.AllAchievements.Count == 0)
                filteredItems = [];
            else
            {
                var values = Profile.Characters.Pmc.AllAchievements;
                if (string.IsNullOrEmpty(AchievementFilter) && string.IsNullOrEmpty(RarityFilter))
                    filteredItems = new(values);
                else
                    filteredItems = new(values.Where(x => CanShow(AchievementFilter,
                                                                  x.LocalizedName,
                                                                  x.LocalizedDescription,
                                                                  RarityFilter,
                                                                  x.Rarity)));
            }
            Achievements = filteredItems;
        }

        private static bool CanShow(string achievementFilter, string achievementName, string achievementDescription, string rarityFilter, string rarityName)
            => (string.IsNullOrEmpty(achievementFilter) || (achievementName + achievementDescription).Contains(achievementFilter, StringComparison.CurrentCultureIgnoreCase))
            && (string.IsNullOrEmpty(rarityFilter) || rarityName.Contains(rarityFilter, StringComparison.CurrentCultureIgnoreCase));
    }
}