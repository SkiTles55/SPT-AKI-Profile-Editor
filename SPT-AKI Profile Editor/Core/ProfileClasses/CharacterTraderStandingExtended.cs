using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterTraderStandingExtended : TemplateableEntity
    {
        private int loyaltyLevel;
        private float standing;
        private long salesSum;
        private bool unlocked;

        public CharacterTraderStandingExtended(CharacterTraderStanding standing, string id, TraderBase traderBase, float ragfairRating)
        {
            TraderStanding = standing;
            Standing = id == AppData.AppSettings.RagfairTraderId ? ragfairRating : standing.Standing;
            LoyaltyLevel = standing.LoyaltyLevel;
            SalesSum = standing.SalesSum;
            Unlocked = standing.Unlocked;
            Id = id;
            TraderBase = traderBase;
            LoadBitmapImage();
        }

        public string Id { get; }
        public CharacterTraderStanding TraderStanding { get; }
        public TraderBase TraderBase { get; }
        public BitmapImage BitmapImage { get; private set; }

        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.ContainsKey(Id.Nickname()) ? AppData.ServerDatabase.LocalesGlobal[Id.Nickname()] : Id;

        public int LoyaltyLevel
        {
            get => loyaltyLevel;
            set
            {
                value = Math.Min(Math.Max(value, 1), MaxLevel);
                SetProperty(nameof(LoyaltyLevel), ref loyaltyLevel, value);
                TraderStanding.LoyaltyLevel = value;
                SetUnlocked(value);
                SetStanding(value);
                SetSalesSum(value);
            }
        }

        public bool IsLoyaltyLevelChanged => changedValues.ContainsKey(nameof(LoyaltyLevel));

        public float Standing
        {
            get => standing;
            set
            {
                SetProperty(nameof(Standing), ref standing, value);
                TraderStanding.Standing = value;
                if (Id == AppData.AppSettings.RagfairTraderId)
                    AppData.Profile.Characters.Pmc.RagfairInfo.Rating = value;
                SetLevel();
            }
        }

        public bool IsStandingChanged => changedValues.ContainsKey(nameof(Standing));

        public long SalesSum
        {
            get => salesSum;
            set
            {
                SetProperty(nameof(SalesSum), ref salesSum, value);
                TraderStanding.SalesSum = value;
                SetLevel();
            }
        }

        public bool IsSalesSumChanged => changedValues.ContainsKey(nameof(SalesSum));

        public bool Unlocked
        {
            get => unlocked;
            set => SetProperty(nameof(Unlocked), ref unlocked, value);
        }

        public bool IsUnlockedChanged => changedValues.ContainsKey(nameof(Unlocked));

        public int MaxLevel => TraderBase?.LoyaltyLevels.Count ?? 0;

        public override string TemplateEntityId => Id;

        public override string TemplateLocalizedName => LocalizedName;

        private int LevelStart => IsLoyaltyLevelChanged ? (int)startValues[nameof(LoyaltyLevel)] : TraderStanding.LoyaltyLevel;

        public bool HasLevelIssue(int? level)
        {
            var currentLevelIndex = Math.Min(Math.Max(0, LoyaltyLevel - 1), MaxLevel - 1);
            return TraderBase?.LoyaltyLevels[currentLevelIndex].MinLevel > level;
        }

        private void SetSalesSum(int level)
        {
            var minSalesSum = TraderBase?.LoyaltyLevels[level - 1].MinSalesSum ?? TraderStanding.SalesSum;
            var salesSumStart = IsSalesSumChanged ? (long)startValues[nameof(SalesSum)] : TraderStanding.SalesSum;
            var newValue = level >= LevelStart ? Math.Max(minSalesSum, salesSumStart) : Math.Min(minSalesSum, salesSumStart);
            if (SalesSum != newValue)
                SalesSum = newValue;
        }

        private void SetStanding(int level)
        {
            var minStanding = TraderBase?.LoyaltyLevels[level - 1].MinStanding ?? TraderStanding.Standing;
            var staindingStart = IsStandingChanged ? (float)startValues[nameof(Standing)] : TraderStanding.Standing;
            var newValue = level >= LevelStart ? Math.Max(minStanding, staindingStart) : Math.Min(minStanding, staindingStart);
            if (Standing != newValue)
                Standing = newValue;
        }

        private void SetUnlocked(int level)
        {
            var unlockedStart = IsUnlockedChanged ? (bool)startValues[nameof(Unlocked)] : TraderStanding.Unlocked;
            Unlocked = level > 1 || unlockedStart;
        }

        private void SetLevel()
        {
            if (TraderBase?.LoyaltyLevels == null)
                return;

            int newLevel = 0;
            foreach (var level in TraderBase.LoyaltyLevels)
            {
                if (level.MinStanding <= Standing && level.MinSalesSum <= SalesSum)
                    newLevel++;
                else
                    break;
            }

            if (newLevel != 0 && LoyaltyLevel != newLevel)
                LoyaltyLevel = newLevel;
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