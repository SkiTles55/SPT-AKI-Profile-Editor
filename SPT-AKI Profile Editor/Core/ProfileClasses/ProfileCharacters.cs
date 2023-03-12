using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ProfileCharacters : TemplateableEntity
    {
        private Character pmc;

        private Character scav;

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

        public override string TemplateEntityId => "Characters";

        public override string TemplateLocalizedName => TemplateEntityId;
    }
}