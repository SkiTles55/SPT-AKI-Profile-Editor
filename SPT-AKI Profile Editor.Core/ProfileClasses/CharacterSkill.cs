using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkill : INotifyPropertyChanged
    {
        [JsonProperty("Id")]
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        [JsonProperty("Progress")]
        public float Progress
        {
            get => progress;
            set
            {
                progress = value > AppData.ServerDatabase.CommonSkillMaxValue ? AppData.ServerDatabase.CommonSkillMaxValue : value;
                OnPropertyChanged("Progress");
            }
        }
        [JsonIgnore]
        public static float MaxValue => AppData.ServerDatabase.CommonSkillMaxValue;
        [JsonIgnore]
        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.Interface.ContainsKey(Id)
            ? AppData.ServerDatabase.LocalesGlobal.Interface[Id] : Id;

        private string id;
        private float progress;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}