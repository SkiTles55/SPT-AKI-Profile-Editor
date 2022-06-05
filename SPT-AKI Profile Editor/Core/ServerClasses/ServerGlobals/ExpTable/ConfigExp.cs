using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ConfigExp : BindableEntity
    {
        private ExpLevel level;

        [JsonProperty("level")]
        public ExpLevel Level
        {
            get => level;
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }
    }
}