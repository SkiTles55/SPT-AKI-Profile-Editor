using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkills : TemplateableEntity
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

        public override string TemplateEntityId => "Skills";

        public override string TemplateLocalizedName => AppData.AppLocalization.Translations["tab_changes_skills_and_masterings"];
    }
}