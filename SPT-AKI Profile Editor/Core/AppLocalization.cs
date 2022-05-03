using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core
{
    public class AppLocalization : BindableEntity
    {
        public static readonly string localizationsDir = Path.Combine(DefaultValues.AppDataFolder, "Localizations");
        private Dictionary<string, string> translations;

        public AppLocalization()
        { }

        public AppLocalization(string language)
        {
            if (!Directory.Exists(localizationsDir))
            {
                DirectoryInfo dir = new(localizationsDir);
                dir.Create();
            }
            CreateDefault();
            LoadLocalization(language);
            CreateLocalizationsDictionary();
        }

        public string Key { get; set; }
        public string Name { get; set; }

        public Dictionary<string, string> Translations
        {
            get => translations;
            set
            {
                translations = value;
                OnPropertyChanged("Translations");
            }
        }

        [JsonIgnore]
        public Dictionary<string, string> Localizations { get; set; }

        public static void Save(string path, AppLocalization data) => ExtMethods.SaveJson(path, data);

        public string GetLocalizedString(string key, params string[] args) => Translations != null && Translations.ContainsKey(key) ? string.Format(Translations[key], args) : key;

        public void LoadLocalization(string key)
        {
            if (!File.Exists(Path.Combine(localizationsDir, key + ".json")))
                key = "en";
            try
            {
                AppLocalization appLocalization = LocalizationFromFile(Path.Combine(localizationsDir, key + ".json"));
                AppLocalization DefaultLocalization = DefaultValues.DefaultLocalizations.Find(x => x.Key == key)
                    ?? DefaultValues.DefaultLocalizations.Find(x => x.Key == "en");
                bool _needReSave = false;
                foreach (var st in DefaultLocalization.Translations.Where(x => !appLocalization.Translations.ContainsKey(x.Key)))
                {
                    appLocalization.Translations.Add(st.Key, st.Value);
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
                Translations = appLocalization.Translations;
            }
            catch (Exception ex) { Logger.Log($"Localization ({key}) loading error: {ex.Message}"); }
        }

        private static void CreateDefault()
        {
            foreach (var loc in DefaultValues.DefaultLocalizations.Where(x => !File.Exists(Path.Combine(localizationsDir, x.Key + ".json"))))
            {
                try { Save(Path.Combine(localizationsDir, loc.Key + ".json"), loc); }
                catch (Exception ex) { Logger.Log($"Localization file ({loc.Key}) creating error: {ex.Message}"); }
            }
        }

        private static AppLocalization LocalizationFromFile(string path) => JsonSerializer.Deserialize<AppLocalization>(File.ReadAllText(path));

        private void CreateLocalizationsDictionary()
        {
            Localizations = new Dictionary<string, string>();
            foreach (string file in Directory.GetFiles(localizationsDir))
            {
                try
                {
                    AppLocalization appLocalization = LocalizationFromFile(file);
                    if (!Localizations.ContainsKey(appLocalization.Key))
                        Localizations.Add(appLocalization.Key, appLocalization.Name);
                    else
                        Logger.Log($"Duplicated localization file founded ({file})");
                }
                catch (Exception ex) { Logger.Log($"Localization file ({file}) loading error: {ex.Message}"); }
            }
        }
    }
}