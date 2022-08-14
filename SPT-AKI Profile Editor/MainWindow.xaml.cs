using MahApps.Metro.Controls;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool _shutdown;

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            InitializeComponent();
            DataContext = new MainWindowViewModel(App.DialogManager, App.ApplicationManager);
            this.AllowDragging();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            App.HandleException(exception ?? new Exception("Unknown Exception!"));
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel && AppData.Profile.IsProfileChanged() && _shutdown == false)
            {
                e.Cancel = true;
                Dispatcher.BeginInvoke(new Action(async () => await ConfirmShutdown()));
                return;
            }

            App.ApplicationManager.CloseItemViewWindows();
        }

        private async Task ConfirmShutdown()
        {
            _shutdown = await App.DialogManager.YesNoDialog(DataContext,
                AppData.AppLocalization.GetLocalizedString("app_quit"),
                AppData.AppLocalization.GetLocalizedString("reload_profile_dialog_caption")); ;
            if (_shutdown)
                System.Windows.Application.Current.Shutdown();
        }
    }
}