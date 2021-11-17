using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class HideoutArea : INotifyPropertyChanged
    {
        [JsonProperty("type")]
        public int Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        [JsonProperty("level")]
        public int Level
        {
            get => level;
            set
            {
                if (value > MaxLevel)
                    value = MaxLevel;
                level = value;
                OnPropertyChanged("Level");
            }
        }
        [JsonIgnore]
        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.Interface.ContainsKey($"hideout_area_{Type}_name") ?
            AppData.ServerDatabase.LocalesGlobal.Interface[$"hideout_area_{Type}_name"] :
            $"hideout_area_{Type}_name";
        [JsonIgnore]
        public int MaxLevel => GetMaxLevel();

        private int type;
        private int level;

        private int GetMaxLevel()
        {
            var areaInfo = AppData.ServerDatabase?.HideoutAreaInfos?.Where(x => x.Type == Type).FirstOrDefault();
            return areaInfo != null ? areaInfo.Stages.Count - 1 : 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}