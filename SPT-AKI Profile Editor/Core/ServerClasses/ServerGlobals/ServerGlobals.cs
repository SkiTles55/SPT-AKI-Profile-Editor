using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerGlobals : BindableEntity
    {
        private ServerGlobalsConfig config;
        private Dictionary<string, ItemPreset> itemPresets;

        public ServerGlobals()
        { }

        [JsonConstructor]
        public ServerGlobals(ServerGlobalsConfig config, Dictionary<string, ItemPreset> itemPresets)
        {
            Config = config;
            ItemPresets = itemPresets;
            GlobalBuilds = GetGlobalWeaponBuilds();
        }

        [JsonProperty("config")]
        public ServerGlobalsConfig Config
        {
            get => config;
            set
            {
                config = value;
                OnPropertyChanged(nameof(Config));
            }
        }

        public Dictionary<string, ItemPreset> ItemPresets
        {
            get => itemPresets;
            set
            {
                itemPresets = value;
                OnPropertyChanged(nameof(ItemPresets));
                OnPropertyChanged(nameof(GlobalBuilds));
            }
        }

        [JsonIgnore]
        public ObservableCollection<KeyValuePair<string, WeaponBuild>> GlobalBuilds { get; }

        private ObservableCollection<KeyValuePair<string, WeaponBuild>> GetGlobalWeaponBuilds() => new(ItemPresets.Select(x => new KeyValuePair<string, WeaponBuild>(x.Key, new WeaponBuild(x.Value))));
    }
}