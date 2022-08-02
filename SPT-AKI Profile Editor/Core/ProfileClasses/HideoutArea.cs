using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class HideoutArea : BindableEntity
    {
        private int type;

        private int level;

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

        private int GetMaxLevel()
        {
            var areaInfo = AppData.ServerDatabase?.HideoutAreaInfos?.Where(x => x.Type == Type).FirstOrDefault();
            return areaInfo != null ? areaInfo.Stages.Count - 1 : 0;
        }
    }
}