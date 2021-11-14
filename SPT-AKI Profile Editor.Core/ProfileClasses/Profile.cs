using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if (profile.Characters.Pmc.Quests != null)
            {
                foreach (var quest in AppData.ServerDatabase.QuestsData)
                    if (!profile.Characters.Pmc.Quests.Any(x => x.Qid == quest.Key))
                        profile.Characters.Pmc.Quests = profile.Characters.Pmc.Quests.Append(new() { Qid = quest.Key, Status = "Locked" }).ToArray();
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
            string json = JsonConvert.SerializeObject(jobject, seriSettings);
            File.WriteAllText(savePath, json);
        }
    }
}