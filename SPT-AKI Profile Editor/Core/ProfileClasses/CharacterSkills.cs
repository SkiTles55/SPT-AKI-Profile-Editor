using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkills : BindableEntity
    {
        private CharacterSkill[] common;

        private CharacterSkill[] mastering;

        public CharacterSkill[] Common
        {
            get => common;
            set
            {
                common = value;
                OnPropertyChanged(nameof(Common));
            }
        }

        public CharacterSkill[] Mastering
        {
            get => mastering;
            set
            {
                mastering = value;
                OnPropertyChanged(nameof(Mastering));
            }
        }
    }
}