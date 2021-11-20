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

        private ProfileCharacters characters;

        public void Load(string path)
        {
            string fileText = File.ReadAllText(path);
            Profile profile = JsonConvert.DeserializeObject<Profile>(fileText);
            if (profile.Characters?.Pmc?.Quests != null
                && AppData.AppSettings.AutoAddMissingQuests)
            {
                foreach (var quest in AppData.ServerDatabase.QuestsData)
                    if (!profile.Characters.Pmc.Quests.Any(x => x.Qid == quest.Key))
                        profile.Characters.Pmc.Quests = profile.Characters.Pmc.Quests.Append(new() { Qid = quest.Key, Status = "Locked" }).ToArray();
            }
            if (profile.Characters?.Pmc?.Skills?.Common != null
                && profile.Characters.Scav.Skills.Common.Length == 0
                && AppData.AppSettings.AutoAddMissingScavSkills)
            {
                List<CharacterSkill> skills = new ();
                foreach (var skill in profile.Characters.Pmc.Skills.Common)
                    skills.Add(new() { Id = skill.Id, Progress = 0 });
                profile.Characters.Scav.Skills.Common = skills.ToArray();
            }
            Characters = profile.Characters;
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
            foreach (var trader in AppData.ServerDatabase.TraderInfos)
            {
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["loyaltyLevel"] = Characters.Pmc.TraderStandings[trader.Key].LoyaltyLevel;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["salesSum"] = Characters.Pmc.TraderStandings[trader.Key].SalesSum;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["standing"] = Characters.Pmc.TraderStandings[trader.Key].Standing;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(trader.Key)["unlocked"] = Characters.Pmc.TraderStandings[trader.Key].Unlocked;
            }
            if (Characters.Pmc.Quests.Length > 0)
            {
                var questsObject = jobject.SelectToken("characters")["pmc"].SelectToken("Quests").ToObject<CharacterQuest[]>();
                for (int i = 0; i < questsObject.Length; i++)
                {
                    var quest = jobject.SelectToken("characters")["pmc"].SelectToken("Quests")[i].ToObject<CharacterQuest>();
                    if (quest != null)
                        jobject.SelectToken("characters")["pmc"].SelectToken("Quests")[i]["status"] = Characters.Pmc.Quests.Where(x => x.Qid == quest.Qid).FirstOrDefault().Status;
                }
                if (questsObject.Length > 0)
                {
                    foreach (var quest in Characters.Pmc.Quests)
                    {
                        if (!questsObject.Any(x => x.Qid == quest.Qid))
                            jobject.SelectToken("characters")["pmc"].SelectToken("Quests").LastOrDefault().AddAfterSelf(JObject.FromObject(quest));
                    }
                }
                else
                    jobject.SelectToken("characters")["pmc"].SelectToken("Quests").Replace(JToken.FromObject(Characters.Pmc.Quests));
            }
            var hideoutAreasObject = jobject.SelectToken("characters")["pmc"].SelectToken("Hideout").SelectToken("Areas").ToObject<HideoutArea[]>();
            for (int i = 0; i < hideoutAreasObject.Length; i++)
            {
                var probe = jobject.SelectToken("characters")["pmc"].SelectToken("Hideout").SelectToken("Areas")[i].ToObject<HideoutArea>();
                var areaInfo = AppData.Profile.Characters.Pmc.Hideout.Areas.Where(x => x.Type == probe.Type).FirstOrDefault();
                if (areaInfo != null)
                    jobject.SelectToken("characters")["pmc"].SelectToken("Hideout").SelectToken("Areas")[i]["level"] = areaInfo.Level;
            }
            ProcessCommonSkills(jobject, Characters.Pmc, "pmc");
            ProcessCommonSkills(jobject, Characters.Scav, "scav");
            string json = JsonConvert.SerializeObject(jobject, seriSettings);
            File.WriteAllText(savePath, json);

            void ProcessCommonSkills(JObject jobject, Character character, string keyword)
            {
                var skillsObject = jobject.SelectToken("characters")[keyword].SelectToken("Skills").SelectToken("Common").ToObject<CharacterSkill[]>();
                if (skillsObject.Length > 0)
                {
                    for (int index = 0; index < skillsObject.Length; ++index)
                    {
                        var probe = jobject.SelectToken("characters")[keyword].SelectToken("Skills").SelectToken("Common")[index]?.ToObject<CharacterSkill>();
                        var edited = character.Skills.Common.Where(x => x.Id == probe.Id).FirstOrDefault();
                        if (edited != null && probe != null && edited.Progress > probe.Progress)
                            jobject.SelectToken("characters")[keyword].SelectToken("Skills").SelectToken("Common")[index]["Progress"] = edited.Progress;
                    }
                }
                else
                    jobject.SelectToken("characters")[keyword].SelectToken("Skills").SelectToken("Common").Replace(JToken.FromObject(character.Skills.Common));
            }
        }
    }
}