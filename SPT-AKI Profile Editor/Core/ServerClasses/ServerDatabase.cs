using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerDatabase : BindableEntity
    {
        private Dictionary<string, string> heads;
        private Dictionary<string, string> voices;
        private Dictionary<string, string> localesGlobal;
        private ServerGlobals serverGlobals;
        private Dictionary<string, TraderBase> traderInfos;
        private Dictionary<string, QuestData> questsData;
        private List<HideoutAreaInfo> hideoutAreaInfos;
        private Dictionary<string, TarkovItem> itemsDB;
        private Dictionary<string, string> pockets;
        private List<TraderSuit> traderSuits;
        private Handbook handbook;
        private HandbookHelper handbookHelper;
        private List<Achievement> achievements;

        public Dictionary<string, string> Heads
        {
            get => heads;
            set
            {
                heads = value;
                OnPropertyChanged(nameof(Heads));
            }
        }

        public Dictionary<string, string> Voices
        {
            get => voices;
            set
            {
                voices = value;
                OnPropertyChanged(nameof(Voices));
            }
        }

        public Dictionary<string, string> LocalesGlobal
        {
            get => localesGlobal;
            set
            {
                localesGlobal = value;
                OnPropertyChanged(nameof(LocalesGlobal));
            }
        }

        public ServerGlobals ServerGlobals
        {
            get => serverGlobals;
            set
            {
                serverGlobals = value;
                OnPropertyChanged(nameof(ServerGlobals));
            }
        }

        public Dictionary<string, TraderBase> TraderInfos
        {
            get => traderInfos;
            set
            {
                traderInfos = value;
                OnPropertyChanged(nameof(TraderInfos));
            }
        }

        public Dictionary<string, QuestData> QuestsData
        {
            get => questsData;
            set
            {
                questsData = value;
                OnPropertyChanged(nameof(QuestsData));
            }
        }

        public List<HideoutAreaInfo> HideoutAreaInfos
        {
            get => hideoutAreaInfos;
            set
            {
                hideoutAreaInfos = value;
                OnPropertyChanged(nameof(HideoutAreaInfos));
            }
        }

        public Dictionary<string, TarkovItem> ItemsDB
        {
            get => itemsDB;
            set
            {
                itemsDB = value;
                OnPropertyChanged(nameof(ItemsDB));
            }
        }

        public Dictionary<string, string> Pockets
        {
            get => pockets;
            set
            {
                pockets = value;
                OnPropertyChanged(nameof(Pockets));
            }
        }

        public List<TraderSuit> TraderSuits
        {
            get => traderSuits;
            set
            {
                traderSuits = value;
                OnPropertyChanged(nameof(TraderSuits));
            }
        }

        public Handbook Handbook
        {
            get => handbook;
            set
            {
                handbook = value;
                OnPropertyChanged(nameof(Handbook));
            }
        }

        public HandbookHelper HandbookHelper
        {
            get => handbookHelper;
            set
            {
                handbookHelper = value;
                OnPropertyChanged(nameof(HandbookHelper));
            }
        }

        public HideoutProduction[] HideoutProduction { get; set; }

        public List<Achievement> Achievements
        {
            get => achievements;
            set
            {
                achievements = value;
                OnPropertyChanged(nameof(Achievements));
            }
        }

        public void AcquireAllClothing()
        {
            foreach (var suit in TraderSuits)
                suit.Boughted = true;
        }
    }
}