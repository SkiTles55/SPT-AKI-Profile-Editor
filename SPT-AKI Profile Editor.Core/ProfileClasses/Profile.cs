using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPT_AKI_Profile_Editor.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Profile : BindableEntity
    {
        private ProfileCharacters characters;

        private string[] suits;

        private Dictionary<string, WeaponBuild> weaponBuilds;

        private int profileHash = 0;

        [JsonProperty("characters")]
        public ProfileCharacters Characters
        {
            get => characters;
            set
            {
                characters = value;
                OnPropertyChanged("Characters");
                OnPropertyChanged("IsProfileEmpty");
            }
        }

        [JsonProperty("suits")]
        public string[] Suits
        {
            get => suits;
            set
            {
                suits = value;
                OnPropertyChanged("Suits");
            }
        }

        [JsonProperty("weaponbuilds")]
        public Dictionary<string, WeaponBuild> WeaponBuilds
        {
            get => weaponBuilds;
            set
            {
                weaponBuilds = value;
                OnPropertyChanged("WeaponBuilds");
                OnPropertyChanged("WBuilds");
            }
        }

        [JsonIgnore]
        public ObservableCollection<KeyValuePair<string, WeaponBuild>> WBuilds => WeaponBuilds != null ? new(WeaponBuilds) : new();

        [JsonIgnore]
        public bool IsProfileEmpty => Characters?.Pmc.Info == null;

        [JsonIgnore]
        public int ProfileHash => profileHash;

        public void Load(string path)
        {
            string fileText = File.ReadAllText(path);
            Profile profile = JsonConvert.DeserializeObject<Profile>(fileText);
            profileHash = JsonConvert.SerializeObject(profile).ToString().GetHashCode();
            if (profile.Characters?.Pmc?.Quests != null)
            {
                if (NeedToAddMissingQuests())
                {
                    profile.Characters.Pmc.Quests = profile.Characters.Pmc.Quests
                    .Concat(AppData.ServerDatabase.QuestsData
                    .Where(x => !profile.Characters.Pmc.Quests.Any(y => y.Qid == x.Key))
                    .Select(x => new CharacterQuest { Qid = x.Key, Status = QuestStatus.Locked })
                    .ToArray())
                    .ToArray();
                }
                if (profile.Characters.Pmc.Quests.Length > 0)
                    foreach (var quest in profile.Characters.Pmc.Quests)
                        SetupQuest(quest);
            }
            if (NeedToAddMissingScavCommonSkills())
            {
                profile.Characters.Scav.Skills.Common = profile.Characters.Pmc.Skills.Common
                    .Select(x => new CharacterSkill { Id = x.Id, Progress = 0 })
                    .ToArray();
            }
            if (NeedToAddMissingMasteringsSkills())
            {
                AddMissingMasteringSkills(profile.Characters.Pmc.Skills);
                AddMissingMasteringSkills(profile.Characters.Scav.Skills);
            }
            if (!string.IsNullOrEmpty(profile.Characters?.Pmc?.Customization?.Head)
                && !AppData.ServerDatabase.Heads.Any(x => x.Key == profile.Characters.Pmc.Customization.Head))
                AppData.ServerDatabase.Heads.Add(profile.Characters.Pmc.Customization.Head, profile.Characters.Pmc.Customization.Head);
            if (!string.IsNullOrEmpty(profile.Characters?.Scav?.Customization?.Head)
                && !AppData.ServerDatabase.Heads.Any(x => x.Key == profile.Characters.Scav.Customization.Head))
                AppData.ServerDatabase.Heads.Add(profile.Characters.Scav.Customization.Head, profile.Characters.Scav.Customization.Head);
            Characters = profile.Characters;
            Suits = profile.Suits;
            WeaponBuilds = profile.WeaponBuilds;

            static void AddMissingMasteringSkills(CharacterSkills characterSkills)
            {
                if (characterSkills.Mastering.Length != AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length)
                {
                    characterSkills.Mastering = characterSkills.Mastering
                        .Concat(AppData.ServerDatabase.ServerGlobals.Config.Mastering
                        .Where(x => !AppData.AppSettings.BannedMasterings.Contains(x.Name) && !characterSkills.Mastering.Any(y => y.Id == x.Name))
                        .Select(x => new CharacterSkill { Id = x.Name, Progress = 0 })
                        .ToArray())
                        .ToArray();
                }
            }

            void SetupQuest(CharacterQuest quest)
            {
                quest.QuestName = quest.Qid;
                quest.Type = QuestType.Standart;
                if (AppData.ServerDatabase.LocalesGlobal.Quests.ContainsKey(quest.Qid) || profile.Characters.Pmc.RepeatableQuests == null || profile.Characters.Pmc.RepeatableQuests.Length == 0)
                {
                    quest.QuestTrader = AppData.ServerDatabase.QuestsData.ContainsKey(quest.Qid) ? AppData.ServerDatabase.QuestsData[quest.Qid] : "unknown";
                    return;
                }
                var dailyQuests = profile.Characters.Pmc.RepeatableQuests.Where(x => x.Type == QuestType.Daily).First();
                if (SetupQuestFromArray(dailyQuests.ActiveQuests, QuestType.Daily))
                    return;
                if (SetupQuestFromArray(dailyQuests.InactiveQuests, QuestType.Daily))
                    return;
                var weeklyQuests = profile.Characters.Pmc.RepeatableQuests.Where(x => x.Type == QuestType.Weekly).First();
                if (SetupQuestFromArray(weeklyQuests.ActiveQuests, QuestType.Weekly))
                    return;
                if (SetupQuestFromArray(weeklyQuests.InactiveQuests, QuestType.Weekly))
                    return;

                bool SetupQuestFromArray(ActiveQuest[] array, QuestType type)
                {
                    if (array.Any())
                    {
                        var repeatableQuest = array.Where(x => x.Id == quest.Qid);
                        if (repeatableQuest.Any())
                        {
                            quest.Type = type;
                            quest.QuestTrader = repeatableQuest.First().TraderId;
                            quest.QuestName = repeatableQuest.First().Type.LocalizedName();
                            return true;
                        }
                    }
                    return false;
                }
            }

            bool NeedToAddMissingQuests() => AppData.AppSettings.AutoAddMissingQuests
                && profile.Characters.Pmc.Quests.Length != AppData.ServerDatabase.QuestsData.Count;

            bool NeedToAddMissingScavCommonSkills() => AppData.AppSettings.AutoAddMissingScavSkills
                && profile.Characters?.Pmc?.Skills?.Common != null
                && profile.Characters?.Scav?.Skills?.Common != null
                && profile.Characters.Scav.Skills.Common.Length == 0;

            bool NeedToAddMissingMasteringsSkills() => AppData.AppSettings.AutoAddMissingMasterings
                && profile.Characters?.Pmc?.Skills?.Mastering != null
                && profile.Characters?.Scav?.Skills?.Mastering != null;
        }

        public void RemoveBuild(string key)
        {
            if (WeaponBuilds.Remove(key))
            {
                OnPropertyChanged("WeaponBuilds");
                OnPropertyChanged("WBuilds");
            }
        }

        public void RemoveBuilds()
        {
            WeaponBuilds = new();
            OnPropertyChanged("WeaponBuilds");
            OnPropertyChanged("WBuilds");
        }

        public void ExportBuild(string key, string path)
        {
            WeaponBuild weaponBuild = WeaponBuilds[key];
            File.WriteAllText(path, JsonConvert.SerializeObject(weaponBuild, Formatting.Indented));
        }

        public void ImportBuild(string path)
        {
            try
            {
                WeaponBuild weaponBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(path));
                if (weaponBuild.Name != null)
                {
                    if (WeaponBuilds == null)
                        WeaponBuilds = new();
                    int count = 0;
                    string tempFileName = weaponBuild.Name;
                    while (WeaponBuilds.ContainsKey(tempFileName))
                        tempFileName = string.Format("{0}({1})", weaponBuild.Name, count++);
                    weaponBuild.Name = tempFileName;
                    weaponBuild.Id = ExtMethods.GenerateNewId(WeaponBuilds.Values.Select(x => x.Id).ToArray());
                    WeaponBuilds.Add(weaponBuild.Name, weaponBuild);
                    OnPropertyChanged("WeaponBuilds");
                    OnPropertyChanged("WBuilds");
                }
                else
                    throw new Exception(AppData.AppLocalization.GetLocalizedString("tab_presets_wrong_file") + ":" + Environment.NewLine + path);
            }
            catch (Exception ex)
            {
                Logger.Log($"WeaponBuild import error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public void Save(string targetPath, string savePath = null)
        {
            if (string.IsNullOrEmpty(savePath))
                savePath = targetPath;
            string newStash = string.Empty;
            JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented };
            JObject jobject = JObject.Parse(File.ReadAllText(targetPath));
            JToken pmc = jobject.SelectToken("characters")["pmc"];
            JToken scav = jobject.SelectToken("characters")["scav"];
            pmc.SelectToken("Info")["Nickname"] = Characters.Pmc.Info.Nickname;
            pmc.SelectToken("Info")["LowerNickname"] = Characters.Pmc.Info.Nickname.ToLower();
            pmc.SelectToken("Info")["Side"] = Characters.Pmc.Info.Side;
            pmc.SelectToken("Info")["Voice"] = Characters.Pmc.Info.Voice;
            pmc.SelectToken("Info")["Level"] = Characters.Pmc.Info.Level;
            pmc.SelectToken("Info")["Experience"] = Characters.Pmc.Info.Experience;
            pmc.SelectToken("Customization")["Head"] = Characters.Pmc.Customization.Head;
            scav.SelectToken("Info")["Nickname"] = Characters.Scav.Info.Nickname;
            scav.SelectToken("Info")["Voice"] = Characters.Scav.Info.Voice;
            scav.SelectToken("Info")["Level"] = Characters.Scav.Info.Level;
            scav.SelectToken("Info")["Experience"] = Characters.Scav.Info.Experience;
            scav.SelectToken("Customization")["Head"] = Characters.Scav.Customization.Head;
            pmc.SelectToken("Encyclopedia").Replace(JToken.FromObject(Characters.Pmc.Encyclopedia));
            JToken TradersInfo = pmc.SelectToken("TradersInfo");
            foreach (var trader in AppData.ServerDatabase.TraderInfos)
            {
                var current = TradersInfo.SelectToken(trader.Key).ToObject<CharacterTraderStanding>();
                if (current.LoyaltyLevel == Characters.Pmc.TraderStandings[trader.Key].LoyaltyLevel)
                    continue;
                TradersInfo.SelectToken(trader.Key)["loyaltyLevel"] = Characters.Pmc.TraderStandings[trader.Key].LoyaltyLevel;
                TradersInfo.SelectToken(trader.Key)["salesSum"] = Characters.Pmc.TraderStandings[trader.Key].SalesSum;
                TradersInfo.SelectToken(trader.Key)["standing"] = Characters.Pmc.TraderStandings[trader.Key].Standing;
                TradersInfo.SelectToken(trader.Key)["unlocked"] = Characters.Pmc.TraderStandings[trader.Key].Unlocked;
            }
            WriteQuests();
            WriteHideout();
            WriteSkills(Characters.Pmc.Skills.Common, pmc, "Common");
            WriteSkills(Characters.Scav.Skills.Common, scav, "Common");
            WriteSkills(Characters.Pmc.Skills.Mastering, pmc, "Mastering");
            WriteSkills(Characters.Scav.Skills.Mastering, scav, "Mastering");
            jobject.SelectToken("suits").Replace(JToken.FromObject(Suits.ToArray()));
            WritePmcStash();
            WriteScavStash();
            jobject.SelectToken("weaponbuilds").Replace(JToken.FromObject(WeaponBuilds));
            string json = JsonConvert.SerializeObject(jobject, seriSettings);
            File.WriteAllText(savePath, json);

            void WriteQuests()
            {
                var questsObject = pmc.SelectToken("Quests").ToObject<CharacterQuest[]>();
                if (questsObject.Length > 0)
                {
                    for (int index = 0; index < questsObject.Length; ++index)
                    {
                        var quest = pmc.SelectToken("Quests")[index].ToObject<CharacterQuest>();
                        var edited = Characters.Pmc.Quests.Where(x => x.Qid == quest.Qid).FirstOrDefault();
                        if (edited != null && quest != null && edited.Status != quest.Status)
                            pmc.SelectToken("Quests")[index]["status"] = edited.Status.ToString();
                    }
                    foreach (var quest in Characters.Pmc.Quests.Where(x => !questsObject.Any(y => y.Qid == x.Qid)))
                        pmc.SelectToken("Quests").LastOrDefault().AddAfterSelf(JObject.FromObject(quest));
                }
                else
                    pmc.SelectToken("Quests").Replace(JToken.FromObject(Characters.Pmc.Quests));
            }

            void WriteSkills(CharacterSkill[] skills, JToken character, string type)
            {
                var skillsObject = character.SelectToken("Skills").SelectToken(type).ToObject<CharacterSkill[]>();
                if (skillsObject.Length > 0)
                {
                    for (int index = 0; index < skillsObject.Length; ++index)
                    {
                        var probe = character.SelectToken("Skills").SelectToken(type)[index]?.ToObject<CharacterSkill>();
                        var edited = skills.Where(x => x.Id == probe.Id).FirstOrDefault();
                        if (edited != null && probe != null && edited.Progress != probe.Progress)
                            character.SelectToken("Skills").SelectToken(type)[index]["Progress"] = edited.Progress;
                    }
                    foreach (var skill in skills.Where(x => !skillsObject.Any(y => y.Id == x.Id)))
                        character.SelectToken("Skills").SelectToken(type).LastOrDefault().AddAfterSelf(JObject.FromObject(skill));
                }
                else
                    character.SelectToken("Skills").SelectToken(type).Replace(JToken.FromObject(skills));
            }

            void WritePmcStash()
            {
                List<JToken> ForRemove = new();
                var itemsObject = pmc.SelectToken("Inventory").SelectToken("items").ToObject<InventoryItem[]>();
                if (itemsObject.Length > 0)
                {
                    for (int index = 0; index < itemsObject.Length; ++index)
                    {
                        var probe = pmc.SelectToken("Inventory").SelectToken("items")[index]?.ToObject<InventoryItem>();
                        if (probe == null)
                            continue;
                        if (!string.IsNullOrEmpty(newStash) && probe.Id == Characters.Pmc.Inventory.Stash && probe.Tpl != newStash)
                            pmc.SelectToken("Inventory").SelectToken("items")[index]["_tpl"] = newStash;
                        if (!AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Id == probe.Id))
                            ForRemove.Add(pmc.SelectToken("Inventory").SelectToken("items")[index]);
                        if (probe.SlotId == AppData.AppSettings.PocketsSlotId)
                            pmc.SelectToken("Inventory").SelectToken("items")[index]["_tpl"] = AppData.Profile.Characters.Pmc.Inventory.Pockets;
                    }
                    foreach (var removedItem in ForRemove)
                        removedItem.Remove();
                    foreach (var item in AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => !itemsObject.Any(y => y.Id == x.Id)))
                        pmc.SelectToken("Inventory").SelectToken("items").LastOrDefault()
                            .AddAfterSelf(ExtMethods.RemoveNullAndEmptyProperties(JObject.FromObject(item)));
                }
            }

            void WriteScavStash()
            {
                var itemsObject = scav.SelectToken("Inventory").SelectToken("items").ToObject<InventoryItem[]>();
                if (itemsObject.Length > 0)
                {
                    for (int index = 0; index < itemsObject.Length; ++index)
                    {
                        var probe = scav.SelectToken("Inventory").SelectToken("items")[index]?.ToObject<InventoryItem>();
                        if (probe == null)
                            continue;
                        if (probe.SlotId == AppData.AppSettings.PocketsSlotId)
                            scav.SelectToken("Inventory").SelectToken("items")[index]["_tpl"] = AppData.Profile.Characters.Scav.Inventory.Pockets;
                    }
                }
            }

            void WriteHideout()
            {
                var hideoutAreasObject = pmc.SelectToken("Hideout").SelectToken("Areas").ToObject<HideoutArea[]>();
                for (int i = 0; i < hideoutAreasObject.Length; i++)
                {
                    var probe = pmc.SelectToken("Hideout").SelectToken("Areas")[i].ToObject<HideoutArea>();
                    var areaInfo = AppData.Profile.Characters.Pmc.Hideout.Areas.Where(x => x.Type == probe.Type).FirstOrDefault();
                    if (areaInfo == null)
                        continue;
                    if (areaInfo.Level > 0 && areaInfo.Level > probe.Level)
                    {
                        for (int l = probe.Level; l <= areaInfo.Level; l++)
                        {
                            var areaBonuses = AppData.ServerDatabase.HideoutAreaInfos
                                .Where(x => x.Type == probe.Type)
                                .FirstOrDefault()
                                .Stages[l.ToString()];
                            if (areaBonuses == null)
                                continue;
                            var BonusesList = JObject.Parse(areaBonuses.ToString()).SelectToken("bonuses").ToObject<List<JToken>>();
                            if (BonusesList == null || BonusesList.Count == 0)
                                continue;
                            foreach (var listItem in BonusesList)
                            {
                                var bonus = listItem.ToObject<CharacterBonuses>();
                                if (bonus == null)
                                    continue;
                                switch (bonus.Type)
                                {
                                    case "StashSize":
                                        newStash = bonus.TemplateId;
                                        break;

                                    case "MaximumEnergyReserve":
                                        pmc.SelectToken("Health").SelectToken("Energy")["Maximum"] = 110;
                                        break;
                                }
                                pmc.SelectToken("Bonuses").LastOrDefault().AddAfterSelf(JObject.FromObject(listItem));
                            }
                        }
                    }
                    pmc.SelectToken("Hideout").SelectToken("Areas")[i]["level"] = areaInfo.Level;
                }
            }
        }
    }
}