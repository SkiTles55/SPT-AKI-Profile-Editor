using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core
{
    public class AppSettings : INotifyPropertyChanged
    {
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
                bool _needReload = autoAddMissingQuests != value;
                autoAddMissingQuests = value;
                OnPropertyChanged("AutoAddMissingQuests");
                if (Loaded)
                {
                    if (_needReload)
                        LoadProfiles();
                    Save();
                }
            }
        }
        public bool AutoAddMissingMasterings
        {
            get => autoAddMissingMasterings;
            set
            {
                bool _needReload = autoAddMissingMasterings != value;
                autoAddMissingMasterings = value;
                OnPropertyChanged("AutoAddMissingWeaponSkills");
                if (Loaded)
                {
                    if (_needReload)
                        LoadProfiles();
                    Save();
                }
            }
        }
        public bool AutoAddMissingScavSkills
        {
            get => autoAddMissingScavSkills;
            set
            {
                bool _needReload = autoAddMissingScavSkills != value;
                autoAddMissingScavSkills = value;
                OnPropertyChanged("AutoAddMissingScavSkills");
                if (Loaded)
                {
                    if (_needReload)
                        LoadProfiles();
                    Save();
                }
            }
        }
        public string PocketsContainerTpl
        {
            get => pocketsContainerTpl;
            set
            {
                bool _needReload = pocketsContainerTpl != value;
                pocketsContainerTpl = value;
                OnPropertyChanged("PocketsContainerTpl");
                if (Loaded)
                {
                    if (_needReload)
                        LoadProfiles();
                    Save();
                }
            }
        }
        public float CommonSkillMaxValue
        {
            get => commonSkillMaxValue;
            set
            {
                bool _needReload = commonSkillMaxValue != value;
                commonSkillMaxValue = value;
                OnPropertyChanged("CommonSkillMaxValue");
                if (Loaded)
                {
                    if (_needReload)
                        LoadProfiles();
                    Save();
                }
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
        public bool Loaded = false;

        private static readonly string configurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");
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

        public static string GetStamp()
        {
            return AppData.AppSettings.ServerPath
                + AppData.AppSettings.DefaultProfile
                + AppData.AppSettings.Language
                + AppData.AppSettings.AutoAddMissingQuests.ToString()
                + AppData.AppSettings.AutoAddMissingMasterings.ToString()
                + AppData.AppSettings.AutoAddMissingScavSkills.ToString()
                + AppData.AppSettings.CommonSkillMaxValue.ToString()
                + AppData.AppSettings.PocketsContainerTpl;
        }

        private void LoadFromFile()
        {
            try
            {
                string cfg = File.ReadAllText(configurationFile);
                AppSettings loaded = JsonSerializer.Deserialize<AppSettings>(cfg);
                bool _needReSave = false;
                if (loaded.DirsList == null)
                {
                    loaded.DirsList = new();
                    _needReSave = true;
                }
                if (loaded.FilesList == null)
                {
                    loaded.FilesList = new();
                    _needReSave = true;
                }
                foreach (var dir in DefaultValues.DefaultDirsList.Where(x => !loaded.DirsList.ContainsKey(x.Key)))
                {
                    loaded.DirsList.Add(dir.Key, dir.Value);
                    _needReSave = true;
                }
                foreach (var file in DefaultValues.DefaultFilesList.Where(x => !loaded.FilesList.ContainsKey(x.Key)))
                {
                    loaded.FilesList.Add(file.Key, file.Value);
                    _needReSave = true;
                }
                if (loaded.ColorScheme == null)
                {
                    loaded.ColorScheme = DefaultValues.ColorScheme;
                    _needReSave = true;
                }
                if (loaded.Language == null)
                {
                    loaded.Language = ExtMethods.WindowsCulture;
                    _needReSave = true;
                }
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
                CommonSkillMaxValue = loaded.CommonSkillMaxValue;
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
            Logger.Log($"Default configuration file created");
            Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}