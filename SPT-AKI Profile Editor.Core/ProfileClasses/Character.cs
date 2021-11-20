using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Character : INotifyPropertyChanged
    {
        [JsonProperty("aid")]
        public string Aid
        {
            get => aid;
            set
            {
                aid = value;
                OnPropertyChanged("Aid");
            }
        }
        [JsonProperty("Info")]
        public CharacterInfo Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
        [JsonProperty("Customization")]
        public CharacterCustomization Customization
        {
            get => customization;
            set
            {
                customization = value;
                OnPropertyChanged("Customization");
            }
        }
        [JsonProperty("TradersInfo")]
        public Dictionary<string, CharacterTraderStanding> TraderStandings
        {
            get => traderStandings;
            set
            {
                traderStandings = value;
                OnPropertyChanged("TraderStandings");
            }
        }
        [JsonProperty("Hideout")]
        public CharacterHideout Hideout
        {
            get => hideout;
            set
            {
                hideout = value;
                OnPropertyChanged("Hideout");
            }
        }
        [JsonProperty("Quests")]
        public CharacterQuest[] Quests
        {
            get => quests;
            set
            {
                quests = value;
                OnPropertyChanged("Quests");
            }
        }
        [JsonProperty("Skills")]
        public CharacterSkills Skills
        {
            get => skills;
            set
            {
                skills = value;
                OnPropertyChanged("Skills");
            }
        }

        public void SetAllQuests(string status)
        {
            foreach (var quest in Quests)
                quest.Status = status;
        }

        public void SetAllHideoutAreasMax()
        {
            foreach (var area in Hideout.Areas)
                area.Level = area.MaxLevel;
        }

        public void SetAllCommonSkills(float value)
        {
            foreach (var skill in Skills.Common)
                if (!skill.Id.StartsWith("Bot"))
                    skill.Progress = value;
        }

        public void SetAllMasteringsSkills(float value)
        {
            foreach (var skill in Skills.Mastering)
                skill.Progress = value;
        }

        private string aid;
        private CharacterInfo info;
        private CharacterCustomization customization;
        private Dictionary<string, CharacterTraderStanding> traderStandings;
        private CharacterHideout hideout;
        private CharacterQuest[] quests;
        private CharacterSkills skills;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}