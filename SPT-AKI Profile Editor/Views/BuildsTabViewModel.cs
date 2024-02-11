using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    public class BuildsTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;

        public BuildsTabViewModel(IDialogManager dialogManager,
                                     IWorker worker,
                                     IWindowsDialogs windowsDialogs,
                                     IApplicationManager applicationManager)
        {
            _dialogManager = dialogManager;
            _windowsDialogs = windowsDialogs;
            _worker = worker;
            _applicationManager = applicationManager;
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

        public RelayCommand OpenContainer => new(obj =>
        {
            if (obj is (InventoryItem item, EquipmentBuild build))
                _applicationManager.OpenContainerWindow(item, build.MakeInventory(), false);
        });

        public RelayCommand InspectWeapon => new(obj =>
        {
            if (obj is (InventoryItem item, EquipmentBuild build))
                _applicationManager.OpenWeaponBuildWindow(item, build.MakeInventory(), false);
        });

        private static WorkerTask ExportTask(Action action)
            => ProgressTask(action, "tab_presets_export");

        private static WorkerTask ImportTask(Action action)
            => ProgressTask(action, "tab_presets_import");

        private void ExportBuildToFile(object obj)
        {
            if (obj is WeaponBuild build)
            {
                var (success, path) = _windowsDialogs.SaveWeaponBuildDialog(build.Name);
                if (success)
                    _worker.AddTask(ExportTask(() => UserBuilds.ExportBuild(build, path)));
                return;
            }

            if (obj is EquipmentBuild eBuild)
            {
                var (success, path) = _windowsDialogs.SaveEquipmentBuildDialog(eBuild.Name);
                if (success)
                    _worker.AddTask(ExportTask(() => UserBuilds.ExportBuild(eBuild, path)));
                return;
            }
        }

        private void ExportAllWeaponBuilds()
        {
            var (success, path) = _windowsDialogs.FolderBrowserDialog(true);
            if (success)
                foreach (var build in Profile.UserBuilds.WeaponBuilds)
                    _worker.AddTask(ExportTask(() => UserBuilds.ExportBuild(build, Path.Combine(path, $"Weapon preset {build.Name}.json"))));
        }

        private void ExportAllEquipmentBuilds()
        {
            var (success, path) = _windowsDialogs.FolderBrowserDialog(true);
            if (success)
                foreach (var build in Profile.UserBuilds.EquipmentBuilds)
                    _worker.AddTask(ExportTask(() => UserBuilds.ExportBuild(build, Path.Combine(path, $"Equipment preset {build.Name}.json"))));
        }

        private void ImportWeaponBuildsFromFiles()
        {
            foreach (var file in GetImportPaths())
                _worker.AddTask(ImportTask(() => Profile.UserBuilds.ImportWeaponBuildFromFile(file)));
        }

        private void ImportEquipmentBuildsFromFile()
        {
            foreach (var file in GetImportPaths())
                _worker.AddTask(ImportTask(() => Profile.UserBuilds.ImportEquipmentBuildFromFile(file)));
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
                _worker.AddTask(ImportTask(() => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(build)));
        }

        private async Task RemoveBuildFromProfile(object obj)
        {
            if (obj is WeaponBuild build && await AskRemoveBuild())
            {
                Profile.UserBuilds.RemoveWeaponBuild(build.Id);
                return;
            }

            if (obj is EquipmentBuild eBuild && await AskRemoveBuild())
            {
                Profile.UserBuilds.RemoveEquipmentBuild(eBuild.Id);
                return;
            }
        }

        private async Task RemoveAllWeaponBuildsFromProfile()
        {
            if (await AskRemoveBuilds())
                Profile.UserBuilds.RemoveWeaponBuilds();
        }

        private async Task RemoveAllEquipmentBuildsFromProfile()
        {
            if (await AskRemoveBuilds())
                Profile.UserBuilds.RemoveEquipmentBuilds();
        }

        private async Task<bool> AskRemoveBuild()
            => await _dialogManager.YesNoDialog("remove_preset_dialog_title", "remove_preset_dialog_caption");

        private async Task<bool> AskRemoveBuilds()
            => await _dialogManager.YesNoDialog("remove_preset_dialog_title", "remove_presets_dialog_caption");
    }
}