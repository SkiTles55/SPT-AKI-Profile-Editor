using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Windows;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// WPF 应用程序入口类。
    /// 创建全局应用管理器、对话协调器和窗口对话框接口，并统一处理未捕获异常。
    /// </summary>

    public partial class App : Application
    {
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