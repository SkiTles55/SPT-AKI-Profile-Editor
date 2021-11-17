using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterHideout : INotifyPropertyChanged
    {
        [JsonProperty("Areas")]
        public HideoutArea[] Areas
        {
            get => areas;
            set
            {
                areas = value;
                OnPropertyChanged("Areas");
            }
        }

        private HideoutArea[] areas;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}