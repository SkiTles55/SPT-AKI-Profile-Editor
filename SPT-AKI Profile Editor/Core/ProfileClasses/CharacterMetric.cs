using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterMetric : BindableEntity
    {
        private int current;

        private int maximum;

        [JsonProperty("Current")]
        public int Current
        {
            get => current;
            set
            {
                current = value;
                OnPropertyChanged("Current");
                if (value > Maximum)
                    Maximum = value;
            }
        }

        [JsonProperty("Maximum")]
        public int Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                OnPropertyChanged("Maximum");
            }
        }
    }
}