using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkills : INotifyPropertyChanged
    {
        [JsonProperty("Common")]
        public CharacterSkill[] Common
        {
            get => common;
            set
            {
                common = value;
                OnPropertyChanged("Common");
            }
        }
        [JsonProperty("Mastering")]
        public CharacterSkill[] Mastering
        {
            get => mastering;
            set
            {
                mastering = value;
                OnPropertyChanged("Mastering");
            }
        }

        private CharacterSkill[] common;
        private CharacterSkill[] mastering;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}