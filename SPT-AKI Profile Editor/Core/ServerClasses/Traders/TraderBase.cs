using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TraderBase : BindableEntity
    {
        private long? salesSumStart;

        private float? staindingStart;

        private bool? unlockedStart;

        private int? levelStart;

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("loyaltyLevels")]
        public List<LoyaltyLevels> LoyaltyLevels { get; set; }

        [JsonIgnore]
        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.Trading.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal.Trading[Id].Nickname : Id;

        [JsonIgnore]
        public bool Unlocked
        {
            get => AppData.Profile?.Characters?.Pmc?.TraderStandings?[Id].Unlocked ?? false;
            set
            {
                if (AppData.Profile?.Characters?.Pmc?.TraderStandings == null)
                    return;
                if (!AppData.Profile.Characters.Pmc.TraderStandings.ContainsKey(Id))
                    return;
                AppData.Profile.Characters.Pmc.TraderStandings[Id].Unlocked = value;
                OnPropertyChanged("Unlocked");
            }
        }

        [JsonIgnore]
        public int Level
        {
            get => AppData.Profile?.Characters?.Pmc?.TraderStandings?[Id].LoyaltyLevel ?? 1;
            set
            {
                if (AppData.Profile?.Characters?.Pmc?.TraderStandings == null)
                    return;
                if (!AppData.Profile.Characters.Pmc.TraderStandings.ContainsKey(Id))
                    return;
                if (levelStart == null)
                    levelStart = AppData.Profile.Characters.Pmc.TraderStandings[Id].LoyaltyLevel;
                value = Math.Min(Math.Max(value, 1), MaxLevel);
                AppData.Profile.Characters.Pmc.TraderStandings[Id].LoyaltyLevel = value;
                OnPropertyChanged("Level");
                if (salesSumStart == null)
                    salesSumStart = AppData.Profile.Characters.Pmc.TraderStandings[Id].SalesSum;
                AppData.Profile.Characters.Pmc.TraderStandings[Id].SalesSum = value >= levelStart ? Math.Max(LoyaltyLevels[value - 1].MinSalesSum + 100, salesSumStart.Value) : Math.Min(LoyaltyLevels[value - 1].MinSalesSum + 100, salesSumStart.Value);
                OnPropertyChanged("SalesSum");
                if (staindingStart == null)
                    staindingStart = AppData.Profile?.Characters?.Pmc?.TraderStandings?[Id].Standing ?? 0;
                AppData.Profile.Characters.Pmc.TraderStandings[Id].Standing = value >= levelStart ? Math.Max(LoyaltyLevels[value - 1].MinStanding + 0.02f, staindingStart.Value) : Math.Min(LoyaltyLevels[value - 1].MinStanding + 0.02f, staindingStart.Value);
                OnPropertyChanged("Standing");
                if (unlockedStart == null)
                    unlockedStart = AppData.Profile?.Characters?.Pmc?.TraderStandings?[Id].Unlocked ?? false;
                AppData.Profile.Characters.Pmc.TraderStandings[Id].Unlocked = value > 1 || unlockedStart.Value;
                OnPropertyChanged("Unlocked");
                OnPropertyChanged("IsLevelAvailable");
            }
        }

        [JsonIgnore]
        public int MaxLevel => LoyaltyLevels.Count;

        [JsonIgnore]
        public bool IsLevelAvailable => (AppData.Profile?.Characters?.Pmc?.Info?.Level ?? 0) >= LoyaltyLevels[Level - 1].MinLevel;

        [JsonIgnore]
        public string SalesSum => (AppData.Profile?.Characters?.Pmc?.TraderStandings?[Id].SalesSum ?? 0).ToString("N0");

        [JsonIgnore]
        public string Standing => GetFormatedStanding();

        private string GetFormatedStanding()
        {
            string value = (AppData.Profile?.Characters?.Pmc?.TraderStandings?[Id].Standing ?? 0).ToString();
            if (value.Length > 4)
                value = value.Remove(3);
            return value.Replace(",", ".");
        }
    }
}