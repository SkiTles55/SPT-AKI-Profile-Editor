using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterCustomization : INotifyPropertyChanged
    {
        [JsonProperty("Head")]
        public string Head
        {
            get => head;
            set
            {
                head = value;
                OnPropertyChanged("Head");
            }
        }

        private string head;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}