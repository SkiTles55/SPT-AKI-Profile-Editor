using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterTraderStandingExtended : BindableEntity
    {
        private long? salesSumStart;
        private float? staindingStart;
        private bool? unlockedStart;
        private int? levelStart;

        private int loyaltyLevel;

        public CharacterTraderStandingExtended(CharacterTraderStanding standing, string id, TraderBase traderBase)
        {
            TraderStanding = standing;
            loyaltyLevel = standing.LoyaltyLevel;
            Id = id;
            TraderBase = traderBase;
        }

        public string Id { get; }

        public CharacterTraderStanding TraderStanding { get; }
        public TraderBase TraderBase { get; }

        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.Trading.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal.Trading[Id].Nickname : Id;

        public int LoyaltyLevel
        {
            get => loyaltyLevel;
            set
            {
                if (levelStart == null)
                    levelStart = loyaltyLevel;
                value = Math.Min(Math.Max(value, 1), MaxLevel);
                loyaltyLevel = value;
                TraderStanding.LoyaltyLevel = value;
                OnPropertyChanged("LoyaltyLevel");
                SetSalesSum(value);
                SetStanding(value);
                SetUnlocked(value);
            }
        }

        public int MaxLevel => TraderBase?.LoyaltyLevels.Count ?? LoyaltyLevel;

        public bool HasLevelIssue(int? level) => TraderBase?.LoyaltyLevels?.Count > LoyaltyLevel - 1
            && TraderBase?.LoyaltyLevels[LoyaltyLevel - 1].MinLevel > level;

        private void SetSalesSum(int value)
        {
            if (salesSumStart == null)
                salesSumStart = TraderStanding.SalesSum;
            var minSalesSum = TraderBase?.LoyaltyLevels[value - 1].MinSalesSum + 100 ?? TraderStanding.SalesSum;
            TraderStanding.SalesSum = value >= levelStart ? Math.Max(minSalesSum, salesSumStart.Value) : Math.Min(minSalesSum, salesSumStart.Value);
        }

        private void SetStanding(int value)
        {
            if (staindingStart == null)
                staindingStart = TraderStanding.Standing;
            var minStanding = TraderBase?.LoyaltyLevels[value - 1].MinStanding + 0.02f ?? TraderStanding.Standing;
            TraderStanding.Standing = value >= levelStart ? Math.Max(minStanding, staindingStart.Value) : Math.Min(minStanding, staindingStart.Value);
        }

        private void SetUnlocked(int value)
        {
            if (unlockedStart == null)
                unlockedStart = TraderStanding.Unlocked;
            TraderStanding.Unlocked = value > 1 || unlockedStart.Value;
        }
    }
}