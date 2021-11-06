using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Profile : INotifyPropertyChanged
    {
        [JsonPropertyName("characters")]
        public ProfileCharacters Characters
        {
            get => characters;
            set
            {
                characters = value;
                OnPropertyChanged("Characters");
            }
        }

        private ProfileCharacters characters;

        public void Load(string path)
        {
            string fileText = File.ReadAllText(path);
            Profile profile = JsonSerializer.Deserialize<Profile>(fileText);
            Characters = profile.Characters;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}