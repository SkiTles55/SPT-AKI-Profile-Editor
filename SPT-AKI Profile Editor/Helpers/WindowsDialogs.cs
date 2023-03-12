using SPT_AKI_Profile_Editor.Core;
using System;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IWindowsDialogs
    {
        public (bool success, string path) SaveWeaponBuildDialog(string name);

        public (bool success, string path) SaveTemplateDialog();

        public (bool success, string path) SaveFileDialog(string fileName, string filter = null);

        public (bool success, string path) FolderBrowserDialog(bool showNewFolderButton = true, string startPath = null, string description = null);

        public (bool success, string path, string[] paths) OpenWeaponBuildDialog();

        public (bool success, string path) OpenTemplateDialog();
    }

    public class WindowsDialogs : IWindowsDialogs
    {
        public (bool success, string path, string[] paths) OpenWeaponBuildDialog()
        {
            var dialog = OpenFileWindowsDialog();
            return dialog.ShowDialog() == DialogResult.OK ? (true, dialog.FileName, dialog.FileNames) : (false, dialog.FileName, dialog.FileNames);
        }
        public (bool success, string path) OpenTemplateDialog()
        {
            var dialog = OpenFileWindowsDialog(false);
            return dialog.ShowDialog() == DialogResult.OK ? (true, dialog.FileName) : (false, dialog.FileName);
        }

        public (bool success, string path) FolderBrowserDialog(bool showNewFolderButton = true, string startPath = null, string description = null)
        {
            var dialog = FolderBrowserWindowsDialog(showNewFolderButton, description);
            if (startPath != null)
                dialog.SelectedPath = startPath;
            return dialog.ShowDialog() == DialogResult.OK ? (true, dialog.SelectedPath) : (false, dialog.SelectedPath);
        }

        public (bool success, string path) SaveWeaponBuildDialog(string name) =>
                            SaveFileDialog($"Weapon preset {name}", GetJsonFilter());

        public (bool success, string path) SaveTemplateDialog() =>
                            SaveFileDialog($"ProfileEditorTemplate", GetJsonFilter());

        public (bool success, string path) SaveFileDialog(string fileName, string filter = null)
        {
            var dialog = SaveFileWindowsDialog(fileName, filter);
            return dialog.ShowDialog() == DialogResult.OK ? (true, dialog.FileName) : (false, dialog.FileName);
        }

        private static string GetJsonFilter() => $"{AppData.AppLocalization.GetLocalizedString("windows_dialogs_json_file")} (*.json)|*.json";

        private static SaveFileDialog SaveFileWindowsDialog(string fileName, string filter = null) => new()
        {
            FileName = fileName,
            RestoreDirectory = true,
            Filter = filter
        };

        private static OpenFileDialog OpenFileWindowsDialog(bool multiselect = true) => new()
        {
            Filter = GetJsonFilter(),
            RestoreDirectory = true,
            Multiselect = multiselect
        };

        private static FolderBrowserDialog FolderBrowserWindowsDialog(bool showNewFolderButton, string description) => new()
        {
            RootFolder = Environment.SpecialFolder.MyComputer,
            ShowNewFolderButton = showNewFolderButton,
            Description = description
        };
    }
}