using System;
using System.Windows.Forms;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class WindowsDialogs
    {
        public static SaveFileDialog SaveWeaponBuildDialog(string name) =>
            SaveFileDialog($"Weapon preset {name}", "Файл JSON (*.json)|*.json|All files (*.*)|*.*");

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
                Filter = "Файл JSON (*.json)|*.json|All files (*.*)|*.*",
                RestoreDirectory = true,
                Multiselect = true
            };
        }
    }
}