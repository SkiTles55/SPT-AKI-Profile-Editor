using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;

namespace SPT_AKI_Profile_Editor.Views
{
    public class MasteringTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;

        public MasteringTabViewModel(IDialogManager dialogManager) => _dialogManager = dialogManager;

        public float SetAllPmcSkillsValue
        {
            get => setAllPmcSkillsValue;
            set
            {
                setAllPmcSkillsValue = Math.Min(ServerDatabase.ServerGlobals.Config.MaxProgressValue, value);
                OnPropertyChanged("SetAllPmcSkillsValue");
            }
        }

        public float SetAllScavSkillsValue
        {
            get => setAllScavSkillsValue;
            set
            {
                setAllScavSkillsValue = Math.Min(ServerDatabase.ServerGlobals.Config.MaxProgressValue, value);
                OnPropertyChanged("SetAllScavSkillsValue");
            }
        }

        public RelayCommand SetAllPmsSkillsCommand => new(obj => Profile.Characters.Pmc.SetAllMasteringsSkills(SetAllPmcSkillsValue));
        public RelayCommand SetAllScavSkillsCommand => new(obj => Profile.Characters.Scav.SetAllMasteringsSkills(SetAllScavSkillsValue));

        public RelayCommand OpenSettingsCommand => new(async obj => await _dialogManager.ShowSettingsDialog(this, 1));
    }
}