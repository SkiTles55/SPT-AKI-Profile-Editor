using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LevelExpTable : INotifyPropertyChanged
    {
        [JsonPropertyName("exp")]
        public long Exp
        {
            get => exp;
            set
            {
                exp = value;
                OnPropertyChanged("Exp");
            }
        }

        private long exp;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
