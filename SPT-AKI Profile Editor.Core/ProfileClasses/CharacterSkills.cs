using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkills : BindableEntity
    {
        [JsonProperty("Common")]
        public CharacterSkill[] Common
        {
            get => common;
            set
            {
                common = value;
                OnPropertyChanged("Common");
            }
        }
        [JsonProperty("Mastering")]
        public CharacterSkill[] Mastering
        {
            get => mastering;
            set
            {
                mastering = value;
                OnPropertyChanged("Mastering");
            }
        }

        private CharacterSkill[] common;
        private CharacterSkill[] mastering;
    }
}