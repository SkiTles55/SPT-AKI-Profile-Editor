using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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

        private ProfileCharacters characters;
        private string[] suits;

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public void Save(string targetPath, string savePath = null)
        {
            if (string.IsNullOrEmpty(savePath))
                savePath = targetPath;
            JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented };
            JObject jobject = JObject.Parse(File.ReadAllText(targetPath));
            jobject.SelectToken("characters")["pmc"].SelectToken("Info")["Nickname"] = Characters.Pmc.Info.Nickname;
            jobject.SelectToken("characters")["pmc"].SelectToken("Info")["LowerNickname"] = Characters.Pmc.Info.Nickname.ToLower();
            jobject.SelectToken("characters")["pmc"].SelectToken("Info")["Side"] = Characters.Pmc.Info.Side;
            jobject.SelectToken("characters")["pmc"].SelectToken("Info")["Voice"] = Characters.Pmc.Info.Voice;
            jobject.SelectToken("characters")["pmc"].SelectToken("Info")["Level"] = Characters.Pmc.Info.Level;
            jobject.SelectToken("characters")["pmc"].SelectToken("Info")["Experience"] = Characters.Pmc.Info.Experience;
            jobject.SelectToken("characters")["pmc"].SelectToken("Customization")["Head"] = Characters.Pmc.Customization.Head;
            jobject.SelectToken("characters")["scav"].SelectToken("Info")["Nickname"] = Characters.Scav.Info.Nickname;
            jobject.SelectToken("characters")["scav"].SelectToken("Info")["Voice"] = Characters.Scav.Info.Voice;
            jobject.SelectToken("characters")["scav"].SelectToken("Info")["Level"] = Characters.Scav.Info.Level;
            jobject.SelectToken("characters")["scav"].SelectToken("Info")["Experience"] = Characters.Scav.Info.Experience;
            jobject.SelectToken("characters")["scav"].SelectToken("Customization")["Head"] = Characters.Scav.Customization.Head;
            jobject.SelectToken("characters")["pmc"].SelectToken("Encyclopedia").Replace(JToken.FromObject(Characters.Pmc.Encyclopedia));
            foreach (var trader in AppData.ServerDatabase.TraderInfos)
            {
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["loyaltyLevel"] = Characters.Pmc.TraderStandings[trader.Key].LoyaltyLevel;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["salesSum"] = Characters.Pmc.TraderStandings[trader.Key].SalesSum;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["standing"] = Characters.Pmc.TraderStandings[trader.Key].Standing;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["unlocked"] = Characters.Pmc.TraderStandings[trader.Key].Unlocked;
            }
            WriteQuests();
            var hideoutAreasObject = jobject.SelectToken("characters")["pmc"].SelectToken("Hideout").SelectToken("Areas").ToObject<HideoutArea[]>();
            for (int i = 0; i < hideoutAreasObject.Length; i++)
            {
                var probe = jobject.SelectToken("characters")["pmc"].SelectToken("Hideout").SelectToken("Areas")[i].ToObject<HideoutArea>();
                var areaInfo = AppData.Profile.Characters.Pmc.Hideout.Areas.Where(x => x.Type == probe.Type).FirstOrDefault();
                if (areaInfo != null)
                    jobject.SelectToken("characters")["pmc"].SelectToken("Hideout").SelectToken("Areas")[i]["level"] = areaInfo.Level;
            }
            WriteSkills(Characters.Pmc.Skills.Common, "pmc", "Common");
            WriteSkills(Characters.Scav.Skills.Common, "scav", "Common");
            WriteSkills(Characters.Pmc.Skills.Mastering, "pmc", "Mastering");
            WriteSkills(Characters.Scav.Skills.Mastering, "scav", "Mastering");
            jobject.SelectToken("suits").Replace(JToken.FromObject(Suits.ToArray()));
            string json = JsonConvert.SerializeObject(jobject, seriSettings);
            File.WriteAllText(savePath, json);

            void WriteQuests()
            {
                var questsObject = jobject.SelectToken("characters")["pmc"].SelectToken("Quests").ToObject<CharacterQuest[]>();
                if (questsObject.Length > 0)
                {
                    for (int index = 0; index < questsObject.Length; ++index)
                    {
                        var quest = jobject.SelectToken("characters")["pmc"].SelectToken("Quests")[index].ToObject<CharacterQuest>();
                        var edited = Characters.Pmc.Quests.Where(x => x.Qid == quest.Qid).FirstOrDefault();
                        if (edited != null && quest != null && edited.Status != quest.Status)
                            jobject.SelectToken("characters")["pmc"].SelectToken("Quests")[index]["status"] = edited.Status;
                    }
                    foreach (var quest in Characters.Pmc.Quests.Where(x => !questsObject.Any(y => y.Qid == x.Qid)))
                        jobject.SelectToken("characters")["pmc"].SelectToken("Quests").LastOrDefault().AddAfterSelf(JObject.FromObject(quest));
                }
                else
                    jobject.SelectToken("characters")["pmc"].SelectToken("Quests").Replace(JToken.FromObject(Characters.Pmc.Quests));
            }

            void WriteSkills(CharacterSkill[] skills, string character, string type)
            {
                var skillsObject = jobject.SelectToken("characters")[character].SelectToken("Skills").SelectToken(type).ToObject<CharacterSkill[]>();
                if (skillsObject.Length > 0)
                {
                    for (int index = 0; index < skillsObject.Length; ++index)
                    {
                        var probe = jobject.SelectToken("characters")[character].SelectToken("Skills").SelectToken(type)[index]?.ToObject<CharacterSkill>();
                        var edited = skills.Where(x => x.Id == probe.Id).FirstOrDefault();
                        if (edited != null && probe != null && edited.Progress > probe.Progress)
                            jobject.SelectToken("characters")[character].SelectToken("Skills").SelectToken(type)[index]["Progress"] = edited.Progress;
                    }
                    foreach (var skill in skills.Where(x => !skillsObject.Any(y => y.Id == x.Id)))
                        jobject.SelectToken("characters")[character].SelectToken("Skills").SelectToken(type).LastOrDefault().AddAfterSelf(JObject.FromObject(skill));
                }
                else
                    jobject.SelectToken("characters")[character].SelectToken("Skills").SelectToken(type).Replace(JToken.FromObject(skills));
            }
        }
    }
}