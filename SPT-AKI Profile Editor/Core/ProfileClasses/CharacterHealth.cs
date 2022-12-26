using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterHealth : BindableEntity
    {
        private CharacterMetric hydration;

        private CharacterMetric energy;

        [JsonProperty("Hydration")]
        public CharacterMetric Hydration
        {
            get => hydration;
            set
            {
                hydration = value;
                OnPropertyChanged("Hydration");
            }
        }

        [JsonProperty("Energy")]
        public CharacterMetric Energy
        {
            get => energy;
            set
            {
                energy = value;
                OnPropertyChanged("Energy");
            }
        }
    }
}