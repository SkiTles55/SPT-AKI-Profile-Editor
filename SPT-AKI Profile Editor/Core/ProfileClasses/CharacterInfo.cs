using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterInfo : BindableEntity
    {
        private string nickname;

        private string side;

        private string voice;

        private int level;

        private long experience;

        [JsonProperty("Nickname")]
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                OnPropertyChanged("Nickname");
            }
        }

        [JsonProperty("Side")]
        public string Side
        {
            get => side;
            set
            {
                side = value;
                OnPropertyChanged("Side");
            }
        }

        [JsonProperty("Voice")]
        public string Voice
        {
            get => voice;
            set
            {
                voice = value;
                OnPropertyChanged("Voice");
            }
        }

        [JsonProperty("Level")]
        public int Level
        {
            get => level;
            set
            {
                if (level != value)
                {
                    level = value;
                    OnPropertyChanged("Level");
                    Experience = LevelToExperience();
                }
            }
        }

        [JsonProperty("Experience")]
        public long Experience
        {
            get => experience;
            set
            {
                if (experience != value)
                {
                    experience = value;
                    OnPropertyChanged("Experience");
                    Level = ExperienceToLevel();
                }
            }
        }

        [JsonProperty("GameVersion")]
        public string GameVersion { get; set; }

        private long LevelToExperience()
        {
            if (AppData.ServerDatabase.ServerGlobals?.Config?.Exp?.Level?.ExpTable == null) return 1;
            if (level == 0)
            {
                level = 1;
                Level = level;
            }
            if (level > AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable.Length)
            {
                level = AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable.Length;
                Level = level;
            }
            long expStart = 0;
            long expEnd = 0;
            for (int i = 0; i < level; i++)
            {
                expStart += AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable[i].Exp;
                expEnd = expStart;
                if (i < AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable.Length - 1)
                    expEnd += AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable[i + 1].Exp;
                else
                    expEnd += 100000;
            }
            if (experience >= expStart && experience < expEnd)
                return experience;
            else
                return expStart;
        }

        private int ExperienceToLevel()
        {
            if (AppData.ServerDatabase.ServerGlobals?.Config?.Exp?.Level?.ExpTable == null) return 0;
            long exp = 0;
            int level = 0;
            for (int i = 0; i < AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable.Length; i++)
            {
                if (experience < exp)
                    break;
                exp += AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable[i].Exp;
                level = i;
            }
            return level;
        }
    }
}