using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerDatabase : INotifyPropertyChanged
    {
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

        private Dictionary<string, string> heads;
        private Dictionary<string, string> voices;
        private LocalesGlobal localesGlobal;
        private ServerGlobals serverGlobals;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}