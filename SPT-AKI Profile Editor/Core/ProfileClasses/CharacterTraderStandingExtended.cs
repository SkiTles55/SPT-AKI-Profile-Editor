using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterTraderStandingExtended : BindableEntity
    {
        private long? salesSumStart;
        private float? staindingStart;
        private bool? unlockedStart;
        private int? levelStart;

        public CharacterTraderStandingExtended(CharacterTraderStanding standing, string id, TraderBase traderBase)
        {
            TraderStanding = standing;
            Id = id;
            TraderBase = traderBase;
            LoadBitmapImage();
        }

        public string Id { get; }

        public CharacterTraderStanding TraderStanding { get; }
        public TraderBase TraderBase { get; }
        public BitmapImage BitmapImage { get; set; }

        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.ContainsKey(Id.Nickname()) ? AppData.ServerDatabase.LocalesGlobal[Id.Nickname()] : Id;

        public int LoyaltyLevel
        {
            get => TraderStanding.LoyaltyLevel;
            set
            {
                if (levelStart == null)
                    levelStart = TraderStanding.LoyaltyLevel;
                value = Math.Min(Math.Max(value, 1), MaxLevel);
                TraderStanding.LoyaltyLevel = value;
                OnPropertyChanged("LoyaltyLevel");
                SetSalesSum(value);
                SetStanding(value);
                SetUnlocked(value);
            }
        }

        public float Standing
        {
            get => TraderStanding.Standing;
            set
            {
                TraderStanding.Standing = value;
                OnPropertyChanged("Standing");
                UpdateLevel();
            }
        }

        public long SalesSum
        {
            get => TraderStanding.SalesSum;
            set
            {
                TraderStanding.SalesSum = value;
                OnPropertyChanged("SalesSum");
                UpdateLevel();
            }
        }

        public int MaxLevel => TraderBase?.LoyaltyLevels.Count ?? 0;

        public bool HasLevelIssue(int? level)
        {
            var currentLevelIndex = Math.Min(Math.Max(0, LoyaltyLevel - 1), MaxLevel - 1);
            return TraderBase?.LoyaltyLevels[currentLevelIndex].MinLevel > level;
        }

        private void UpdateLevel()
        {
            var nearestLevel = LoyaltyLevel;
            var checkedLevel = 1;
            foreach (var level in TraderBase?.LoyaltyLevels)
            {
                if (level.MinSalesSum <= SalesSum && level.MinStanding <= Standing)
                    nearestLevel = checkedLevel;
                checkedLevel++;
            }
            if (LoyaltyLevel != nearestLevel)
                LoyaltyLevel = nearestLevel;
        }

        private void SetSalesSum(int value)
        {
            if (salesSumStart == null)
                salesSumStart = TraderStanding.SalesSum;
            var minSalesSum = TraderBase?.LoyaltyLevels[value - 1].MinSalesSum + 100 ?? TraderStanding.SalesSum;
            var calculatedSum = value >= levelStart ? Math.Max(minSalesSum, salesSumStart.Value) : Math.Min(minSalesSum, salesSumStart.Value);
            TraderStanding.SalesSum = calculatedSum;
            OnPropertyChanged("SalesSum");
        }

        private void SetStanding(int value)
        {
            if (staindingStart == null)
                staindingStart = TraderStanding.Standing;
            var minStanding = TraderBase?.LoyaltyLevels[value - 1].MinStanding + 0.02f ?? TraderStanding.Standing;
            var calculatedSTanding = value >= levelStart ? Math.Max(minStanding, staindingStart.Value) : Math.Min(minStanding, staindingStart.Value);
            TraderStanding.Standing = calculatedSTanding;
            OnPropertyChanged("Standing");
        }

        private void SetUnlocked(int value)
        {
            if (unlockedStart == null)
                unlockedStart = TraderStanding.Unlocked;
            TraderStanding.Unlocked = value > 1 || unlockedStart.Value;
        }

        private void LoadBitmapImage()
        {
            var imageUrl = TraderBase?.ImageUrl ?? "unknown.png";

            var imagePath = Path.Combine(AppData.AppSettings.ServerPath,
                AppData.AppSettings.DirsList[SPTServerDir.traderImages],
                Path.GetFileNameWithoutExtension(imageUrl) + ".png");

            if (File.Exists(imagePath))
            {
                try
                {
                    BitmapImage = new BitmapImage(new Uri(imagePath));
                }
                catch { }
            }
        }
    }
}