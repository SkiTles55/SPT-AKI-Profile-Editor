using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
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

        [JsonConverter(typeof(QuestStatusConverter))]
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
                if (StatusTimers == null)
                    StatusTimers = new();
                if (!StatusTimers.ContainsKey(newStatus))
                    StatusTimers.Add(newStatus, ExtMethods.GetTimestamp);
                OnPropertyChanged("Status");
            }
        }

        [JsonProperty("statusTimers")]
        public Dictionary<QuestStatus, double> StatusTimers { get; set; }

        [JsonIgnore]
        public string LocalizedTraderName => AppData.ServerDatabase.LocalesGlobal.ContainsKey(QuestTrader.Nickname()) ? AppData.ServerDatabase.LocalesGlobal[QuestTrader.Nickname()] : QuestTrader;

        [JsonIgnore]
        public string LocalizedQuestName => AppData.ServerDatabase.LocalesGlobal.ContainsKey(QuestQid.QuestName()) ? AppData.ServerDatabase.LocalesGlobal[QuestQid.QuestName()] : QuestQid;

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

        [JsonIgnore]
        public string LocalizedQuestType => Type.LocalizedName();

        [JsonIgnore]
        public string QuestTrader { get; set; } = "unknown";

        [JsonIgnore]
        public QuestData QuestData { get; set; }

        [JsonIgnore]
        public string QuestQid { get; set; } = "unknown";

        private QuestStatus GetNewStatus(QuestStatus newStatus)
        {
            var suitableStatuses = Type.GetAvailableStatuses().Where(x => StatusIsSuitable(newStatus, x));
            newStatus = suitableStatuses.Any() ? suitableStatuses.First() : status;
            return newStatus;

            bool StatusIsSuitable(QuestStatus newStatus, QuestStatus x) => newStatus < status ? x > newStatus : x < newStatus;
        }
    }
}