using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterCustomization : INotifyPropertyChanged
    {
        [JsonPropertyName("Head")]
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