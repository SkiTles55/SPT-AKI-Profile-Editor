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
                    OnPropertyChanged("LowerNickname");
                }
                OnPropertyChanged("Nickname");
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
                OnPropertyChanged("Side");
            }
        }

        [JsonPropertyName("Voice")]
        public string Voice
        {
            get => voice;
            set
            {
                voice = value;
                OnPropertyChanged("Voice");
            }
        }

        [JsonPropertyName("Level")]
        public int Level
        {
            get => level;
            set
            {
                level = value;
                OnPropertyChanged("Level");
                Experience = LevelToExperience(level);
            }
        }

        [JsonPropertyName("Experience")]
        public long Experience
        {
            get => experience;
            set
            {
                experience = value;
                OnPropertyChanged("Experience");
            }
        }

        [JsonPropertyName("GameVersion")]
        public string GameVersion { get; set; }

        private string nickname;
        private string side;
        private string voice;
        private int level;
        private long experience;

        private long LevelToExperience(int level)
        {
            //if (ExtMethods.ExpTable == null) return 0;
            //if (level > ExtMethods.ExpTable.Count())
            //    level = ExtMethods.ExpTable.Count();
            long exp = 0;
            //for (int i = 0; i < level; i++)
            //    exp += ExtMethods.ExpTable[i];
            return exp;
        }

        private int ExperienceToLevel(long experience)
        {
            //if (ExtMethods.ExpTable == null) return 0;
            //long exp = 0;
            int level = 0;
            //for (int i = 0; i < ExtMethods.ExpTable.Count(); i++)
            //{
            //    if (experience < exp)
            //        break;
            //    exp += ExtMethods.ExpTable[i];
            //    level = i;
            //}
            return level;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}