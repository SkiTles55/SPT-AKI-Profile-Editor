using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterMetric : BindableEntity
    {
        private float current;

        private float maximum;

        [JsonProperty("Current")]
        public float Current
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
        public float Maximum
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