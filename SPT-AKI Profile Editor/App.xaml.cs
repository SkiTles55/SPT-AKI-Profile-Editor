using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public static readonly IDialogManager DialogManager = new MetroDialogManager();
        public static readonly IApplicationManager ApplicationManager = new ApplicationManager();
        public static readonly IDialogCoordinator DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;
        public static readonly IWindowsDialogs WindowsDialogs = new WindowsDialogs();

        public static void HandleException(Exception exception)
        {
            string text = $"Exception Message: {exception.Message}. | StackTrace: {exception.StackTrace}";
            Logger.Log(text);
            MessageBox.Show(text, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Application_Startup(object s, StartupEventArgs e) => Current.DispatcherUnhandledException += (sender, args) => HandleException(args.Exception);
    }
}