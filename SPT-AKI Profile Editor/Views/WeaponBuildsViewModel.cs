using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class WeaponBuildsViewModel : BindableViewModel
    {
        public static RelayCommand ExportBuild => new(obj => ExportBuildToFile(obj));

        public static RelayCommand ExportBuilds => new(obj => ExportAllBuilds());

        public static RelayCommand ImportBuilds => new(obj => ImportBuildsFromFiles());

        public RelayCommand RemoveBuild => new(async obj => await RemoveBuildFromProfile(obj));

        public RelayCommand RemoveBuilds => new(async obj => await RemoveAllBuildsFromProfile());

        private static void ExportBuildToFile(object obj)
        {
            if (obj == null || obj is not WeaponBuild build)
                return;
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"Weapon preset {build.Name}",
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            App.Worker.AddAction(new WorkerTask
            {
                Action = () => Profile.ExportBuild(build, saveFileDialog.FileName),
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("tab_presets_export")
            });
        }

        private static void ExportAllBuilds()
        {
            FolderBrowserDialog folderBrowserDialog = new()
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true
            };
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;
            foreach (var build in Profile.WeaponBuilds)
            {
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => Profile.ExportBuild(build.Value, Path.Combine(folderBrowserDialog.SelectedPath, $"Weapon preset {build.Value.Name}.json")),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_export")
                });
            }
        }

        private static void ImportBuildsFromFiles()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*",
                RestoreDirectory = true,
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            foreach (var file in openFileDialog.FileNames)
            {
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => Profile.ImportBuildFromFile(file),
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("tab_presets_import")
                });
            }
        }

        private async Task RemoveBuildFromProfile(object obj)
        {
            if (!string.IsNullOrEmpty(obj?.ToString()) && await Dialogs.YesNoDialog(this, "remove_preset_dialog_title", "remove_preset_dialog_caption"))
                Profile.RemoveBuild(obj.ToString());
        }

        private async Task RemoveAllBuildsFromProfile()
        {
            if (await Dialogs.YesNoDialog(this, "remove_preset_dialog_title", "remove_presets_dialog_caption"))
                Profile.RemoveBuilds();
        }
    }
}