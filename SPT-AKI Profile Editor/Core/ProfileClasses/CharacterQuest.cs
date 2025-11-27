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
        private double startTime;
        private QuestStatus status;

        [JsonProperty("qid")]
        public string Qid
        {
            get => qid;
            set
            {
                qid = value;
                OnPropertyChanged(nameof(Qid));
            }
        }

        [JsonProperty("startTime")]
        public double StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        [JsonConverter(typeof(SafeEnumConverter<QuestStatus>))]
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
                StatusTimers ??= [];
                if (!StatusTimers.ContainsKey(newStatus))
                    StatusTimers.Add(newStatus, ExtMethods.GetTimestamp);
                AppData.Profile?.Characters?.Pmc?.SetupCraftForQuest(QuestQid, newStatus == QuestStatus.Success);
                OnPropertyChanged(nameof(Status));
            }
        }

        [JsonProperty("statusTimers")]
        public Dictionary<QuestStatus, double> StatusTimers { get; set; }

        [JsonIgnore]
        public string LocalizedTraderName
            => AppData.ServerDatabase.LocalesGlobal.ContainsKey(QuestTrader.Nickname()) ? AppData.ServerDatabase.LocalesGlobal[QuestTrader.Nickname()] : QuestTrader;

        [JsonIgnore]
        public string LocalizedQuestName
            => AppData.ServerDatabase.LocalesGlobal.ContainsKey(QuestQid.NameLowercased()) ? AppData.ServerDatabase.LocalesGlobal[QuestQid.NameLowercased()] : QuestQid;

        [JsonIgnore]
        public QuestType Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
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

        [JsonIgnore]
        public bool IsModdedQuest => Type switch
        {
            QuestType.Standart => !AppData.ServerDatabase.QuestsData.ContainsKey(Qid),
            _ => !AppData.ServerDatabase.LocalesGlobal.ContainsKey(QuestTrader.Nickname()),
        };

        private QuestStatus GetNewStatus(QuestStatus newStatus)
        {
            var suitableStatuses = Type.GetAvailableStatuses().Where(x => StatusIsSuitable(newStatus, x));
            newStatus = suitableStatuses.Any() ? suitableStatuses.First() : status;
            return newStatus;

            bool StatusIsSuitable(QuestStatus newStatus, QuestStatus x) => newStatus < status ? x > newStatus : x < newStatus;
        }
    }
}