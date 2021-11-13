using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerGlobals : INotifyPropertyChanged
    {
        [JsonPropertyName("config")]
        public ServerGlobalsConfig Config
        {
            get => config;
            set
            {
                config = value;
                OnPropertyChanged("Config");
            }
        }

        private ServerGlobalsConfig config;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
