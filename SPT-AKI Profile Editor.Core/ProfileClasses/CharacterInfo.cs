using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterInfo : INotifyPropertyChanged
    {
        [JsonPropertyName("Nickname")]
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                if (nickname != value)
                {
                    LowerNickname = value.ToLower();
                }
            }
        }

        [JsonPropertyName("LowerNickname")]
        public string LowerNickname { get; set; }

        [JsonPropertyName("Side")]
        public string Side
        {
            get => side;
            set
            {
                side = value;
            }
        }

        [JsonPropertyName("Voice")]
        public string Voice
        {
            get => voice;
            set
            {
                voice = value;
            }
        }

        [JsonPropertyName("Level")]
        public int Level
        {
            get => level;
            set
            {
                level = value;
            }
        }

        [JsonPropertyName("Experience")]
        public long Experience
        {
            get => experience;
            set
            {
                experience = value;
            }
        }

        [JsonPropertyName("GameVersion")]
        public string GameVersion { get; set; }

        private string nickname;
        private string side;
        private string voice;
        private int level;
        private long experience;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}