using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterQuest : BindableEntity
    {
        [JsonProperty("qid")]
        public string Qid
        {
            get => qid;
            set
            {
                qid = value;
                OnPropertyChanged("Qid");
            }
        }
        [JsonProperty("status")]
        public string Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        [JsonIgnore]
        public string LocalizedTraderName => AppData.ServerDatabase.LocalesGlobal.Trading.ContainsKey(QuestTrader) ? AppData.ServerDatabase.LocalesGlobal.Trading[QuestTrader].Nickname : QuestTrader;
        [JsonIgnore]
        public string LocalizedQuestName => AppData.ServerDatabase.LocalesGlobal.Quests.ContainsKey(Qid) ? AppData.ServerDatabase.LocalesGlobal.Quests[Qid].Name : Qid;

        private string qid;
        private string status;
        private string QuestTrader => AppData.ServerDatabase.QuestsData.ContainsKey(Qid) ? AppData.ServerDatabase.QuestsData[Qid] : "unknown";
    }
}