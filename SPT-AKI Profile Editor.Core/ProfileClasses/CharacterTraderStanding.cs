using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterTraderStanding : BindableEntity
    {
        [JsonProperty("loyaltyLevel")]
        public int LoyaltyLevel
        {
            get => loyaltyLevel;
            set
            {
                loyaltyLevel = value;
                OnPropertyChanged("LoyaltyLevel");
            }
        }
        [JsonProperty("salesSum")]
        public long SalesSum
        {
            get => salesSum;
            set
            {
                salesSum = value;
                OnPropertyChanged("SalesSum");
            }
        }
        [JsonProperty("standing")]
        public float Standing
        {
            get => standing;
            set
            {
                standing = value;
                OnPropertyChanged("Standing");
            }
        }
        [JsonProperty("unlocked")]
        public bool Unlocked
        {
            get => unlocked;
            set
            {
                unlocked = value;
                OnPropertyChanged("Unlocked");
            }
        }

        private int loyaltyLevel;
        private long salesSum;
        private float standing;
        private bool unlocked;
    }
}