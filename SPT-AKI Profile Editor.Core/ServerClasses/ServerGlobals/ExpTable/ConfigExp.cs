using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ConfigExp : BindableEntity
    {
        [JsonPropertyName("level")]
        public ExpLevel Level
        {
            get => level;
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }

        private ExpLevel level;
    }
}