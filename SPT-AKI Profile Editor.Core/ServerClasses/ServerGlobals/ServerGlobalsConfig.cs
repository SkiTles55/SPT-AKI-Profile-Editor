using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerGlobalsConfig : INotifyPropertyChanged
    {
        [JsonPropertyName("Mastering")]
        public Mastering[] Mastering
        {
            get => mastering;
            set
            {
                mastering = value;
                OnPropertyChanged("Mastering");
            }
        }
        [JsonPropertyName("exp")]
        public ConfigExp Exp
        {
            get => exp;
            set
            {
                exp = value;
                OnPropertyChanged("Exp");
            }
        }
        [JsonIgnore]
        public float MaxProgressValue => Mastering.Max(x => x.Level2 + x.Level3);

        private Mastering[] mastering;
        private ConfigExp exp;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}