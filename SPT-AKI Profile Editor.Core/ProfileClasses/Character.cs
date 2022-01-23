using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Character : BindableEntity
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
        [JsonProperty("Encyclopedia")]
        public Dictionary<string, bool> Encyclopedia
        {
            get => encyclopedia;
            set
            {
                encyclopedia = value;
                OnPropertyChanged("Encyclopedia");
            }
        }
        [JsonProperty("Inventory")]
        public CharacterInventory Inventory
        {
            get => inventory;
            set
            {
                inventory = value;
                OnPropertyChanged("Inventory");
            }
        }
        [JsonIgnore]
        public List<string> ExaminedItems => Encyclopedia?
            .Select(x => AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(x.Key)
            ? AppData.ServerDatabase.LocalesGlobal.Templates[x.Key].Name : x.Key)
            .ToList();
        [JsonIgnore]
        public bool IsQuestsEmpty => Quests == null || Quests.Length == 0;
        [JsonIgnore]
        public bool IsCommonSkillsEmpty => Skills == null || Skills.Common == null || Skills.Common.Length == 0;
        [JsonIgnore]
        public bool IsMasteringsEmpty => Skills == null || Skills.Mastering == null || Skills.Mastering.Length == 0;

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

        public void ExamineAll()
        {
            foreach (var item in AppData.ServerDatabase.ItemsDB
                .Where(x => x.Value.Parent != null
                && x.Value.Type == "Item"
                && !x.Value.Properties.ExaminedByDefault
                && !Encyclopedia.Any(c => c.Key == x.Key)
                && AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(x.Key)))
                Encyclopedia.Add(item.Key, true);
            OnPropertyChanged("ExaminedItems");
        }

        private string aid;
        private CharacterInfo info;
        private CharacterCustomization customization;
        private Dictionary<string, CharacterTraderStanding> traderStandings;
        private CharacterHideout hideout;
        private CharacterQuest[] quests;
        private CharacterSkills skills;
        private Dictionary<string, bool> encyclopedia;
        private CharacterInventory inventory;
    }
}