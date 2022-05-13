using SPT_AKI_Profile_Editor.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core
{
    public class AppSettings : BindableEntity
    {
        public readonly string repoAuthor = "SkiTles55";

        public readonly string repoName = "SPT-AKI-Profile-Editor";

        public readonly List<QuestStatus> standartQuestStatuses = new()
        {
            QuestStatus.Locked,
            QuestStatus.AvailableForStart,
            QuestStatus.Started,
            QuestStatus.Fail,
            QuestStatus.AvailableForFinish,
            QuestStatus.Success
        };

        public readonly List<QuestStatus> repeatableQuestStatuses = new()
        {
            QuestStatus.AvailableForStart,
            QuestStatus.Started,
            QuestStatus.Fail,
            QuestStatus.AvailableForFinish,
            QuestStatus.Success
        };

        [JsonIgnore]
        public bool Loaded = false;

        public static readonly string configurationFile = Path.Combine(DefaultValues.AppDataFolder, "AppSettings.json");

        private string serverPath;
        private string defaultProfile;
        private string language;
        private string colorScheme;
        private bool autoAddMissingQuests;
        private bool autoAddMissingScavSkills;
        private bool autoAddMissingMasterings;
        private string pocketsContainerTpl;
        private float commonSkillMaxValue;
        private Dictionary<string, string> serverProfiles;
        private string pocketsSlotId;
        private string firstPrimaryWeaponSlotId;
        private string headwearSlotId;
        private string tacticalVestSlotId;
        private string securedContainerSlotId;
        private string backpackSlotId;
        private string earpieceSlotId;
        private string faceCoverSlotId;
        private string eyewearSlotId;
        private string armorVestSlotId;
        private string secondPrimaryWeaponSlotId;
        private string holsterSlotId;
        private string scabbardSlotId;
        private string armBandSlotId;
        private string moneysDollarsTpl;
        private string moneysRublesTpl;
        private string moneysEurosTpl;
        private List<string> bannedItems;
        private List<string> bannedMasterings;
        private IssuesAction issuesAction;
        private bool fastModeOpened = false;

        public string ServerPath
        {
            get => serverPath;
            set
            {
                bool _needReload = serverPath != value;
                serverPath = value;
                OnPropertyChanged("ServerPath");
                if (Loaded)
                {
                    if (_needReload)
                        LoadProfiles();
                    Save();
                }
            }
        }

        public string DefaultProfile
        {
            get => defaultProfile;
            set
            {
                defaultProfile = value;
                OnPropertyChanged("DefaultProfile");
                if (Loaded)
                    Save();
            }
        }

        public string Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged("Language");
                if (Loaded)
                    Save();
            }
        }

        public string ColorScheme
        {
            get => colorScheme;
            set
            {
                colorScheme = value;
                OnPropertyChanged("ColorScheme");
                if (Loaded)
                    Save();
            }
        }

        public Dictionary<string, string> DirsList { get; set; }
        public Dictionary<string, string> FilesList { get; set; }

        public bool AutoAddMissingQuests
        {
            get => autoAddMissingQuests;
            set
            {
                autoAddMissingQuests = value;
                OnPropertyChanged("AutoAddMissingQuests");
                if (Loaded)
                    Save();
            }
        }

        public bool AutoAddMissingMasterings
        {
            get => autoAddMissingMasterings;
            set
            {
                autoAddMissingMasterings = value;
                OnPropertyChanged("AutoAddMissingWeaponSkills");
                if (Loaded)
                    Save();
            }
        }

        public bool AutoAddMissingScavSkills
        {
            get => autoAddMissingScavSkills;
            set
            {
                autoAddMissingScavSkills = value;
                OnPropertyChanged("AutoAddMissingScavSkills");
                if (Loaded)
                    Save();
            }
        }

        public string PocketsContainerTpl
        {
            get => pocketsContainerTpl;
            set
            {
                pocketsContainerTpl = value;
                OnPropertyChanged("PocketsContainerTpl");
                if (Loaded)
                    Save();
            }
        }

        public float CommonSkillMaxValue
        {
            get => commonSkillMaxValue;
            set
            {
                commonSkillMaxValue = value;
                OnPropertyChanged("CommonSkillMaxValue");
                if (Loaded)
                    Save();
            }
        }

        public string PocketsSlotId
        {
            get => pocketsSlotId;
            set
            {
                pocketsSlotId = value;
                OnPropertyChanged("PocketsSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string FirstPrimaryWeaponSlotId
        {
            get => firstPrimaryWeaponSlotId;
            set
            {
                firstPrimaryWeaponSlotId = value;
                OnPropertyChanged("FirstPrimaryWeaponSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string HeadwearSlotId
        {
            get => headwearSlotId;
            set
            {
                headwearSlotId = value;
                OnPropertyChanged("HeadwearSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string TacticalVestSlotId
        {
            get => tacticalVestSlotId;
            set
            {
                tacticalVestSlotId = value;
                OnPropertyChanged("TacticalVestSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string SecuredContainerSlotId
        {
            get => securedContainerSlotId;
            set
            {
                securedContainerSlotId = value;
                OnPropertyChanged("SecuredContainerSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string BackpackSlotId
        {
            get => backpackSlotId;
            set
            {
                backpackSlotId = value;
                OnPropertyChanged("BackpackSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string EarpieceSlotId
        {
            get => earpieceSlotId;
            set
            {
                earpieceSlotId = value;
                OnPropertyChanged("EarpieceSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string FaceCoverSlotId
        {
            get => faceCoverSlotId;
            set
            {
                faceCoverSlotId = value;
                OnPropertyChanged("FaceCoverSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string EyewearSlotId
        {
            get => eyewearSlotId;
            set
            {
                eyewearSlotId = value;
                OnPropertyChanged("EyewearSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string ArmorVestSlotId
        {
            get => armorVestSlotId;
            set
            {
                armorVestSlotId = value;
                OnPropertyChanged("ArmorVestSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string SecondPrimaryWeaponSlotId
        {
            get => secondPrimaryWeaponSlotId;
            set
            {
                secondPrimaryWeaponSlotId = value;
                OnPropertyChanged("SecondPrimaryWeaponSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string HolsterSlotId
        {
            get => holsterSlotId;
            set
            {
                holsterSlotId = value;
                OnPropertyChanged("HolsterSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string ScabbardSlotId
        {
            get => scabbardSlotId;
            set
            {
                scabbardSlotId = value;
                OnPropertyChanged("ScabbardSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string ArmBandSlotId
        {
            get => armBandSlotId;
            set
            {
                armBandSlotId = value;
                OnPropertyChanged("ArmBandSlotId");
                if (Loaded)
                    Save();
            }
        }

        public string MoneysDollarsTpl
        {
            get => moneysDollarsTpl;
            set
            {
                moneysDollarsTpl = value;
                OnPropertyChanged("MoneysDollarsTpl");
                if (Loaded)
                    Save();
            }
        }

        public string MoneysRublesTpl
        {
            get => moneysRublesTpl;
            set
            {
                moneysRublesTpl = value;
                OnPropertyChanged("MoneysRublesTpl");
                if (Loaded)
                    Save();
            }
        }

        public string MoneysEurosTpl
        {
            get => moneysEurosTpl;
            set
            {
                moneysEurosTpl = value;
                OnPropertyChanged("MoneysEurosTpl");
                if (Loaded)
                    Save();
            }
        }

        public List<string> BannedItems
        {
            get => bannedItems;
            set
            {
                bannedItems = value;
                OnPropertyChanged("BannedItems");
                if (Loaded)
                    Save();
            }
        }

        public List<string> BannedMasterings
        {
            get => bannedMasterings;
            set
            {
                bannedMasterings = value;
                OnPropertyChanged("BannedMasterings");
                if (Loaded)
                    Save();
            }
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IssuesAction IssuesAction
        {
            get => issuesAction;
            set
            {
                issuesAction = value;
                OnPropertyChanged("IssuesAction");
                if (Loaded)
                    Save();
            }
        }

        [JsonIgnore]
        public Dictionary<string, string> ServerProfiles
        {
            get => serverProfiles;
            set
            {
                serverProfiles = value;
                OnPropertyChanged("ServerProfiles");
            }
        }

        [JsonIgnore]
        public bool FastModeOpened
        {
            get => fastModeOpened;
            set
            {
                fastModeOpened = value;
                OnPropertyChanged("FastModeOpened");
            }
        }

        public static string GetStamp()
        {
            return AppData.AppSettings.ServerPath
                + AppData.AppSettings.DefaultProfile
                + AppData.AppSettings.Language
                + AppData.AppSettings.AutoAddMissingQuests.ToString()
                + AppData.AppSettings.AutoAddMissingMasterings.ToString()
                + AppData.AppSettings.AutoAddMissingScavSkills.ToString()
                + AppData.AppSettings.CommonSkillMaxValue.ToString()
                + AppData.AppSettings.PocketsContainerTpl
                + AppData.AppSettings.PocketsSlotId
                + AppData.AppSettings.EarpieceSlotId
                + AppData.AppSettings.HeadwearSlotId
                + AppData.AppSettings.FaceCoverSlotId
                + AppData.AppSettings.TacticalVestSlotId
                + AppData.AppSettings.FirstPrimaryWeaponSlotId
                + AppData.AppSettings.BackpackSlotId
                + AppData.AppSettings.SecuredContainerSlotId
                + AppData.AppSettings.EyewearSlotId
                + AppData.AppSettings.ArmorVestSlotId
                + AppData.AppSettings.SecondPrimaryWeaponSlotId
                + AppData.AppSettings.HolsterSlotId
                + AppData.AppSettings.ScabbardSlotId
                + AppData.AppSettings.ArmBandSlotId
                + AppData.AppSettings.MoneysDollarsTpl
                + AppData.AppSettings.MoneysEurosTpl
                + AppData.AppSettings.MoneysRublesTpl;
        }

        public void Load()
        {
            Loaded = false;
            if (File.Exists(configurationFile))
                LoadFromFile();
            else
                CreateDefault();
            Loaded = true;
            LoadProfiles();
        }

        public void Save() => ExtMethods.SaveJson(configurationFile, this);

        public void LoadProfiles()
        {
            Dictionary<string, string> Profiles = new();
            if (string.IsNullOrEmpty(ServerPath)) return;
            if (!Directory.Exists(Path.Combine(ServerPath, DirsList["dir_profiles"]))) return;
            foreach (var file in Directory.GetFiles(Path.Combine(ServerPath, DirsList["dir_profiles"])))
            {
                try
                {
                    string profileFile = File.ReadAllText(file);
                    ServerProfile serverProfile = JsonSerializer.Deserialize<ServerProfile>(profileFile);
                    if (serverProfile.Characters.Pmc.Info == null)
                        serverProfile.Characters.Pmc.Info = new() { Nickname = "Empty", Level = 0, Side = "Empty" };
                    Profiles.Add(Path.GetFileName(file), serverProfile.ToString() + $" [{Path.GetFileName(file)}]");
                }
                catch (Exception ex) { Logger.Log($"Profile ({file}) reading error: {ex.Message}"); }
            }
            if (Profiles.Count > 0)
            {
                if (string.IsNullOrEmpty(DefaultProfile) || !Profiles.ContainsKey(DefaultProfile))
                    DefaultProfile = Profiles.Keys.First();
            }
            else
                DefaultProfile = null;
            ServerProfiles = Profiles;
        }

        private void LoadFromFile()
        {
            try
            {
                string cfg = File.ReadAllText(configurationFile);
                AppSettings loaded = JsonSerializer.Deserialize<AppSettings>(cfg);
                bool _needReSave = loaded.CheckValues();
                ApplyLoadedValues(loaded);
                if (_needReSave)
                {
                    Logger.Log($"Configuration file updated");
                    Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Configuration file ({configurationFile}) loading error: {ex.Message}");
                CreateDefault();
            }
        }

        private void ApplyLoadedValues(AppSettings loaded)
        {
            ServerPath = loaded.ServerPath;
            DefaultProfile = loaded.DefaultProfile;
            Language = loaded.Language;
            ColorScheme = loaded.ColorScheme;
            DirsList = loaded.DirsList;
            FilesList = loaded.FilesList;
            AutoAddMissingMasterings = loaded.AutoAddMissingMasterings;
            AutoAddMissingQuests = loaded.AutoAddMissingQuests;
            AutoAddMissingScavSkills = loaded.AutoAddMissingScavSkills;
            PocketsContainerTpl = loaded.PocketsContainerTpl;
            FirstPrimaryWeaponSlotId = loaded.FirstPrimaryWeaponSlotId;
            HeadwearSlotId = loaded.HeadwearSlotId;
            TacticalVestSlotId = loaded.TacticalVestSlotId;
            SecuredContainerSlotId = loaded.SecuredContainerSlotId;
            BackpackSlotId = loaded.BackpackSlotId;
            EarpieceSlotId = loaded.EarpieceSlotId;
            FaceCoverSlotId = loaded.FaceCoverSlotId;
            EyewearSlotId = loaded.EyewearSlotId;
            ArmorVestSlotId = loaded.ArmorVestSlotId;
            SecondPrimaryWeaponSlotId = loaded.SecondPrimaryWeaponSlotId;
            HolsterSlotId = loaded.HolsterSlotId;
            ScabbardSlotId = loaded.ScabbardSlotId;
            ArmBandSlotId = loaded.ArmBandSlotId;
            CommonSkillMaxValue = loaded.CommonSkillMaxValue;
            PocketsSlotId = loaded.PocketsSlotId;
            MoneysDollarsTpl = loaded.MoneysDollarsTpl;
            MoneysEurosTpl = loaded.MoneysEurosTpl;
            MoneysRublesTpl = loaded.MoneysRublesTpl;
            BannedItems = loaded.BannedItems;
            BannedMasterings = loaded.bannedMasterings;
            IssuesAction = loaded.IssuesAction;
        }

        private bool CheckValues()
        {
            bool _needReSave = false;
            if (DirsList == null)
            {
                DirsList = new();
                _needReSave = true;
            }
            if (FilesList == null)
            {
                FilesList = new();
                _needReSave = true;
            }
            foreach (var dir in DefaultValues.DefaultDirsList.Where(x => !DirsList.ContainsKey(x.Key)))
            {
                DirsList.Add(dir.Key, dir.Value);
                _needReSave = true;
            }
            foreach (var file in DefaultValues.DefaultFilesList.Where(x => !FilesList.ContainsKey(x.Key)))
            {
                FilesList.Add(file.Key, file.Value);
                _needReSave = true;
            }
            if (PocketsContainerTpl == null)
            {
                PocketsContainerTpl = DefaultValues.PocketsContainerTpl;
                _needReSave = true;
            }
            if (PocketsSlotId == null)
            {
                PocketsSlotId = DefaultValues.PocketsSlotId;
                _needReSave = true;
            }
            if (MoneysDollarsTpl == null)
            {
                MoneysDollarsTpl = DefaultValues.MoneysDollarsTpl;
                _needReSave = true;
            }
            if (MoneysRublesTpl == null)
            {
                MoneysRublesTpl = DefaultValues.MoneysRublesTpl;
                _needReSave = true;
            }
            if (MoneysEurosTpl == null)
            {
                MoneysEurosTpl = DefaultValues.MoneysEurosTpl;
                _needReSave = true;
            }
            if (CommonSkillMaxValue == 0)
            {
                CommonSkillMaxValue = DefaultValues.CommonSkillMaxValue;
                _needReSave = true;
            }
            if (ColorScheme == null)
            {
                ColorScheme = DefaultValues.ColorScheme;
                _needReSave = true;
            }
            if (Language == null)
            {
                Language = ExtMethods.WindowsCulture;
                _needReSave = true;
            }
            if (BannedItems == null)
            {
                BannedItems = DefaultValues.BannedItems;
                _needReSave = true;
            }
            if (BannedMasterings == null)
            {
                BannedMasterings = DefaultValues.BannedMasterings;
                _needReSave = true;
            }
            if (FirstPrimaryWeaponSlotId == null)
            {
                FirstPrimaryWeaponSlotId = DefaultValues.FirstPrimaryWeaponSlotId;
                _needReSave = true;
            }
            if (HeadwearSlotId == null)
            {
                HeadwearSlotId = DefaultValues.HeadwearSlotId;
                _needReSave = true;
            }
            if (TacticalVestSlotId == null)
            {
                TacticalVestSlotId = DefaultValues.TacticalVestSlotId;
                _needReSave = true;
            }
            if (SecuredContainerSlotId == null)
            {
                SecuredContainerSlotId = DefaultValues.SecuredContainerSlotId;
                _needReSave = true;
            }
            if (BackpackSlotId == null)
            {
                BackpackSlotId = DefaultValues.BackpackSlotId;
                _needReSave = true;
            }
            if (EarpieceSlotId == null)
            {
                EarpieceSlotId = DefaultValues.EarpieceSlotId;
                _needReSave = true;
            }
            if (FaceCoverSlotId == null)
            {
                FaceCoverSlotId = DefaultValues.FaceCoverSlotId;
                _needReSave = true;
            }
            if (EyewearSlotId == null)
            {
                EyewearSlotId = DefaultValues.EyewearSlotId;
                _needReSave = true;
            }
            if (ArmorVestSlotId == null)
            {
                ArmorVestSlotId = DefaultValues.ArmorVestSlotId;
                _needReSave = true;
            }
            if (SecondPrimaryWeaponSlotId == null)
            {
                SecondPrimaryWeaponSlotId = DefaultValues.SecondPrimaryWeaponSlotId;
                _needReSave = true;
            }
            if (HolsterSlotId == null)
            {
                HolsterSlotId = DefaultValues.HolsterSlotId;
                _needReSave = true;
            }
            if (ScabbardSlotId == null)
            {
                ScabbardSlotId = DefaultValues.ScabbardSlotId;
                _needReSave = true;
            }
            if (ArmBandSlotId == null)
            {
                ArmBandSlotId = DefaultValues.ArmBandSlotId;
                _needReSave = true;
            }
            return _needReSave;
        }

        private void CreateDefault()
        {
            ColorScheme = DefaultValues.ColorScheme;
            Language = ExtMethods.WindowsCulture;
            DirsList = DefaultValues.DefaultDirsList;
            FilesList = DefaultValues.DefaultFilesList;
            AutoAddMissingQuests = false;
            AutoAddMissingMasterings = false;
            AutoAddMissingScavSkills = false;
            PocketsContainerTpl = DefaultValues.PocketsContainerTpl;
            CommonSkillMaxValue = DefaultValues.CommonSkillMaxValue;
            PocketsSlotId = DefaultValues.PocketsSlotId;
            FirstPrimaryWeaponSlotId = DefaultValues.FirstPrimaryWeaponSlotId;
            HeadwearSlotId = DefaultValues.HeadwearSlotId;
            TacticalVestSlotId = DefaultValues.TacticalVestSlotId;
            SecuredContainerSlotId = DefaultValues.SecuredContainerSlotId;
            BackpackSlotId = DefaultValues.BackpackSlotId;
            EarpieceSlotId = DefaultValues.EarpieceSlotId;
            FaceCoverSlotId = DefaultValues.FaceCoverSlotId;
            EyewearSlotId = DefaultValues.EyewearSlotId;
            ArmorVestSlotId = DefaultValues.ArmorVestSlotId;
            SecondPrimaryWeaponSlotId = DefaultValues.SecondPrimaryWeaponSlotId;
            HolsterSlotId = DefaultValues.HolsterSlotId;
            ScabbardSlotId = DefaultValues.ScabbardSlotId;
            ArmBandSlotId = DefaultValues.ArmBandSlotId;
            MoneysDollarsTpl = DefaultValues.MoneysDollarsTpl;
            MoneysEurosTpl = DefaultValues.MoneysEurosTpl;
            MoneysRublesTpl = DefaultValues.MoneysRublesTpl;
            BannedItems = DefaultValues.BannedItems;
            BannedMasterings = DefaultValues.BannedMasterings;
            IssuesAction = DefaultValues.DefaultIssuesAction;
            Logger.Log($"Default configuration file created");
            Save();
        }
    }
}