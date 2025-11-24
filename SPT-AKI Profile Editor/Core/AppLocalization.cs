using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public class AppLocalization : BindableEntity
    {
        [JsonIgnore]
        public readonly string localizationsDir;

        private Dictionary<string, string> translations;
        private Dictionary<string, string> localizations;

        public AppLocalization()
        { }

        public AppLocalization(string language, string localizationsDir)
        {
            this.localizationsDir = localizationsDir;
            if (!Directory.Exists(localizationsDir))
            {
                DirectoryInfo dir = new(localizationsDir);
                dir.Create();
            }
            CreateDefault();
            CreateLocalizationsDictionary();
            LoadLocalization(language);
        }

        public string Key { get; set; }
        public string Name { get; set; }

        public Dictionary<string, string> Translations
        {
            get => translations;
            set
            {
                translations = value;
                OnPropertyChanged(nameof(Translations));
            }
        }

        [JsonIgnore]
        public Dictionary<string, string> Localizations
        {
            get => localizations;
            set
            {
                localizations = value;
                OnPropertyChanged(nameof(Localizations));
            }
        }

        public static void Save(string path, AppLocalization data) => File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));

        public string GetLocalizedString(string key, params string[] args) => Translations != null && Translations.TryGetValue(key, out string value) ? string.Format(value, args) : key;

        public void LoadLocalization(string key)
        {
            if (!File.Exists(Path.Combine(localizationsDir, key + ".json")))
            {
                key = "en";
                AppData.AppSettings.Language = "en";
            }
            try
            {
                AppLocalization appLocalization = LocalizationFromFile(Path.Combine(localizationsDir, key + ".json"));
                AppLocalization DefaultLocalization = DefaultValues.DefaultLocalizations().Find(x => x.Key == key)
                    ?? DefaultValues.DefaultLocalizations().Find(x => x.Key == "en");
                bool _needReSave = false;
                foreach (var st in DefaultLocalization.Translations.Where(x => !appLocalization.Translations.ContainsKey(x.Key)))
                {
                    appLocalization.Translations.Add(st.Key, st.Value);
                    _needReSave = true;
                }
                foreach (var deprecated in appLocalization.Translations.Where(x => !DefaultLocalization.Translations.ContainsKey(x.Key)))
                {
                    appLocalization.Translations.Remove(deprecated.Key);
                    _needReSave = true;
                }
                if (_needReSave)
                {
                    try
                    {
                        Save(Path.Combine(localizationsDir, appLocalization.Key + ".json"), appLocalization);
                        Logger.Log($"Localization file ({appLocalization.Key}) updated");
                    }
                    catch (Exception ex) { Logger.Log($"Localization file ({appLocalization.Key}) updating error: {ex.Message}"); }
                }
                Key = appLocalization.Key;
                Name = appLocalization.Name;
                Translations = appLocalization.Translations;
            }
            catch (Exception ex) { Logger.Log($"Localization ({key}) loading error: {ex.Message}"); }
        }

        public void Update(Dictionary<string, string> newValues)
        {
            Translations = newValues;
            Save(Path.Combine(localizationsDir, Key + ".json"), this);
        }

        public void AddNew(string key, string name, Dictionary<string, string> values, SettingsDialogViewModel settingsDialog = null)
        {
            AppLocalization newLocalization = new() { Key = key, Name = name, Translations = values };
            Save(Path.Combine(localizationsDir, key + ".json"), newLocalization);
            CreateLocalizationsDictionary();
            if (settingsDialog != null)
                settingsDialog.CurrentLocalization = key;
        }

        private static AppLocalization LocalizationFromFile(string path) => JsonConvert.DeserializeObject<AppLocalization>(File.ReadAllText(path));

        private void CreateDefault()
        {
            foreach (var loc in DefaultValues.DefaultLocalizations().Where(x => !File.Exists(Path.Combine(localizationsDir, x.Key + ".json"))))
            {
                try { Save(Path.Combine(localizationsDir, loc.Key + ".json"), loc); }
                catch (Exception ex) { Logger.Log($"Localization file ({loc.Key}) creating error: {ex.Message}"); }
            }
        }

        private void CreateLocalizationsDictionary()
        {
            Dictionary<string, string> localizations = [];
            foreach (string file in Directory.GetFiles(localizationsDir))
            {
                try
                {
                    AppLocalization appLocalization = LocalizationFromFile(file);
                    if (!localizations.ContainsKey(appLocalization.Key))
                        localizations.Add(appLocalization.Key, appLocalization.Name);
                    else
                        Logger.Log($"Duplicated localization file founded ({file})");
                }
                catch (Exception ex) { Logger.Log($"Localization file ({file}) loading error: {ex.Message}"); }
            }
            Localizations = localizations;
        }
    }
}