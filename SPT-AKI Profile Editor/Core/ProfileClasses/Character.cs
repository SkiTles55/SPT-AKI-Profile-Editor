using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Character : BindableEntity
    {
        public List<CharacterHideoutProduction> hideoutProductions;
        private string aid;
        private string pmcId;
        private CharacterInfo info;
        private CharacterCustomization customization;
        private CharacterHealth health;
        private Dictionary<string, CharacterTraderStanding> traderStandings;
        private RagfairInfo ragfairInfo;
        private CharacterHideout hideout;
        private CharacterQuest[] quests;
        private CharacterRepeatableQuest[] repeatableQuests;
        private CharacterSkills skills;
        private Dictionary<string, bool> encyclopedia;
        private CharacterInventory inventory;
        private UnlockedInfo unlockedInfo;
        private CharacterBonus[] bonuses;

        [JsonIgnore]
        public bool IsScav => Info.Side == "Savage";

        [JsonProperty("aid")]
        public string Aid
        {
            get => aid;
            set
            {
                aid = value;
                OnPropertyChanged(nameof(Aid));
            }
        }

        [JsonProperty("_id")]
        public string PmcId
        {
            get => pmcId;
            set
            {
                pmcId = value;
                OnPropertyChanged(nameof(PmcId));
            }
        }

        public CharacterInfo Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged(nameof(Info));
            }
        }

        public CharacterCustomization Customization
        {
            get => customization;
            set
            {
                customization = value;
                OnPropertyChanged(nameof(Customization));
            }
        }

        public CharacterHealth Health
        {
            get => health;
            set
            {
                health = value;
                OnPropertyChanged(nameof(Health));
            }
        }

        [JsonProperty("TradersInfo")]
        public Dictionary<string, CharacterTraderStanding> TraderStandings
        {
            get => traderStandings;
            set
            {
                traderStandings = value;
                NotifyTradersUpdated();
            }
        }

        public UnlockedInfo UnlockedInfo
        {
            get => unlockedInfo;
            set
            {
                unlockedInfo = value;
                OnPropertyChanged(nameof(UnlockedInfo));
            }
        }

        public RagfairInfo RagfairInfo
        {
            get => ragfairInfo;
            set
            {
                ragfairInfo = value;
                OnPropertyChanged(nameof(RagfairInfo));
                OnPropertyChanged(nameof(TraderStandingsExt));
            }
        }

        public CharacterHideout Hideout
        {
            get => hideout;
            set
            {
                hideout = value;
                OnPropertyChanged(nameof(Hideout));
            }
        }

        public CharacterQuest[] Quests
        {
            get => quests;
            set
            {
                quests = value;
                OnPropertyChanged(nameof(Quests));
            }
        }

        public CharacterRepeatableQuest[] RepeatableQuests
        {
            get => repeatableQuests;
            set
            {
                repeatableQuests = value;
                OnPropertyChanged(nameof(RepeatableQuests));
            }
        }

        public CharacterSkills Skills
        {
            get => skills;
            set
            {
                skills = value;
                OnPropertyChanged(nameof(Skills));
            }
        }

        public Dictionary<string, bool> Encyclopedia
        {
            get => encyclopedia;
            set
            {
                encyclopedia = value;
                OnPropertyChanged(nameof(Encyclopedia));
                OnPropertyChanged(nameof(ExaminedItems));
            }
        }

        public CharacterInventory Inventory
        {
            get => inventory;
            set
            {
                inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }

        public CharacterBonus[] Bonuses
        {
            get => bonuses;
            set
            {
                bonuses = value;
                OnPropertyChanged(nameof(Bonuses));
            }
        }

        [JsonIgnore]
        public int StashRowsBonusCount
        {
            get
            {
                var bonus = Bonuses.FirstOrDefault(x => x.Type == CharacterBonus.StashRowsType);
                return (int)(bonus?.Value ?? 0);
            }
            set
            {
                if (value % 2 > 0)
                    value++;
                var bonus = Bonuses.FirstOrDefault(x => x.Type == CharacterBonus.StashRowsType);
                if (bonus != null)
                    bonus.Value = value;
                else
                    Bonuses = Bonuses.Append(CharacterBonus.CreateStashRowsBonus(value)).ToArray();
            }
        }

        [JsonIgnore]
        public ObservableCollection<CharacterTraderStandingExtended> TraderStandingsExt =>
            new(TraderStandings?.Select(x => new CharacterTraderStandingExtended(x.Value,
                                                                                x.Key,
                                                                                GetTraderInfo(x.Key))));

        [JsonIgnore]
        public IEnumerable<ExaminedItem> ExaminedItems => Encyclopedia?
            .Select(x => AppData.ServerDatabase.ItemsDB.ContainsKey(x.Key)
            ? AppData.ServerDatabase.ItemsDB[x.Key].GetExaminedItem() : new ExaminedItem(x.Key, x.Key, null));

        [JsonIgnore]
        public List<CharacterHideoutProduction> HideoutProductions => hideoutProductions;

        public void NotifyTradersUpdated()
        {
            OnPropertyChanged(nameof(TraderStandings));
            OnPropertyChanged(nameof(TraderStandingsExt));
        }

        public void RemoveAllQuests() => Quests = Array.Empty<CharacterQuest>();

        public void RemoveQuests(IEnumerable<string> questQids)
            => Quests = Quests.Where(x => !questQids.Contains(x.Qid)).ToArray();

        public void AddQuests(IEnumerable<CharacterQuest> newQuests)
        {
            var questList = Quests.ToList();
            foreach (var quest in newQuests)
                questList.Add(quest);
            Quests = questList.ToArray();
            UpdateQuestsData();
        }

        public void AddAllMisingQuests(bool eventQuests)
            => AddQuests(MissingQuests(eventQuests).Select(qid => new CharacterQuest() { Qid = qid, Status = QuestStatus.Locked }));

        public IEnumerable<string> MissingQuests(bool eventQuests)
        {
            bool QuestIsMissing(bool eventQuests, string qid)
                => Quests.FirstOrDefault(y => y.Qid == qid) == null
                && AppData.ServerConfigs.Quest.EventQuests.ContainsKey(qid) == eventQuests;

            return AppData.ServerDatabase.QuestsData
                .Select(x => x.Key)
                .Where(x => QuestIsMissing(eventQuests, x));
        }

        public void UpdateQuestsData()
        {
            if (Quests.Any())
            {
                foreach (var quest in Quests)
                    SetupQuest(quest);
                OnPropertyChanged(nameof(Quests));
            }

            void SetupQuest(CharacterQuest quest)
            {
                quest.QuestQid = quest.Qid;
                quest.Type = QuestType.Standart;
                if (AppData.ServerDatabase.LocalesGlobal.ContainsKey(quest.Qid.QuestName()) || RepeatableQuests == null || RepeatableQuests.Length == 0)
                {
                    quest.QuestTrader = AppData.ServerDatabase.QuestsData.ContainsKey(quest.Qid) ? AppData.ServerDatabase.QuestsData[quest.Qid].TraderId : "unknown";
                    quest.QuestData = AppData.ServerDatabase.QuestsData.ContainsKey(quest.Qid) ? AppData.ServerDatabase.QuestsData[quest.Qid] : null;
                    return;
                }

                foreach (QuestType type in Enum.GetValues(typeof(QuestType)))
                {
                    var typeQuests = RepeatableQuests.Where(x => x.Type == type).FirstOrDefault();
                    if (typeQuests == null)
                        continue;
                    if (SetupQuestFromArray(typeQuests.ActiveQuests, type))
                        return;
                    if (SetupQuestFromArray(typeQuests.InactiveQuests, type))
                        return;
                }

                bool SetupQuestFromArray(ActiveQuest[] array, QuestType type)
                {
                    if (array.Any())
                    {
                        var repeatableQuest = array.Where(x => x.Id == quest.Qid);
                        if (repeatableQuest.Any())
                        {
                            quest.Type = type;
                            quest.QuestTrader = repeatableQuest.First().TraderId;
                            quest.QuestQid = repeatableQuest.First().Type.LocalizedName();
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        public void SetAllTradersMax()
        {
            foreach (var trader in TraderStandingsExt)
                trader.LoyaltyLevel = trader.MaxLevel;
            OnPropertyChanged(nameof(TraderStandings));
            OnPropertyChanged(nameof(TraderStandingsExt));
        }

        public void SetAllQuests(QuestStatus status)
        {
            foreach (var quest in Quests)
                quest.Status = status;
        }

        public void SetAllHideoutAreasMax()
        {
            foreach (var area in Hideout?.Areas)
                if (area.CanSetMaxLevel)
                    area.Level = area.MaxLevel;
        }

        public void SetAllCommonSkills(float value)
        {
            foreach (var skill in Skills.Common)
                if (!skill.Id.ToLower().StartsWith("bot"))
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
                && AppData.ServerDatabase.LocalesGlobal.ContainsKey(x.Key.Name())))
                Encyclopedia.Add(item.Key, true);
            OnPropertyChanged(nameof(ExaminedItems));
        }

        public void RemoveExaminedItem(string id)
        {
            if (Encyclopedia.ContainsKey(id))
                Encyclopedia.Remove(id);
            OnPropertyChanged(nameof(ExaminedItems));
        }

        public void AddAllCrafts()
        {
            foreach (var production in HideoutProductions)
                production.Added = true;
        }

        public void SetupCraftForQuest(string questId, bool isCompleted)
        {
            var production = HideoutProductions?
                .FirstOrDefault(x => x.Production.UnlocksByQuest && x.Production.Requirements.Any(r => r.QuestId == questId));
            if (production != null)
                production.Added = isCompleted;
        }

        public void SetupHideoutProductions()
        {
            var productions = AppData.ServerDatabase?.HideoutProduction;
            if (UnlockedInfo != null && productions != null)
                hideoutProductions = productions
                    .Where(x => x.UnlocksByQuest)
                    .Select(x => new CharacterHideoutProduction(x, UnlockedInfo.UnlockedProductionRecipe.Contains(x.Id)))
                    .ToList();
            OnPropertyChanged(nameof(HideoutProductions));
        }

        private static TraderBase GetTraderInfo(string key)
                            => AppData.ServerDatabase.TraderInfos.ContainsKey(key) ? AppData.ServerDatabase.TraderInfos[key] : null;
    }
}