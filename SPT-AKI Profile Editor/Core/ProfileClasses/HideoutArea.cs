using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    [method: JsonConstructor]
    public class HideoutArea(int type, int level) : BindableEntity
    {
        private int type = type;
        private int level = level;

        [JsonProperty("type")]
        public int Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        [JsonProperty("level")]
        public int Level
        {
            get => level;
            set
            {
                level = Math.Min(value, MaxLevel);
                OnPropertyChanged(nameof(Level));
                var areaInfo = AppData.ServerDatabase.HideoutAreaInfos.FirstOrDefault(x => x.Type == type);
                if (!string.IsNullOrEmpty(areaInfo?.Id))
                    SetAreaLevel(x => x.ParentArea == areaInfo.Id);
                if (!string.IsNullOrEmpty(areaInfo?.ParentArea))
                    SetAreaLevel(x => x.Id == areaInfo.ParentArea);
            }
        }

        [JsonIgnore]
        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.ContainsKey($"hideout_area_{Type}_name") ?
            AppData.ServerDatabase.LocalesGlobal[$"hideout_area_{Type}_name"] :
            $"hideout_area_{Type}_name";

        [JsonIgnore]
        public int MaxLevel => GetMaxLevel();

        private void SetAreaLevel(Func<HideoutAreaInfo, bool> predicate)
        {
            var areaType = AppData.ServerDatabase.HideoutAreaInfos.FirstOrDefault(predicate)?.Type;
            if (areaType == null)
                return;
            var area = AppData.Profile.Characters?.Pmc?.Hideout?.Areas.FirstOrDefault(x => x.Type == areaType);
            if (area != null && area.Level != level)
                area.Level = level;
        }

        private int GetMaxLevel()
        {
            var areaInfo = AppData.ServerDatabase?.HideoutAreaInfos?.Where(x => x.Type == Type).FirstOrDefault();
            return areaInfo != null ? areaInfo.Stages.Count - 1 : 0;
        }
    }
}