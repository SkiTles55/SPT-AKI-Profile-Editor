using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class UserBuilds : BindableEntity
    {
        //private Dictionary<string, WeaponBuild> equipmentBuilds;

        private Dictionary<string, WeaponBuild> weaponBuilds;

        // TODO: Implement equipment builds
        //[JsonProperty("equipmentBuilds")]
        //public Dictionary<string, WeaponBuild> EquipmentBuilds
        //{
        //    get => equipmentBuilds;
        //    set
        //    {
        //        equipmentBuilds = value;
        //        OnPropertyChanged("EquipmentBuilds");
        //    }
        //}

        [JsonProperty("weaponBuilds")]
        public Dictionary<string, WeaponBuild> WeaponBuilds
        {
            get => weaponBuilds;
            set
            {
                weaponBuilds = value;
                OnPropertyChanged("WeaponBuilds");
            }
        }
    }
}