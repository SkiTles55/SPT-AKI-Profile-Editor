using ControlzEx.Theming;
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
        public static RelayCommand CloseApplication => new(obj =>
        {
            Current.Shutdown();
        });
        public static void ChangeTheme() => ThemeManager.Current.ChangeTheme(Current, AppData.AppSettings.ColorScheme);

        private void Application_Startup(object s, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Current.DispatcherUnhandledException += (sender, args) => HandleException(args.Exception);
        }
        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
                HandleException(exception);
            else
                HandleException(new Exception("Unknown Exception!"));
        }
        private static void HandleException(Exception exception)
        {
            string text = $"Exception Message: {exception.Message}. | StackTrace: {exception.StackTrace}";
            Logger.Log(text);
            MessageBox.Show(text, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
