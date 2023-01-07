using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    public class FastModeViewModel : BindableViewModel
    {
        private readonly RelayCommand _saveCommand;
        private CharacterInfo pmc;
        private CharacterInfo scav;
        private bool firstOpen = true;
        private float setAllPmcSkillsValue;
        private float setAllPmcMasteringsValue;
        private float setAllScavSkillsValue;
        private float setAllScavMasteringsValue;
        private bool pmcLevel = true;
        private bool scavLevel = true;
        private bool pmcQuests = true;
        private bool pmcCommonSkills = true;
        private bool scavCommonSkills = true;
        private bool pmcMasteringsSkills = true;
        private bool scavMasteringsSkills = true;

        public FastModeViewModel(RelayCommand saveCommand)
        {
            Pmc = new();
            Scav = new();
            _saveCommand = saveCommand;
        }

        public static AppSettings AppSettings => AppData.AppSettings;

        public static List<string> QuestStatuses => new() { "Locked", "AvailableForStart", "Started", "Fail", "AvailableForFinish", "Success" };

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

        public CharacterInfo Pmc
        {
            get => pmc;
            set
            {
                pmc = value;
                OnPropertyChanged("Pmc");
            }
        }

        public CharacterInfo Scav
        {
            get => scav;
            set
            {
                scav = value;
                OnPropertyChanged("Scav");
            }
        }

        public bool SetMerchantsMax { get; set; } = true;

        public QuestStatus SetAllQuestsValue { get; set; } = QuestStatus.Success;

        public bool SetHideoutMax { get; set; } = true;

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

        public float SetAllPmcMasteringsValue
        {
            get => setAllPmcMasteringsValue;
            set
            {
                setAllPmcMasteringsValue = Math.Min(ServerDatabase.ServerGlobals.Config.MaxProgressValue, value);
                OnPropertyChanged("SetAllPmcMasteringsValue");
            }
        }

        public float SetAllScavMasteringsValue
        {
            get => setAllScavMasteringsValue;
            set
            {
                setAllScavMasteringsValue = Math.Min(ServerDatabase.ServerGlobals.Config.MaxProgressValue, value);
                OnPropertyChanged("SetAllScavMasteringsValue");
            }
        }

        public bool ExamineAll { get; set; } = true;

        public bool AcquireAll { get; set; } = true;

        public bool PmcLevel
        {
            get => pmcLevel;
            set
            {
                pmcLevel = value;
                OnPropertyChanged("PmcLevel");
            }
        }

        public bool ScavLevel
        {
            get => scavLevel;
            set
            {
                scavLevel = value;
                OnPropertyChanged("ScavLevel");
            }
        }

        public bool PmcQuests
        {
            get => pmcQuests;
            set
            {
                pmcQuests = value;
                OnPropertyChanged("PmcQuests");
            }
        }

        public bool PmcCommonSkills
        {
            get => pmcCommonSkills;
            set
            {
                pmcCommonSkills = value;
                OnPropertyChanged("PmcCommonSkills");
            }
        }

        public bool ScavCommonSkills
        {
            get => scavCommonSkills;
            set
            {
                scavCommonSkills = value;
                OnPropertyChanged("ScavCommonSkills");
            }
        }

        public bool PmcMasteringsSkills
        {
            get => pmcMasteringsSkills;
            set
            {
                pmcMasteringsSkills = value;
                OnPropertyChanged("PmcMasteringsSkills");
            }
        }

        public bool ScavMasteringsSkills
        {
            get => scavMasteringsSkills;
            set
            {
                scavMasteringsSkills = value;
                OnPropertyChanged("ScavMasteringsSkills");
            }
        }

        public RelayCommand SaveProfile => new(obj =>
        {
            if (PmcLevel)
            {
                Profile.Characters.Pmc.Info.Level = Pmc.Level;
                Profile.Characters.Pmc.Info.Experience = Pmc.Experience;
            }
            if (ScavLevel)
            {
                Profile.Characters.Scav.Info.Level = Scav.Level;
                Profile.Characters.Scav.Info.Experience = Scav.Experience;
            }
            if (SetMerchantsMax)
                Profile.Characters.Pmc.SetAllTradersMax();
            if (PmcQuests)
                Profile.Characters.Pmc.SetAllQuests(SetAllQuestsValue);
            if (SetHideoutMax)
                Profile.Characters.Pmc.SetAllHideoutAreasMax();
            if (PmcCommonSkills)
                Profile.Characters.Pmc.SetAllCommonSkills(SetAllPmcSkillsValue);
            if (ScavCommonSkills)
                Profile.Characters.Scav.SetAllCommonSkills(SetAllScavSkillsValue);
            if (PmcMasteringsSkills)
                Profile.Characters.Pmc.SetAllMasteringsSkills(SetAllPmcMasteringsValue);
            if (ScavMasteringsSkills)
                Profile.Characters.Scav.SetAllMasteringsSkills(SetAllScavMasteringsValue);
            if (ExamineAll)
                Profile.Characters.Pmc.ExamineAll();
            if (AcquireAll)
                ServerDatabase.AcquireAllClothing();
            AppSettings.FastModeOpened = false;
            _saveCommand.Execute(null);
        });
    }
}