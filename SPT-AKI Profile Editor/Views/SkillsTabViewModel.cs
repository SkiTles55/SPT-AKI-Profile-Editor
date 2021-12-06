using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    class SkillsTabViewModel : BindableViewModel
    {
        public static AppSettings AppSettings => AppData.AppSettings;
        public float SetAllPmcSkillsValue
        {
            get => setAllPmcSkillsValue;
            set
            {
                setAllPmcSkillsValue = value > AppSettings.CommonSkillMaxValue ? AppSettings.CommonSkillMaxValue : value;
                OnPropertyChanged("SetAllPmcSkillsValue");
            }
        }
        public float SetAllScavSkillsValue
        {
            get => setAllScavSkillsValue;
            set
            {
                setAllScavSkillsValue = value > AppSettings.CommonSkillMaxValue ? AppSettings.CommonSkillMaxValue : value;
                OnPropertyChanged("SetAllScavSkillsValue");
            }
        }
        public RelayCommand SetAllPmsSkillsCommand => new(obj => { Profile.Characters.Pmc.SetAllCommonSkills(SetAllPmcSkillsValue); });
        public RelayCommand SetAllScavSkillsCommand => new(obj => { Profile.Characters.Scav.SetAllCommonSkills(SetAllScavSkillsValue); });
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            await Dialogs.ShowSettingsDialog(this, 1);
        });

        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;
    }
}