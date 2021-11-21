using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Views
{
    class SkillsTabViewModel : INotifyPropertyChanged
    {
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static AppSettings AppSettings => AppData.AppSettings;
        public static Profile Profile => AppData.Profile;
        public static GridFilters GridFilters => AppData.GridFilters;
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

        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}