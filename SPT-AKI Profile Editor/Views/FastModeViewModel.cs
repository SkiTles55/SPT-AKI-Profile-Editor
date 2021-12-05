using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Views
{
    class FastModeViewModel : INotifyPropertyChanged
    {
        public RelayCommand OpenningRefresh => new(obj =>
        {
            if (firstOpen)
            {
                firstOpen = false;
                Pmc.Experience = long.MaxValue;
                Scav.Experience = long.MaxValue;
                SetAllPmcSkillsValue = AppSettings.CommonSkillMaxValue;
                SetAllScavSkillsValue = AppSettings.CommonSkillMaxValue;
                SetAllPmcMasteringsValue = ServerDatabase.ServerGlobals?.Config?.MaxProgressValue ?? 0;
                SetAllScavMasteringsValue = ServerDatabase.ServerGlobals?.Config?.MaxProgressValue ?? 0;
            }
        });

        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static AppSettings AppSettings => AppData.AppSettings;
        public static ServerDatabase ServerDatabase => AppData.ServerDatabase;
        public CharacterInfo Pmc
        {
            get => pmc;
            set
            {
                OnPropertyChanged("Pmc");
                pmc = value;
            }
        }
        public CharacterInfo Scav
        {
            get => scav;
            set
            {
                OnPropertyChanged("Scav");
                scav = value;
            }
        }
        public bool SetMerchantsMax { get; set; } = true;
        public static List<string> QuestStatuses => new() { "Locked", "AvailableForStart", "Started", "Fail", "AvailableForFinish", "Success" };
        public string SetAllQuestsValue { get; set; } = "Success";
        public bool SetHideoutMax { get; set; } = true;
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
        public float SetAllPmcMasteringsValue
        {
            get => setAllPmcMasteringsValue;
            set
            {
                setAllPmcMasteringsValue = value > ServerDatabase.ServerGlobals.Config.MaxProgressValue ? ServerDatabase.ServerGlobals.Config.MaxProgressValue : value;
                OnPropertyChanged("SetAllPmcMasteringsValue");
            }
        }
        public float SetAllScavMasteringsValue
        {
            get => setAllScavMasteringsValue;
            set
            {
                setAllScavMasteringsValue = value > ServerDatabase.ServerGlobals.Config.MaxProgressValue ? ServerDatabase.ServerGlobals.Config.MaxProgressValue : value;
                OnPropertyChanged("SetAllScavMasteringsValue");
            }
        }
        public bool ExamineAll { get; set; } = true;
        public bool AcquireAll { get; set; } = true;
        public RelayCommand SaveProfile => new(obj =>
        {
            AppData.Profile.Characters.Pmc.Info.Level = Pmc.Level;
            AppData.Profile.Characters.Scav.Info.Level = Scav.Level;
            AppData.Profile.Characters.Pmc.Info.Experience = Pmc.Experience;
            AppData.Profile.Characters.Scav.Info.Experience = Scav.Experience;
            if (SetMerchantsMax)
                ServerDatabase.SetAllTradersMax();
            AppData.Profile.Characters.Pmc.SetAllQuests(SetAllQuestsValue);
            if (SetHideoutMax)
                AppData.Profile.Characters.Pmc.SetAllHideoutAreasMax();
            AppData.Profile.Characters.Pmc.SetAllCommonSkills(SetAllPmcSkillsValue);
            AppData.Profile.Characters.Scav.SetAllCommonSkills(SetAllScavSkillsValue);
            AppData.Profile.Characters.Pmc.SetAllMasteringsSkills(SetAllPmcMasteringsValue);
            AppData.Profile.Characters.Scav.SetAllMasteringsSkills(SetAllScavMasteringsValue);
            if (ExamineAll)
                AppData.Profile.Characters.Pmc.ExamineAll();
            if (AcquireAll)
                ServerDatabase.AcquireAllClothing();
            AppSettings.FastModeOpened = false;
            MainWindowViewModel.SaveProfileAndReload();
        });

        private CharacterInfo pmc = new();
        private CharacterInfo scav = new();
        private bool firstOpen = true;
        private float setAllPmcSkillsValue;
        private float setAllPmcMasteringsValue;
        private float setAllScavSkillsValue;
        private float setAllScavMasteringsValue;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
