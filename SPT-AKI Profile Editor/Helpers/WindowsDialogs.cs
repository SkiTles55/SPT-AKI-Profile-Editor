using SPT_AKI_Profile_Editor.Core;
using System;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IWindowsDialogs
    {
        public (bool success, string path) SaveWeaponBuildDialog(string name);

        public (bool success, string path) SaveEquipmentBuildDialog(string name);

        public (bool success, string path) SaveFileDialog(string fileName, string filter = null);

        public (bool success, string path) FolderBrowserDialog(bool showNewFolderButton = true, string startPath = null, string description = null);

        public (bool success, string path, string[] paths) OpenBuildDialog();
    }

    public class WindowsDialogs : IWindowsDialogs
    {
        private static string JsonFileText => AppData.AppLocalization.GetLocalizedString("windows_dialogs_json_file");

        public (bool success, string path, string[] paths) OpenBuildDialog()
        {
            var dialog = OpenFileWindowsDialog();
            return dialog.ShowDialog() == DialogResult.OK ? (true, dialog.FileName, dialog.FileNames) : (false, dialog.FileName, dialog.FileNames);
        }

        public (bool success, string path) FolderBrowserDialog(bool showNewFolderButton = true, string startPath = null, string description = null)
        {
            var dialog = FolderBrowserWindowsDialog(showNewFolderButton, description);
            if (startPath != null)
                dialog.SelectedPath = startPath;
            return dialog.ShowDialog() == DialogResult.OK ? (true, dialog.SelectedPath) : (false, dialog.SelectedPath);
        }

        public (bool success, string path) SaveWeaponBuildDialog(string name) =>
                            SaveFileDialog($"Weapon preset {name}", $"{JsonFileText} (*.json)|*.json");

        public (bool success, string path) SaveEquipmentBuildDialog(string name) =>
                            SaveFileDialog($"Equipment preset {name}", $"{JsonFileText} (*.json)|*.json");

        public (bool success, string path) SaveFileDialog(string fileName, string filter = null)
        {
            var dialog = SaveFileWindowsDialog(fileName, filter);
            return dialog.ShowDialog() == DialogResult.OK ? (true, dialog.FileName) : (false, dialog.FileName);
        }

        private static SaveFileDialog SaveFileWindowsDialog(string fileName, string filter = null) => new()
        {
            FileName = fileName,
            RestoreDirectory = true,
            Filter = filter
        };

        private static OpenFileDialog OpenFileWindowsDialog() => new()
        {
            Filter = $"{JsonFileText} (*.json)|*.json",
            RestoreDirectory = true,
            Multiselect = true
        };

        private static FolderBrowserDialog FolderBrowserWindowsDialog(bool showNewFolderButton, string description) => new()
        {
            RootFolder = Environment.SpecialFolder.MyComputer,
            ShowNewFolderButton = showNewFolderButton,
            Description = description
        };
    }
}