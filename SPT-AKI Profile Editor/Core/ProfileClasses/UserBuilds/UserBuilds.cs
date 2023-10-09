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
        private List<EquipmentBuild> equipmentBuilds;

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

        [JsonProperty("equipmentBuilds")]
        public List<EquipmentBuild> EquipmentBuilds
        {
            get => equipmentBuilds;
            set
            {
                equipmentBuilds = value;
                EquipmentBuildsChanged();
            }
        }

        [JsonIgnore]
        public ObservableCollection<WeaponBuild> WBuilds => WeaponBuilds != null ? new(WeaponBuilds) : new();

        [JsonIgnore]
        public ObservableCollection<EquipmentBuild> EBuilds => EquipmentBuilds != null ? new(EquipmentBuilds) : new();

        [JsonIgnore]
        public bool HasWeaponBuilds => WBuilds.Count > 0;

        [JsonIgnore]
        public bool HasEquipmentBuilds => EBuilds.Count > 0;

        public static void ExportBuild(WeaponBuild weaponBuild, string path)
            => ExportBuild(weaponBuild, path, nameof(WeaponBuild));

        public static void ExportBuild(EquipmentBuild equipmentBuild, string path)
            => ExportBuild(equipmentBuild, path, nameof(EquipmentBuild));

        public void RemoveWeaponBuild(string id)
        {
            var existBuild = WeaponBuilds.FirstOrDefault(x => x.Id == id);
            if (existBuild != null && WeaponBuilds.Remove(existBuild))
                WeaponBuildsChanged();
        }

        public void RemoveWeaponBuilds() => WeaponBuilds = new();

        public void RemoveEquipmentBuild(string id)
        {
            var existBuild = EquipmentBuilds.FirstOrDefault(x => x.Id == id);
            if (existBuild != null && EquipmentBuilds.Remove(existBuild))
                EquipmentBuildsChanged();
        }

        public void RemoveEquipmentBuilds() => EquipmentBuilds = new();

        public void ImportWeaponBuildFromFile(string path)
        {
            var weaponBuild = GetBuildFromFile<WeaponBuild>(path, WeaponBuild.WeaponBuildType);
            if (weaponBuild != null)
                ImportBuild(weaponBuild);
        }

        public void ImportEquipmentBuildFromFile(string path)
        {
            var equipmentBuild = GetBuildFromFile<EquipmentBuild>(path, EquipmentBuild.EquipmentBuildType);
            if (equipmentBuild != null)
                ImportBuild(equipmentBuild);
        }

        public void ImportBuild(WeaponBuild weaponBuild)
        {
            WeaponBuilds ??= new();
            weaponBuild.PrepareForImport(WeaponBuilds);
            WeaponBuilds.Add(weaponBuild);
            WeaponBuildsChanged();
        }

        public void ImportBuild(EquipmentBuild equipmentBuild)
        {
            EquipmentBuilds ??= new();
            equipmentBuild.PrepareForImport(EquipmentBuilds);
            EquipmentBuilds.Add(equipmentBuild);
            EquipmentBuildsChanged();
        }

        private static void ExportBuild(object buildObject, string path, string typeName)
        {
            try
            {
                JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented };
                JsonSerializer serializer = JsonSerializer.Create(seriSettings);
                var build = JObject.FromObject(buildObject, serializer).RemoveNullAndEmptyProperties();
                File.WriteAllText(path, JsonConvert.SerializeObject(build, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.Log($"{typeName} export error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        private T GetBuildFromFile<T>(string path, string expectedType) where T : Build
        {
            try
            {
                T build = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                if (build.Name == null || build.Type != expectedType)
                    throw new Exception(AppData.AppLocalization.GetLocalizedString("tab_presets_file_not_build") + ":" + Environment.NewLine + path);
                return build;
            }
            catch (Exception ex)
            {
                Logger.Log($"{nameof(T)} import error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        private void WeaponBuildsChanged()
        {
            OnPropertyChanged(nameof(WeaponBuilds));
            OnPropertyChanged(nameof(WBuilds));
            OnPropertyChanged(nameof(HasWeaponBuilds));
        }

        private void EquipmentBuildsChanged()
        {
            OnPropertyChanged(nameof(EquipmentBuilds));
            OnPropertyChanged(nameof(EBuilds));
            OnPropertyChanged(nameof(HasEquipmentBuilds));
        }
    }
}