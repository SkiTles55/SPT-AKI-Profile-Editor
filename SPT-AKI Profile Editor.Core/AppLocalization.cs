using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core
{
    public class AppLocalization
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Translations { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Localizations { get; set; }

        private static readonly string localizationsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localizations");

        public AppLocalization() { }

        public AppLocalization(string language)
        {
            if (!Directory.Exists(localizationsDir))
            {
                DirectoryInfo dir = new DirectoryInfo(localizationsDir);
                dir.Create();
            }
            CreateDefault();
            LoadLocalization(language);
            CreateLocalizationsDictionary();
        }

        public void LoadLocalization(string key)
        {
            if (!File.Exists(Path.Combine(localizationsDir, key + ".json")))
                key = "en";
            try
            {
                AppLocalization appLocalization = LocalizationFromFile(Path.Combine(localizationsDir, key + ".json"));
                var DefaultLocalization = DefaultValues.DefaultLocalizations.Find(x => x.Key == "en");
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

        public void Save(string path, AppLocalization data) => Helpers.SaveJson(path, data);

        private void CreateDefault()
        {
            foreach (var loc in DefaultValues.DefaultLocalizations.Where(x => !File.Exists(Path.Combine(localizationsDir, x.Key + ".json"))))
            {
                try { Save(Path.Combine(localizationsDir, loc.Key + ".json"), loc); }
                catch (Exception ex) { Logger.Log($"Localization file ({loc.Key}) creating error: {ex.Message}"); }
            }
        }

        private void CreateLocalizationsDictionary()
        {
            Localizations = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles(localizationsDir))
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

        private AppLocalization LocalizationFromFile(string path) => JsonSerializer.Deserialize<AppLocalization>(File.ReadAllText(path));
    }
}