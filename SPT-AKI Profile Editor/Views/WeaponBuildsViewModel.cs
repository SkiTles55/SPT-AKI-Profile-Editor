using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Views
{
    public class WeaponBuildsViewModel : BindableViewModel
    {
        public readonly IDialogManager _dialogManager;
        public readonly IWorker _worker;

        public WeaponBuildsViewModel(IDialogManager dialogManager, IWorker worker)
        {
            _dialogManager = dialogManager;
            _worker = worker;
        }

        public RelayCommand AddBuildToStash => new(obj => AddBuildToProfileStash(obj));

        public RelayCommand ExportBuild => new(obj => ExportBuildToFile(obj));

        public RelayCommand ExportBuilds => new(obj => ExportAllBuilds());

        public RelayCommand ImportBuilds => new(obj => ImportBuildsFromFiles());

        public RelayCommand RemoveBuild => new(async obj => await RemoveBuildFromProfile(obj));

        public RelayCommand RemoveBuilds => new(async obj => await RemoveAllBuildsFromProfile());

        private void ExportBuildToFile(object obj)
        {
            if (obj == null || obj is not WeaponBuild build)
                return;
            var saveBuildDialog = WindowsDialogs.SaveWeaponBuildDialog(build.Name);
            if (saveBuildDialog.ShowDialog() != DialogResult.OK)
                return;
            _worker.AddTask(new WorkerTask
            {
                Action = () => Profile.ExportBuild(build, saveBuildDialog.FileName),
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("tab_presets_export")
            });
        }

        private void ExportAllBuilds()
        {
            var folderBrowserDialog = WindowsDialogs.FolderBrowserDialog(true);
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            foreach (var build in Profile.WeaponBuilds)
            {
                _worker.AddTask(new WorkerTask
                {
                    Action = () => Profile.ExportBuild(build.Value, Path.Combine(folderBrowserDialog.SelectedPath, $"Weapon preset {build.Value.Name}.json")),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_export")
                });
            }
        }

        private void ImportBuildsFromFiles()
        {
            var openFileDialog = WindowsDialogs.OpenWeaponBuildDialog();
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            foreach (var file in openFileDialog.FileNames)
            {
                _worker.AddTask(new WorkerTask
                {
                    Action = () => Profile.ImportBuildFromFile(file),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_import")
                });
            }
        }

        private void AddBuildToProfileStash(object obj)
        {
            if (obj is WeaponBuild build)
            {
                _worker.AddTask(new WorkerTask
                {
                    Action = () => Profile.Characters.Pmc.Inventory.AddNewItemsToStash(build),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_import")
                });
            }
        }

        private async Task RemoveBuildFromProfile(object obj)
        {
            if (!string.IsNullOrEmpty(obj?.ToString()) && await _dialogManager.YesNoDialog(this, "remove_preset_dialog_title", "remove_preset_dialog_caption"))
                Profile.RemoveBuild(obj.ToString());
        }

        private async Task RemoveAllBuildsFromProfile()
        {
            if (await _dialogManager.YesNoDialog(this, "remove_preset_dialog_title", "remove_presets_dialog_caption"))
                Profile.RemoveBuilds();
        }
    }
}