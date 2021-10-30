using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SPT_AKI_Profile_Editor.Core
{
    public class AppSettings
    {
        public string ServerPath { get; set; }
        public string DefaultProfile { get; set; }
        public string Language { get; set; }
        public string ColorScheme { get; set; }

        public Dictionary<string, string> DirsList { get; set; }
        public Dictionary<string, string> FilesList { get; set; }

        private static readonly string configurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");

        public void Load()
        {
            if (File.Exists(configurationFile))
                LoadFromFile();
            else
                CreateDefault();
        }

        public void Save() => Helpers.SaveJson(configurationFile, this);

        private void LoadFromFile()
        {
            try
            {
                string cfg = File.ReadAllText(configurationFile);
                AppSettings loaded = JsonSerializer.Deserialize<AppSettings>(cfg);
                bool _needReSave = false;
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
                ColorScheme = loaded.ColorScheme;
                if (loaded.Language == null)
                {
                    loaded.Language = Helpers.WindowsCulture;
                    _needReSave = true;
                }
                Language = loaded.Language;
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
            Language = Helpers.WindowsCulture;
            DirsList = DefaultValues.DefaultDirsList;
            FilesList = DefaultValues.DefaultFilesList;
            Logger.Log($"Default configuration file created");
            Save();
        }
    }
}