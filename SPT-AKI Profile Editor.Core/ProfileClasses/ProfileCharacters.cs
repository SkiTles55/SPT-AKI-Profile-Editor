using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ProfileCharacters : INotifyPropertyChanged
    {
        [JsonPropertyName("pmc")]
        public CharacterPmc Pmc
        {
            get => pmc;
            set
            {
                pmc = value;
                OnPropertyChanged("Pmc");
            }
        }

        private CharacterPmc pmc;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}