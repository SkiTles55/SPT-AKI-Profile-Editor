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
        [JsonIgnore]
        public bool Loaded = false;
        [JsonIgnore]
        public bool AutoAddMissingQuests = true;
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
        public Dictionary<string, string> DirsList { get; set; }
        public Dictionary<string, string> FilesList { get; set; }

        private static readonly string configurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");
        private string serverPath;
        private string defaultProfile;
        private string language;
        private string colorScheme;
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
            Logger.Log($"Default configuration file created");
            Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}