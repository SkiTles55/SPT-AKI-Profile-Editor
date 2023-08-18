using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    public class WeaponBuildsViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IWorker _worker;

        public WeaponBuildsViewModel(IDialogManager dialogManager,
                                     IWorker worker,
                                     IWindowsDialogs windowsDialogs)
        {
            _dialogManager = dialogManager;
            _windowsDialogs = windowsDialogs;
            _worker = worker;
        }

        public RelayCommand AddBuildToStash
            => new(obj => AddBuildToProfileStash(obj));

        public RelayCommand ExportBuild
            => new(obj => ExportBuildToFile(obj));

        public RelayCommand ExportWeaponBuilds
            => new(obj => ExportAllWeaponBuilds());

        public RelayCommand ExportEquipmentBuilds
            => new(obj => ExportAllEquipmentBuilds());

        public RelayCommand ImportWeaponBuilds
            => new(obj => ImportWeaponBuildsFromFiles());

        public RelayCommand ImportEquipmentBuilds
            => new(obj => ImportEquipmentBuildsFromFile());

        public RelayCommand RemoveBuild
            => new(async obj => await RemoveBuildFromProfile(obj));

        public RelayCommand RemoveWeaponBuilds
            => new(async obj => await RemoveAllWeaponBuildsFromProfile());

        public RelayCommand RemoveEquipmentBuilds
            => new(async obj => await RemoveAllEquipmentBuildsFromProfile());

        private void ExportBuildToFile(object obj)
        {
            if (obj != null && obj is WeaponBuild build)
            {
                var (success, path) = _windowsDialogs.SaveWeaponBuildDialog(build.Name);
                if (success)
                    _worker.AddTask(new(() => UserBuilds.ExportBuild(build, path),
                                         AppLocalization.GetLocalizedString("progress_dialog_title"),
                                         AppLocalization.GetLocalizedString("tab_presets_export")));
            }
        }

        private void ExportAllWeaponBuilds()
        {
            var (success, path) = _windowsDialogs.FolderBrowserDialog(true);
            if (success)
                foreach (var build in Profile.UserBuilds.WeaponBuilds)
                    _worker.AddTask(new(() => UserBuilds.ExportBuild(build, Path.Combine(path, $"Weapon preset {build.Name}.json")),
                                        AppLocalization.GetLocalizedString("progress_dialog_title"),
                                        AppLocalization.GetLocalizedString("tab_presets_export")));
        }

        private void ExportAllEquipmentBuilds()
        {
            var (success, path) = _windowsDialogs.FolderBrowserDialog(true);
            if (success)
                foreach (var build in Profile.UserBuilds.EquipmentBuilds)
                    _worker.AddTask(new(() => UserBuilds.ExportBuild(build, Path.Combine(path, $"Equipment preset {build.Name}.json")),
                                        AppLocalization.GetLocalizedString("progress_dialog_title"),
                                        AppLocalization.GetLocalizedString("tab_presets_export")));
        }

        private void ImportWeaponBuildsFromFiles()
        {
            foreach (var file in GetImportPaths())
                _worker.AddTask(new(() => Profile.UserBuilds.ImportWeaponBuildFromFile(file),
                                    AppLocalization.GetLocalizedString("progress_dialog_title"),
                                    AppLocalization.GetLocalizedString("tab_presets_import")));
        }

        private void ImportEquipmentBuildsFromFile()
        {
            foreach (var file in GetImportPaths())
                _worker.AddTask(new(() => Profile.UserBuilds.ImportEquipmentBuildFromFile(file),
                                    AppLocalization.GetLocalizedString("progress_dialog_title"),
                                    AppLocalization.GetLocalizedString("tab_presets_import")));
        }

        private string[] GetImportPaths()
        {
            var (success, _, paths) = _windowsDialogs.OpenBuildDialog();
            if (success)
                return paths;
            return Array.Empty<string>();
        }

        private void AddBuildToProfileStash(object obj)
        {
            if (obj is WeaponBuild build)
                _worker.AddTask(new(() => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(build),
                                    AppLocalization.GetLocalizedString("progress_dialog_title"),
                                    AppLocalization.GetLocalizedString("tab_presets_import")));
        }

        private async Task RemoveBuildFromProfile(object obj)
        {
            if (!string.IsNullOrEmpty(obj?.ToString())
                && await _dialogManager.YesNoDialog("remove_preset_dialog_title", "remove_preset_dialog_caption"))
                Profile.UserBuilds.RemoveWeaponBuild(obj.ToString());
        }

        private async Task RemoveAllWeaponBuildsFromProfile()
        {
            if (await _dialogManager.YesNoDialog("remove_preset_dialog_title", "remove_presets_dialog_caption"))
                Profile.UserBuilds.RemoveWeaponBuilds();
        }

        private async Task RemoveAllEquipmentBuildsFromProfile()
        {
            if (await _dialogManager.YesNoDialog("remove_preset_dialog_title", "remove_presets_dialog_caption"))
                Profile.UserBuilds.RemoveEquipmentBuilds();
        }
    }
}