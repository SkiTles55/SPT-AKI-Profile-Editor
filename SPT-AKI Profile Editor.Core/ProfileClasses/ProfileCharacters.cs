using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ProfileCharacters : INotifyPropertyChanged
    {
        [JsonProperty("pmc")]
        public Character Pmc
        {
            get => pmc;
            set
            {
                pmc = value;
                OnPropertyChanged("Pmc");
            }
        }
        [JsonProperty("scav")]
        public Character Scav
        {
            get => scav;
            set
            {
                scav = value;
                OnPropertyChanged("Scav");
            }
        }

        private Character pmc;
        private Character scav;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}