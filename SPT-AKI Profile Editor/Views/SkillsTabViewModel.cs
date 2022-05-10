using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class SkillsTabViewModel : BindableViewModel
    {
        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;
        public static AppSettings AppSettings => AppData.AppSettings;

        public float SetAllPmcSkillsValue
        {
            get => setAllPmcSkillsValue;
            set
            {
                setAllPmcSkillsValue = Math.Min(AppSettings.CommonSkillMaxValue, value);
                OnPropertyChanged("SetAllPmcSkillsValue");
            }
        }

        public float SetAllScavSkillsValue
        {
            get => setAllScavSkillsValue;
            set
            {
                setAllScavSkillsValue = Math.Min(AppSettings.CommonSkillMaxValue, value);
                OnPropertyChanged("SetAllScavSkillsValue");
            }
        }

        public RelayCommand SetAllPmsSkillsCommand => new(obj => { Profile.Characters.Pmc.SetAllCommonSkills(SetAllPmcSkillsValue); });
        public RelayCommand SetAllScavSkillsCommand => new(obj => { Profile.Characters.Scav.SetAllCommonSkills(SetAllScavSkillsValue); });

        public RelayCommand OpenSettingsCommand => new(async obj =>
         {
             await Dialogs.ShowSettingsDialog(this, 1);
         });
    }
}