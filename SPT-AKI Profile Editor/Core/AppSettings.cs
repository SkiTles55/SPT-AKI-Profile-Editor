using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public class AppSettings : BindableEntity
    {
        [JsonIgnore]
        public readonly string repoAuthor = "SkiTles55";

        [JsonIgnore]
        public readonly string repoName = "SPT-AKI-Profile-Editor";

        [JsonIgnore]
        public readonly string steamTradeUrl = "https://steamcommunity.com/tradeoffer/new/?partner=350485380%26token=zCrhUwxR";

        [JsonIgnore]
        public readonly string yoomoneyUrl = "https://yoomoney.ru/to/410015658095326";

        [JsonIgnore]
        public readonly string ltcWallet = "MNtz8Zz1cPD1CZadoc38jT5qeqeFBS6Aif";

        [JsonIgnore]
        public readonly string sptAkiProjectUrl = "https://www.sp-tarkov.com/";

        [JsonIgnore]
        public readonly List<QuestStatus> standartQuestStatuses = new()
        {
            QuestStatus.Locked,
            QuestStatus.AvailableForStart,
            QuestStatus.Started,
            QuestStatus.Fail,
            QuestStatus.AvailableForFinish,
            QuestStatus.Success
        };

        [JsonIgnore]
        public readonly List<QuestStatus> repeatableQuestStatuses = new()
        {
            QuestStatus.AvailableForStart,
            QuestStatus.Started,
            QuestStatus.Fail,
            QuestStatus.AvailableForFinish,
            QuestStatus.Success
        };

        [JsonIgnore]
        public readonly string configurationFile;

        [JsonIgnore]
        public bool Loaded = false;

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
        private bool? checkUpdates;
        private string ragfairTraderId;
        private string bearDogtagTpl;
        private string endlessDevBackpackId;
        private bool usingModHelper;

        public AppSettings(string configurationFile) => this.configurationFile = configurationFile;

        public string ServerPath
        {
            get => serverPath;
            set
            {
                bool _needReload = serverPath != value;
                serverPath = value;
                OnPropertyChanged(nameof(ServerPath));
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
                NotifyPropertyChangedAndSave(nameof(DefaultProfile));
            }
        }

        public string Language
        {
            get => language;
            set
            {
                language = value;
                NotifyPropertyChangedAndSave(nameof(Language));
            }
        }

        public string ColorScheme
        {
            get => colorScheme;
            set
            {
                colorScheme = value;
                NotifyPropertyChangedAndSave(nameof(ColorScheme));
            }
        }

        public bool? CheckUpdates
        {
            get => checkUpdates;
            set
            {
                checkUpdates = value;
                NotifyPropertyChangedAndSave(nameof(CheckUpdates));
            }
        }

        public bool UsingModHelper
        {
            get => usingModHelper;
            set
            {
                usingModHelper = value;
                NotifyPropertyChangedAndSave(nameof(UsingModHelper));
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
                NotifyPropertyChangedAndSave(nameof(AutoAddMissingQuests));
            }
        }

        public bool AutoAddMissingMasterings
        {
            get => autoAddMissingMasterings;
            set
            {
                autoAddMissingMasterings = value;
                NotifyPropertyChangedAndSave(nameof(AutoAddMissingMasterings));
            }
        }

        public bool AutoAddMissingScavSkills
        {
            get => autoAddMissingScavSkills;
            set
            {
                autoAddMissingScavSkills = value;
                NotifyPropertyChangedAndSave(nameof(AutoAddMissingScavSkills));
            }
        }

        public string PocketsContainerTpl
        {
            get => pocketsContainerTpl;
            set
            {
                pocketsContainerTpl = value;
                NotifyPropertyChangedAndSave(nameof(PocketsContainerTpl));
            }
        }

        public float CommonSkillMaxValue
        {
            get => commonSkillMaxValue;
            set
            {
                commonSkillMaxValue = value;
                NotifyPropertyChangedAndSave(nameof(CommonSkillMaxValue));
            }
        }

        public string PocketsSlotId
        {
            get => pocketsSlotId;
            set
            {
                pocketsSlotId = value;
                NotifyPropertyChangedAndSave("PocketsSlotId");
            }
        }

        public string FirstPrimaryWeaponSlotId
        {
            get => firstPrimaryWeaponSlotId;
            set
            {
                firstPrimaryWeaponSlotId = value;
                NotifyPropertyChangedAndSave("FirstPrimaryWeaponSlotId");
            }
        }

        public string HeadwearSlotId
        {
            get => headwearSlotId;
            set
            {
                headwearSlotId = value;
                NotifyPropertyChangedAndSave("HeadwearSlotId");
            }
        }

        public string TacticalVestSlotId
        {
            get => tacticalVestSlotId;
            set
            {
                tacticalVestSlotId = value;
                NotifyPropertyChangedAndSave("TacticalVestSlotId");
            }
        }

        public string SecuredContainerSlotId
        {
            get => securedContainerSlotId;
            set
            {
                securedContainerSlotId = value;
                NotifyPropertyChangedAndSave("SecuredContainerSlotId");
            }
        }

        public string BackpackSlotId
        {
            get => backpackSlotId;
            set
            {
                backpackSlotId = value;
                NotifyPropertyChangedAndSave("BackpackSlotId");
            }
        }

        public string EarpieceSlotId
        {
            get => earpieceSlotId;
            set
            {
                earpieceSlotId = value;
                NotifyPropertyChangedAndSave("EarpieceSlotId");
            }
        }

        public string FaceCoverSlotId
        {
            get => faceCoverSlotId;
            set
            {
                faceCoverSlotId = value;
                NotifyPropertyChangedAndSave("FaceCoverSlotId");
            }
        }

        public string EyewearSlotId
        {
            get => eyewearSlotId;
            set
            {
                eyewearSlotId = value;
                NotifyPropertyChangedAndSave("EyewearSlotId");
            }
        }

        public string ArmorVestSlotId
        {
            get => armorVestSlotId;
            set
            {
                armorVestSlotId = value;
                NotifyPropertyChangedAndSave("ArmorVestSlotId");
            }
        }

        public string SecondPrimaryWeaponSlotId
        {
            get => secondPrimaryWeaponSlotId;
            set
            {
                secondPrimaryWeaponSlotId = value;
                NotifyPropertyChangedAndSave("SecondPrimaryWeaponSlotId");
            }
        }

        public string HolsterSlotId
        {
            get => holsterSlotId;
            set
            {
                holsterSlotId = value;
                NotifyPropertyChangedAndSave("HolsterSlotId");
            }
        }

        public string ScabbardSlotId
        {
            get => scabbardSlotId;
            set
            {
                scabbardSlotId = value;
                NotifyPropertyChangedAndSave("ScabbardSlotId");
            }
        }

        public string ArmBandSlotId
        {
            get => armBandSlotId;
            set
            {
                armBandSlotId = value;
                NotifyPropertyChangedAndSave("ArmBandSlotId");
            }
        }

        public string MoneysDollarsTpl
        {
            get => moneysDollarsTpl;
            set
            {
                moneysDollarsTpl = value;
                NotifyPropertyChangedAndSave("MoneysDollarsTpl");
            }
        }

        public string MoneysRublesTpl
        {
            get => moneysRublesTpl;
            set
            {
                moneysRublesTpl = value;
                NotifyPropertyChangedAndSave("MoneysRublesTpl");
            }
        }

        public string MoneysEurosTpl
        {
            get => moneysEurosTpl;
            set
            {
                moneysEurosTpl = value;
                NotifyPropertyChangedAndSave("MoneysEurosTpl");
            }
        }

        public string RagfairTraderId
        {
            get => ragfairTraderId;
            set
            {
                ragfairTraderId = value;
                NotifyPropertyChangedAndSave("RagfairTraderId");
            }
        }

        public string EndlessDevBackpackId
        {
            get => endlessDevBackpackId;
            set
            {
                endlessDevBackpackId = value;
                NotifyPropertyChangedAndSave("EndlessDevBackpackId");
            }
        }

        public string BearDogtagTpl
        {
            get => bearDogtagTpl;
            set
            {
                bearDogtagTpl = value;
                NotifyPropertyChangedAndSave("BearDogtagTpl");
            }
        }

        public List<string> BannedItems
        {
            get => bannedItems;
            set
            {
                bannedItems = value;
                NotifyPropertyChangedAndSave("BannedItems");
            }
        }

        public List<string> BannedMasterings
        {
            get => bannedMasterings;
            set
            {
                bannedMasterings = value;
                NotifyPropertyChangedAndSave("BannedMasterings");
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public IssuesAction IssuesAction
        {
            get => issuesAction;
            set
            {
                issuesAction = value;
                NotifyPropertyChangedAndSave("IssuesAction");
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

        public string GetStamp()
        {
            return ServerPath
                + DefaultProfile
                + Language
                + UsingModHelper
                + AutoAddMissingQuests.ToString()
                + AutoAddMissingMasterings.ToString()
                + AutoAddMissingScavSkills.ToString()
                + CommonSkillMaxValue.ToString()
                + PocketsContainerTpl
                + PocketsSlotId
                + EarpieceSlotId
                + HeadwearSlotId
                + FaceCoverSlotId
                + TacticalVestSlotId
                + FirstPrimaryWeaponSlotId
                + BackpackSlotId
                + SecuredContainerSlotId
                + EyewearSlotId
                + ArmorVestSlotId
                + SecondPrimaryWeaponSlotId
                + HolsterSlotId
                + ScabbardSlotId
                + ArmBandSlotId
                + MoneysDollarsTpl
                + MoneysEurosTpl
                + MoneysRublesTpl
                + RagfairTraderId
                + BearDogtagTpl
                + EndlessDevBackpackId;
        }

        public bool ServerHaveProfiles() => ServerProfiles != null && ServerProfiles.Count > 0;

        public bool PathIsServerFolder(string path = null) => CheckServerPath(path)?.All(x => x.IsFounded) == true;

        public List<ServerPathEntry> CheckServerPath(string path = null)
        {
            if (string.IsNullOrEmpty(path)) path = ServerPath;
            if (string.IsNullOrEmpty(path)) return null;
            if (!Directory.Exists(path)) return null;
            var result = new List<ServerPathEntry>();

            result.AddRange(FilesList.Select(x => new ServerPathEntry(x.Key, x.Value, File.Exists(Path.Combine(path, x.Value)))));
            result.AddRange(DirsList.Select(x => new ServerPathEntry(x.Key, x.Value, Directory.Exists(Path.Combine(path, x.Value)))));

            return result;
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

        public void Save() => File.WriteAllText(configurationFile, JsonConvert.SerializeObject(this, Formatting.Indented));

        public void LoadProfiles()
        {
            Dictionary<string, string> Profiles = new();
            if (string.IsNullOrEmpty(ServerPath)) return;
            var profilesPath = Path.Combine(ServerPath, DirsList[SPTServerDir.profiles]);
            if (!Directory.Exists(profilesPath)) return;
            foreach (var file in Directory.GetFiles(profilesPath))
            {
                try
                {
                    string profileFile = File.ReadAllText(file);
                    ServerProfile serverProfile = JsonConvert.DeserializeObject<ServerProfile>(profileFile);
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

        private void NotifyPropertyChangedAndSave(string prop)
        {
            OnPropertyChanged(prop);
            if (Loaded)
                Save();
        }

        private void LoadFromFile()
        {
            try
            {
                string cfg = File.ReadAllText(configurationFile);
                AppSettings loaded = JsonConvert.DeserializeObject<AppSettings>(cfg);
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
            CheckUpdates = loaded.CheckUpdates;
            UsingModHelper = loaded.UsingModHelper;
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
            RagfairTraderId = loaded.RagfairTraderId;
            BannedItems = loaded.BannedItems;
            BannedMasterings = loaded.bannedMasterings;
            IssuesAction = loaded.IssuesAction;
            BearDogtagTpl = loaded.BearDogtagTpl;
            EndlessDevBackpackId = loaded.EndlessDevBackpackId;
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
            if (CheckUpdates == null)
            {
                CheckUpdates = DefaultValues.CheckUpdates;
                _needReSave = true;
            }
            if (RagfairTraderId == null)
            {
                RagfairTraderId = DefaultValues.RagfairTraderId;
                _needReSave = true;
            }
            if (BearDogtagTpl == null)
            {
                BearDogtagTpl = DefaultValues.BearDogtagTpl;
                _needReSave = true;
            }
            if (EndlessDevBackpackId == null)
            {
                EndlessDevBackpackId = DefaultValues.EndlessDevBackpackId;
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
            CheckUpdates = DefaultValues.CheckUpdates;
            UsingModHelper = false;
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
            RagfairTraderId = DefaultValues.RagfairTraderId;
            BannedItems = DefaultValues.BannedItems;
            BannedMasterings = DefaultValues.BannedMasterings;
            IssuesAction = DefaultValues.DefaultIssuesAction;
            BearDogtagTpl = DefaultValues.BearDogtagTpl;
            EndlessDevBackpackId = DefaultValues.EndlessDevBackpackId;
            Logger.Log($"Default configuration file created");
            Save();
        }
    }
}