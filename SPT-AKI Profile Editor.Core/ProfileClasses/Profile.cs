using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Profile : INotifyPropertyChanged
    {
        [JsonPropertyName("characters")]
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
            Profile profile = System.Text.Json.JsonSerializer.Deserialize<Profile>(fileText);
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
            foreach (var tr in AppData.ServerDatabase.TraderInfos)
            {
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(tr.Key)["loyaltyLevel"] = Characters.Pmc.TraderStandings[tr.Key].LoyaltyLevel;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(tr.Key)["salesSum"] = Characters.Pmc.TraderStandings[tr.Key].SalesSum;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(tr.Key)["standing"] = Characters.Pmc.TraderStandings[tr.Key].Standing;
                jobject.SelectToken("characters")["pmc"].SelectToken("TradersInfo").SelectToken(tr.Key)["unlocked"] = Characters.Pmc.TraderStandings[tr.Key].Unlocked;
            }
            string json = JsonConvert.SerializeObject(jobject, seriSettings);
            File.WriteAllText(savePath, json);
        }
    }
}