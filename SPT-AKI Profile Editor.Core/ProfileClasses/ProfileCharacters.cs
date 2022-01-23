using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ProfileCharacters : BindableEntity
    {
        [JsonProperty("pmc")]
        public Character Pmc
        {
            get => pmc;
            set
            {
                pmc = value;
                OnPropertyChanged("Pmc");
            }
        }
        [JsonProperty("scav")]
        public Character Scav
        {
            get => scav;
            set
            {
                scav = value;
                OnPropertyChanged("Scav");
            }
        }

        private Character pmc;
        private Character scav;
    }
}