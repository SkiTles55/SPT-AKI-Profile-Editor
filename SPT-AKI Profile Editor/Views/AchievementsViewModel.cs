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

        public override void ApplyFilter()
        {
            ObservableCollection<CharacterAchievement> filteredItems;

            if (Profile?.Characters?.Pmc?.AllAchievements == null || Profile.Characters.Pmc.AllAchievements.Count == 0)
                filteredItems = [];
            else if (string.IsNullOrEmpty(AchievementFilter))
                filteredItems = new(Profile.Characters.Pmc.AllAchievements);
            else
                filteredItems = new(Profile.Characters.Pmc.AllAchievements.Where(x => x.LocalizedName.Contains(AchievementFilter, StringComparison.CurrentCultureIgnoreCase)));

            Achievements = filteredItems;
        }
    }
}