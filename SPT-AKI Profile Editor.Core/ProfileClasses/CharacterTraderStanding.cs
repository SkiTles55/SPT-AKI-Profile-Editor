using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterTraderStanding : INotifyPropertyChanged
    {
        [JsonPropertyName("loyaltyLevel")]
        public int LoyaltyLevel
        {
            get => loyaltyLevel;
            set
            {
                loyaltyLevel = value;
                OnPropertyChanged("LoyaltyLevel");
            }
        }
        [JsonPropertyName("salesSum")]
        public long SalesSum
        {
            get => salesSum;
            set
            {
                salesSum = value;
                OnPropertyChanged("SalesSum");
            }
        }
        [JsonPropertyName("standing")]
        public float Standing
        {
            get => standing;
            set
            {
                standing = value;
                OnPropertyChanged("Standing");
            }
        }
        [JsonPropertyName("unlocked")]
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}