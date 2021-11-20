using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerDatabase : INotifyPropertyChanged
    {
        public float CommonSkillMaxValue => 5100;

        public Dictionary<string, string> Heads
        {
            get => heads;
            set
            {
                heads = value;
                OnPropertyChanged("Heads");
            }
        }
        public Dictionary<string, string> Voices
        {
            get => voices;
            set
            {
                voices = value;
                OnPropertyChanged("Voices");
            }
        }
        public LocalesGlobal LocalesGlobal
        {
            get => localesGlobal;
            set
            {
                localesGlobal = value;
                OnPropertyChanged("LocalesGlobal");
            }
        }
        public ServerGlobals ServerGlobals
        {
            get => serverGlobals;
            set
            {
                serverGlobals = value;
                OnPropertyChanged("ServerGlobals");
            }
        }
        public Dictionary<string, TraderBase> TraderInfos
        {
            get => traderInfos;
            set
            {
                traderInfos = value;
                OnPropertyChanged("TraderInfos");
            }
        }
        public Dictionary<string, string> QuestsData
        {
            get => questsData;
            set
            {
                questsData = value;
                OnPropertyChanged("QuestsData");
            }
        }
        public List<HideoutAreaInfo> HideoutAreaInfos
        {
            get => hideoutAreaInfos;
            set
            {
                hideoutAreaInfos = value;
                OnPropertyChanged("HideoutAreaInfos");
            }
        }

        public void SetAllTradersMax()
        {
            foreach (var trader in TraderInfos.Values)
                trader.Level = trader.MaxLevel;
        }

        private Dictionary<string, string> heads;
        private Dictionary<string, string> voices;
        private LocalesGlobal localesGlobal;
        private ServerGlobals serverGlobals;
        private Dictionary<string, TraderBase> traderInfos;
        private Dictionary<string, string> questsData;
        private List<HideoutAreaInfo> hideoutAreaInfos;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}