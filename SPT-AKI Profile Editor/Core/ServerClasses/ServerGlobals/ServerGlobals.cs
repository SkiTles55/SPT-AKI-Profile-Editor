using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerGlobals : BindableEntity
    {
        private ServerGlobalsConfig config;
        private Dictionary<string, ItemPreset> itemPresets;

        [JsonPropertyName("config")]
        public ServerGlobalsConfig Config
        {
            get => config;
            set
            {
                config = value;
                OnPropertyChanged("Config");
            }
        }

        [JsonPropertyName("ItemPresets")]
        public Dictionary<string, ItemPreset> ItemPresets
        {
            get => itemPresets;
            set
            {
                itemPresets = value;
                OnPropertyChanged("ItemPresets");
            }
        }
    }
}