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
    public class AppSettings(string configurationFile) : BindableEntity
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
        public readonly string sptProjectUrl = "https://www.sp-tarkov.com/";

        [JsonIgnore]
        public readonly string modHelperUpdateUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/master/SPT-AKI%20Profile%20Editor.ModHelper/bin/Debug/net9.0/";

        [JsonIgnore]
        public readonly string modHelperHowToUseUrl = "https://github.com/SkiTles55/SPT-AKI-Profile-Editor/blob/master/Guidelines/ModHelper";

        [JsonIgnore]
        public readonly List<QuestStatus> standartQuestStatuses =
        [
            QuestStatus.Locked,
            QuestStatus.AvailableForStart,
            QuestStatus.Started,
            QuestStatus.Fail,
            QuestStatus.AvailableForFinish,
            QuestStatus.Success
        ];

        [JsonIgnore]
        public readonly List<QuestStatus> repeatableQuestStatuses =
        [
            QuestStatus.AvailableForStart,
            QuestStatus.Started,
            QuestStatus.Fail,
            QuestStatus.AvailableForFinish,
            QuestStatus.Success
        ];

        [JsonIgnore]
        public bool Loaded = false;

        private string serverPath;
        private string defaultProfile;
        private string language;
        private string colorScheme;
        private bool autoAddMissingScavSkills;
        private bool autoAddMissingMasterings;
        private Dictionary<string, string> serverProfiles;
        private IssuesAction issuesAction;
        private bool fastModeOpened = false;
        private bool? checkUpdates;
        private bool usingModHelper;

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

        public string PocketsContainerTpl { get; set; }

        public float CommonSkillMaxValue { get; set; }

        public string PocketsSlotId { get; set; }

        public string FirstPrimaryWeaponSlotId { get; set; }

        public string HeadwearSlotId { get; set; }

        public string TacticalVestSlotId { get; set; }

        public string SecuredContainerSlotId { get; set; }

        public string BackpackSlotId { get; set; }

        public string EarpieceSlotId { get; set; }

        public string FaceCoverSlotId { get; set; }

        public string EyewearSlotId { get; set; }

        public string ArmorVestSlotId { get; set; }

        public string SecondPrimaryWeaponSlotId { get; set; }

        public string HolsterSlotId { get; set; }

        public string ScabbardSlotId { get; set; }

        public string ArmBandSlotId { get; set; }

        public string MoneysDollarsTpl { get; set; }

        public string MoneysRublesTpl { get; set; }

        public string MoneysEurosTpl { get; set; }

        public string EndlessDevBackpackId { get; set; }

        public string BearDogtagTpl { get; set; }

        public List<string> BannedItems { get; set; }

        public string FenceTraderId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public IssuesAction IssuesAction
        {
            get => issuesAction;
            set
            {
                issuesAction = value;
                NotifyPropertyChangedAndSave(nameof(IssuesAction));
            }
        }

        public List<string> SkipMigrationTags { get; set; }

        public string MannequinInventoryTpl { get; set; }

        public int HideoutAreaEquipmentPresetsType { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> ServerProfiles
        {
            get => serverProfiles;
            set
            {
                serverProfiles = value;
                OnPropertyChanged(nameof(ServerProfiles));
            }
        }

        [JsonIgnore]
        public bool FastModeOpened
        {
            get => fastModeOpened;
            set
            {
                fastModeOpened = value;
                OnPropertyChanged(nameof(FastModeOpened));
            }
        }

        public string GetStamp() => JsonConvert.SerializeObject(this, Formatting.None);

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
            Dictionary<string, string> Profiles = [];
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

        public void SkipMigrationTag(string tag)
        {
            SkipMigrationTags.Add(tag);
            Save();
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
            IssuesAction = loaded.IssuesAction;
            BearDogtagTpl = loaded.BearDogtagTpl;
            EndlessDevBackpackId = loaded.EndlessDevBackpackId;
            FenceTraderId = loaded.FenceTraderId;
            SkipMigrationTags = loaded.SkipMigrationTags;
            MannequinInventoryTpl = loaded.MannequinInventoryTpl;
            HideoutAreaEquipmentPresetsType = loaded.HideoutAreaEquipmentPresetsType;
        }

        private bool CheckValues()
        {
            bool _needReSave = false;
            if (DirsList == null)
            {
                DirsList = [];
                _needReSave = true;
            }
            if (FilesList == null)
            {
                FilesList = [];
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
            if (FenceTraderId == null)
            {
                FenceTraderId = DefaultValues.FenceTraderId;
                _needReSave = true;
            }
            if (SkipMigrationTags == null)
            {
                SkipMigrationTags = [];
                _needReSave = true;
            }
            if (MannequinInventoryTpl == null)
            {
                MannequinInventoryTpl = DefaultValues.MannequinInventoryTpl;
                _needReSave = true;
            }
            if (HideoutAreaEquipmentPresetsType == 0)
            {
                HideoutAreaEquipmentPresetsType = DefaultValues.HideoutAreaEquipmentPresetsType;
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
            IssuesAction = DefaultValues.DefaultIssuesAction;
            BearDogtagTpl = DefaultValues.BearDogtagTpl;
            EndlessDevBackpackId = DefaultValues.EndlessDevBackpackId;
            FenceTraderId = DefaultValues.FenceTraderId;
            SkipMigrationTags = [];
            MannequinInventoryTpl = DefaultValues.MannequinInventoryTpl;
            HideoutAreaEquipmentPresetsType = DefaultValues.HideoutAreaEquipmentPresetsType;
            Logger.Log($"Default configuration file created");
            Save();
        }
    }
}