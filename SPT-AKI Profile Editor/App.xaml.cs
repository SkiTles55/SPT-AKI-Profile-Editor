using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public static readonly IDialogManager DialogManager = new MetroDialogManager();
        public static RelayCommand CloseApplication => new(obj => Current.Shutdown());
        public static IDialogCoordinator DialogCoordinator => MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;

        public static void ChangeTheme() => ThemeManager.Current.ChangeTheme(Current, AppData.AppSettings.ColorScheme);

        public static void OpenContainerWindow(object obj, StashEditMode editMode)
        {
            if (obj == null || obj is not InventoryItem item)
                return;
            if (CheckForOpenedWindow(item.Id))
                return;
            ContainerWindow containerWindow = new(item, editMode);
            containerWindow.Show();
        }

        public static void OpenWeaponBuildWindow(object obj, StashEditMode editMode)
        {
            if (obj == null || obj is not InventoryItem item)
                return;
            if (CheckForOpenedWindow(item.Id))
                return;
            WeaponBuildWindow weaponBuildWindow = new(item, editMode);
            weaponBuildWindow.Show();
        }

        public static bool CheckForOpenedWindow(string itemId)
        {
            foreach (Window window in Current.Windows)
                if (window is ItemViewWindow openedWindow && openedWindow.ItemId == itemId)
                    return openedWindow.Activate();
            return false;
        }

        public static void CloseItemViewWindows(List<string> idsList = null)
        {
            // Skipping in nUnit tests
            if (Current == null)
                return;
            Current.Dispatcher.Invoke(() =>
            {
                foreach (Window window in Current.Windows)
                    if (window is ItemViewWindow containerWindow && (idsList == null || idsList.Contains(containerWindow.ItemId)))
                        containerWindow.Close();
            });
        }

        public static void HandleException(Exception exception)
        {
            string text = $"Exception Message: {exception.Message}. | StackTrace: {exception.StackTrace}";
            Logger.Log(text);
            MessageBox.Show(text, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Application_Startup(object s, StartupEventArgs e) => Current.DispatcherUnhandledException += (sender, args) => HandleException(args.Exception);
    }
}