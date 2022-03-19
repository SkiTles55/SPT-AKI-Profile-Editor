using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerGlobals : BindableEntity
    {
        private ServerGlobalsConfig config;

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
    }
}