using SPT_AKI_Profile_Editor.Core;
using System;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class WindowsDialogs
    {
        private static string JsonFileText => AppData.AppLocalization.GetLocalizedString("windows_dialogs_json_file");

        public static SaveFileDialog SaveWeaponBuildDialog(string name) =>
            SaveFileDialog($"Weapon preset {name}", $"{JsonFileText} (*.json)|*.json");

        public static SaveFileDialog SaveFileDialog(string fileName, string filter = null)
        {
            return new()
            {
                FileName = fileName,
                RestoreDirectory = true,
                Filter = filter
            };
        }

        public static FolderBrowserDialog FolderBrowserDialog(bool showNewFolderButton = true, string description = null)
        {
            return new()
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = showNewFolderButton,
                Description = description
            };
        }

        public static OpenFileDialog OpenWeaponBuildDialog()
        {
            return new()
            {
                Filter = $"{JsonFileText} (*.json)|*.json",
                RestoreDirectory = true,
                Multiselect = true
            };
        }
    }
}