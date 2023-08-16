using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class UserBuilds : BindableEntity
    {
        private List<WeaponBuild> weaponBuilds;

        [JsonProperty("weaponBuilds")]
        public List<WeaponBuild> WeaponBuilds
        {
            get => weaponBuilds;
            set
            {
                weaponBuilds = value;
                WeaponBuildsChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<WeaponBuild> WBuilds => WeaponBuilds != null ? new(WeaponBuilds) : new();

        [JsonIgnore]
        public bool HasWeaponBuilds => WBuilds.Count > 0;

        public static void ExportBuild(WeaponBuild weaponBuild, string path)
        {
            try
            {
                JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented };
                JsonSerializer serializer = JsonSerializer.Create(seriSettings);
                var build = JObject.FromObject(weaponBuild, serializer).RemoveNullAndEmptyProperties();
                File.WriteAllText(path, JsonConvert.SerializeObject(build, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.Log($"WeaponBuild export error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public void RemoveWeaponBuild(string id)
        {
            var existBuild = WeaponBuilds.FirstOrDefault(x => x.Id == id);
            if (existBuild != null && WeaponBuilds.Remove(existBuild))
                WeaponBuildsChanged();
        }

        public void RemoveWeaponBuilds()
        {
            WeaponBuilds = new();
            WeaponBuildsChanged();
        }

        public void ImportWeaponBuildFromFile(string path)
        {
            try
            {
                WeaponBuild weaponBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(path));
                if (weaponBuild.Name == null)
                    throw new Exception(AppData.AppLocalization.GetLocalizedString("tab_presets_wrong_file") + ":" + Environment.NewLine + path);
                ImportBuild(weaponBuild);
            }
            catch (Exception ex)
            {
                Logger.Log($"WeaponBuild import error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public void ImportBuild(WeaponBuild weaponBuild)
        {
            WeaponBuilds ??= new();
            int count = 1;
            string tempFileName = weaponBuild.Name;
            while (WeaponBuilds.Any(x => x.Name == tempFileName))
                tempFileName = string.Format("{0}({1})", weaponBuild.Name, count++);
            weaponBuild.Name = tempFileName;
            weaponBuild.Id = ExtMethods.GenerateNewId(WeaponBuilds.Select(x => x.Id));
            WeaponBuilds.Add(weaponBuild);
            WeaponBuildsChanged();
        }

        private void WeaponBuildsChanged()
        {
            OnPropertyChanged(nameof(WeaponBuilds));
            OnPropertyChanged(nameof(WBuilds));
            OnPropertyChanged(nameof(HasWeaponBuilds));
        }
    }
}