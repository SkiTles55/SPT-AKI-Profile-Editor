using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    class FastModeViewModel : BindableViewModel
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
        public static AppSettings AppSettings => AppData.AppSettings;
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
            Profile.Characters.Pmc.Info.Level = Pmc.Level;
            Profile.Characters.Scav.Info.Level = Scav.Level;
            Profile.Characters.Pmc.Info.Experience = Pmc.Experience;
            Profile.Characters.Scav.Info.Experience = Scav.Experience;
            if (SetMerchantsMax)
                ServerDatabase.SetAllTradersMax();
            Profile.Characters.Pmc.SetAllQuests(SetAllQuestsValue);
            if (SetHideoutMax)
                Profile.Characters.Pmc.SetAllHideoutAreasMax();
            Profile.Characters.Pmc.SetAllCommonSkills(SetAllPmcSkillsValue);
            Profile.Characters.Scav.SetAllCommonSkills(SetAllScavSkillsValue);
            Profile.Characters.Pmc.SetAllMasteringsSkills(SetAllPmcMasteringsValue);
            Profile.Characters.Scav.SetAllMasteringsSkills(SetAllScavMasteringsValue);
            if (ExamineAll)
                Profile.Characters.Pmc.ExamineAll();
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
    }
}
