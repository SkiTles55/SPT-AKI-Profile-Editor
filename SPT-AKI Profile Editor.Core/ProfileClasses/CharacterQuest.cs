using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterQuest : BindableEntity
    {
        private QuestType type;

        private string qid;

        private QuestStatus status;

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
        public QuestStatus Status
        {
            get => status;
            set
            {
                var newStatus = value;
                if (!Type.GetAvailableStatuses().Contains(newStatus))
                    newStatus = GetNewStatus(newStatus);
                status = newStatus;
                OnPropertyChanged("Status");
            }
        }

        [JsonIgnore]
        public string LocalizedTraderName => AppData.ServerDatabase.LocalesGlobal.Trading.ContainsKey(QuestTrader) ? AppData.ServerDatabase.LocalesGlobal.Trading[QuestTrader].Nickname : QuestTrader;

        [JsonIgnore]
        public string LocalizedQuestName => AppData.ServerDatabase.LocalesGlobal.Quests.ContainsKey(Qid) ? AppData.ServerDatabase.LocalesGlobal.Quests[Qid].Name : Qid;

        [JsonIgnore]
        public QuestType Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        private string QuestTrader => AppData.ServerDatabase.QuestsData.ContainsKey(Qid) ? AppData.ServerDatabase.QuestsData[Qid] : "unknown";

        private QuestStatus GetNewStatus(QuestStatus newStatus)
        {
            var suitableStatuses = Type.GetAvailableStatuses().Where(x => StatusIsSuitable(newStatus, x));
            newStatus = suitableStatuses.Any() ? suitableStatuses.First() : status;
            return newStatus;

            bool StatusIsSuitable(QuestStatus newStatus, QuestStatus x)
            {
                return newStatus < status ? x > newStatus : x < newStatus;
            }
        }
    }
}