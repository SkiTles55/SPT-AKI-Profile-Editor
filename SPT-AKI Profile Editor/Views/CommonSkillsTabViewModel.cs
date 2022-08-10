using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class CommonSkillsTabViewModel : SkillsTabViewModel
    {
        public CommonSkillsTabViewModel(IDialogManager dialogManager) : base(dialogManager)
        {
        }

        public override float MaxSkillsValue => AppData.AppSettings.CommonSkillMaxValue;

        public override RelayCommand SetAllPmsSkillsCommand => new(obj => Profile.Characters.Pmc.SetAllCommonSkills(SetAllPmcSkillsValue));
        public override RelayCommand SetAllScavSkillsCommand => new(obj => Profile.Characters.Scav.SetAllCommonSkills(SetAllScavSkillsValue));
    }
}