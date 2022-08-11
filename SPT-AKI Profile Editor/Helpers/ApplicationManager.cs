using ControlzEx.Theming;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Windows;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IApplicationManager
    {
        public RelayCommand CloseApplication { get; }

        public void OpenContainerWindow(object obj, StashEditMode editMode);

        public void OpenWeaponBuildWindow(object obj, StashEditMode editMode);

        public void CloseItemViewWindows(List<string> idsList = null);

        public void ChangeTheme();
    }

    public class ApplicationManager : IApplicationManager
    {
        public RelayCommand CloseApplication => new(obj => Application.Current.Shutdown());

        public void OpenContainerWindow(object obj, StashEditMode editMode)
        {
            if (obj == null || obj is not InventoryItem item)
                return;
            if (CheckForOpenedWindow(item.Id))
                return;
            ContainerWindow containerWindow = new(item, editMode);
            containerWindow.Show();
        }

        public void OpenWeaponBuildWindow(object obj, StashEditMode editMode)
        {
            if (obj == null || obj is not InventoryItem item)
                return;
            if (CheckForOpenedWindow(item.Id))
                return;
            WeaponBuildWindow weaponBuildWindow = new(item, editMode);
            weaponBuildWindow.Show();
        }

        public void CloseItemViewWindows(List<string> idsList = null)
        {
            // Skipping in nUnit tests
            if (Application.Current == null)
                return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Application.Current.Windows)
                    if (window is ItemViewWindow containerWindow && (idsList == null || idsList.Contains(containerWindow.ItemId)))
                        containerWindow.Close();
            });
        }

        public void ChangeTheme() => ThemeManager.Current.ChangeTheme(Application.Current, AppData.AppSettings.ColorScheme);

        private static bool CheckForOpenedWindow(string itemId)
        {
            foreach (Window window in Application.Current.Windows)
                if (window is ItemViewWindow openedWindow && openedWindow.ItemId == itemId)
                    return openedWindow.Activate();
            return false;
        }
    }
}