using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class MasteringTabViewModel : SkillsTabViewModel
    {
        public MasteringTabViewModel(IDialogManager dialogManager,
                                     RelayCommand reloadCommand,
                                     RelayCommand faqCommand,
                                     IWorker worker) : base(dialogManager,
                                                            reloadCommand,
                                                            faqCommand,
                                                            worker)
        {
        }

        public override float MaxSkillsValue => ServerDatabase.ServerGlobals.Config.MaxProgressValue;

        public override RelayCommand SetAllPmsSkillsCommand
            => new(obj => Profile.Characters.Pmc.SetAllMasteringsSkills(SetAllPmcSkillsValue));
        public override RelayCommand SetAllScavSkillsCommand
            => new(obj => Profile.Characters.Scav.SetAllMasteringsSkills(SetAllScavSkillsValue));
    }
}