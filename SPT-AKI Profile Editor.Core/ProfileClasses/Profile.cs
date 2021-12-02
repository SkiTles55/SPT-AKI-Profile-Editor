using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Profile : INotifyPropertyChanged
    {
        [JsonProperty("characters")]
        public ProfileCharacters Characters
        {
            get => characters;
            set
            {
                characters = value;
                OnPropertyChanged("Characters");
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
        public ObservableCollection<KeyValuePair<string, WeaponBuild>> WBuilds => new(WeaponBuilds);

        private ProfileCharacters characters;
        private string[] suits;
        private Dictionary<string, WeaponBuild> weaponBuilds;

        public void Load(string path)
        {
            string fileText = File.ReadAllText(path);
            Profile profile = JsonConvert.DeserializeObject<Profile>(fileText);
            if (profile.Characters?.Pmc?.Quests != null
                && AppData.AppSettings.AutoAddMissingQuests)
            {
                if (profile.Characters.Pmc.Quests.Length != AppData.ServerDatabase.QuestsData.Count)
                {
                    profile.Characters.Pmc.Quests = profile.Characters.Pmc.Quests
                    .Concat(AppData.ServerDatabase.QuestsData
                    .Where(x => !profile.Characters.Pmc.Quests.Any(y => y.Qid == x.Key))
                    .Select(x => new CharacterQuest { Qid = x.Key, Status = "Locked" })
                    .ToArray())
                    .ToArray();
                }
            }
            if (profile.Characters?.Pmc?.Skills?.Common != null
                && profile.Characters?.Scav?.Skills?.Common != null
                && profile.Characters.Scav.Skills.Common.Length == 0
                && AppData.AppSettings.AutoAddMissingScavSkills)
            {
                profile.Characters.Scav.Skills.Common = profile.Characters.Pmc.Skills.Common
                    .Select(x => new CharacterSkill { Id = x.Id, Progress = 0 })
                    .ToArray();
            }
            if (profile.Characters?.Pmc?.Skills?.Mastering != null
                && profile.Characters?.Scav?.Skills?.Mastering != null
                && AppData.AppSettings.AutoAddMissingMasterings)
            {
                AddMissingMasteringSkills(profile.Characters.Pmc.Skills);
                AddMissingMasteringSkills(profile.Characters.Scav.Skills);
            }
            Characters = profile.Characters;
            Suits = profile.Suits;
            WeaponBuilds = profile.WeaponBuilds;

            static void AddMissingMasteringSkills(CharacterSkills characterSkills)
            {
                if (characterSkills.Mastering.Length != AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length)
                {
                    characterSkills.Mastering = characterSkills.Mastering
                        .Concat(AppData.ServerDatabase.ServerGlobals.Config.Mastering
                        .Where(x => !characterSkills.Mastering.Any(y => y.Id == x.Name))
                        .Select(x => new CharacterSkill { Id = x.Name, Progress = 0 })
                        .ToArray())
                        .ToArray();
                }
            }
        }

        public void RemoveBuild(string key)
        {
            if (WeaponBuilds.Remove(key))
            {
                OnPropertyChanged("WeaponBuilds");
                OnPropertyChanged("WBuilds");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public void Save(string targetPath, string savePath = null)
        {
            if (string.IsNullOrEmpty(savePath))
                savePath = targetPath;
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
                TradersInfo.SelectToken(trader.Key)["loyaltyLevel"] = Characters.Pmc.TraderStandings[trader.Key].LoyaltyLevel;
                TradersInfo.SelectToken(trader.Key)["salesSum"] = Characters.Pmc.TraderStandings[trader.Key].SalesSum;
                TradersInfo.SelectToken(trader.Key)["standing"] = Characters.Pmc.TraderStandings[trader.Key].Standing;
                TradersInfo.SelectToken(trader.Key)["unlocked"] = Characters.Pmc.TraderStandings[trader.Key].Unlocked;
            }
            WriteQuests();
            var hideoutAreasObject = pmc.SelectToken("Hideout").SelectToken("Areas").ToObject<HideoutArea[]>();
            for (int i = 0; i < hideoutAreasObject.Length; i++)
            {
                var probe = pmc.SelectToken("Hideout").SelectToken("Areas")[i].ToObject<HideoutArea>();
                var areaInfo = AppData.Profile.Characters.Pmc.Hideout.Areas.Where(x => x.Type == probe.Type).FirstOrDefault();
                if (areaInfo != null)
                    pmc.SelectToken("Hideout").SelectToken("Areas")[i]["level"] = areaInfo.Level;
            }
            WriteSkills(Characters.Pmc.Skills.Common, pmc, "Common");
            WriteSkills(Characters.Scav.Skills.Common, scav, "Common");
            WriteSkills(Characters.Pmc.Skills.Mastering, pmc, "Mastering");
            WriteSkills(Characters.Scav.Skills.Mastering, scav, "Mastering");
            jobject.SelectToken("suits").Replace(JToken.FromObject(Suits.ToArray()));
            WriteStash();
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
                            pmc.SelectToken("Quests")[index]["status"] = edited.Status;
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
                        if (edited != null && probe != null && edited.Progress > probe.Progress)
                            character.SelectToken("Skills").SelectToken(type)[index]["Progress"] = edited.Progress;
                    }
                    foreach (var skill in skills.Where(x => !skillsObject.Any(y => y.Id == x.Id)))
                        character.SelectToken("Skills").SelectToken(type).LastOrDefault().AddAfterSelf(JObject.FromObject(skill));
                }
                else
                    character.SelectToken("Skills").SelectToken(type).Replace(JToken.FromObject(skills));
            }

            void WriteStash()
            {
                List<JToken> ForRemove = new ();
                var itemsObject = pmc.SelectToken("Inventory").SelectToken("items").ToObject<InventoryItem[]>();
                if (itemsObject.Length > 0)
                {
                    for (int index = 0; index < itemsObject.Length; ++index)
                    {
                        var probe = pmc.SelectToken("Inventory").SelectToken("items")[index]?.ToObject<InventoryItem>();
                        if (probe == null)
                            continue;
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
        }
    }
}