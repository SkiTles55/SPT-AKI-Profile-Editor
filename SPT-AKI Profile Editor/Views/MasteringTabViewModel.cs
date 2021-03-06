using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class MasteringTabViewModel : BindableViewModel
    {
        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;

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

        public RelayCommand OpenSettingsCommand => new(async obj =>
         {
             await Dialogs.ShowSettingsDialog(this, 1);
         });
    }
}