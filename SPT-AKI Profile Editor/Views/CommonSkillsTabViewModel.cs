using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class CommonSkillsTabViewModel(IDialogManager dialogManager,
        RelayCommand reloadCommand,
        RelayCommand faqCommand,
        IWorker worker,
        IHelperModManager helperModManager) : SkillsTabViewModel(dialogManager,
            reloadCommand,
            faqCommand,
            worker,
            helperModManager)
    {
        public override float MaxSkillsValue => AppData.AppSettings.CommonSkillMaxValue;

        public override RelayCommand SetAllPmsSkillsCommand
            => new(obj => Profile.Characters.Pmc.SetAllCommonSkills(SetAllPmcSkillsValue));

        public override RelayCommand SetAllScavSkillsCommand
            => new(obj => Profile.Characters.Scav.SetAllCommonSkills(SetAllScavSkillsValue));

        public RelayCommand AddMissingPmcSkills => new(obj =>
        {
            Profile.Characters.Pmc.AddMissingSkills();
            ApplyFilter();
        });

        public RelayCommand AddMissingScavSkills => new(obj =>
        {
            Profile.Characters.Scav.AddMissingSkills();
            ApplyFilter();
        });

        public override void ApplyFilter()
        {
            HasMissingPmcSkills = Profile?.Characters?.Pmc?.Skills?.IsCommonSkillsEmpty ?? false;
            HasMissingScavSkills = Profile?.Characters?.Scav?.Skills?.IsCommonSkillsEmpty ?? false;
        }
    }
}