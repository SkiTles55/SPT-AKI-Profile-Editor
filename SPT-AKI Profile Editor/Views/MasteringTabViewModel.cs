using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Views
{
    class MasteringTabViewModel : INotifyPropertyChanged
    {
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static Profile Profile => AppData.Profile;
        public static ServerDatabase ServerDatabase => AppData.ServerDatabase;
        public static GridFilters GridFilters => AppData.GridFilters;
        public float SetAllPmcSkillsValue
        {
            get => setAllPmcSkillsValue;
            set
            {
                setAllPmcSkillsValue = value > ServerDatabase.ServerGlobals.Config.MaxProgressValue ? ServerDatabase.ServerGlobals.Config.MaxProgressValue : value;
                OnPropertyChanged("SetAllPmcSkillsValue");
            }
        }
        public float SetAllScavSkillsValue
        {
            get => setAllScavSkillsValue;
            set
            {
                setAllScavSkillsValue = value > ServerDatabase.ServerGlobals.Config.MaxProgressValue ? ServerDatabase.ServerGlobals.Config.MaxProgressValue : value;
                OnPropertyChanged("SetAllScavSkillsValue");
            }
        }
        public RelayCommand SetAllPmsSkillsCommand => new(obj => { Profile.Characters.Pmc.SetAllMasteringsSkills(SetAllPmcSkillsValue); });
        public RelayCommand SetAllScavSkillsCommand => new(obj => { Profile.Characters.Scav.SetAllMasteringsSkills(SetAllScavSkillsValue); });

        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
