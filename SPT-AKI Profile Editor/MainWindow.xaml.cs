using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
            DataContext = new MainWindowViewModel();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
                App.HandleException(exception);
            else
                App.HandleException(new Exception("Unknown Exception!"));
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            if (ExtMethods.IsProfileChanged(AppData.Profile) && _shutdown == false)
            {
                e.Cancel = true;
                Dispatcher.BeginInvoke(new Action(async () => await ConfirmShutdown()));
            }
        }

        private async Task ConfirmShutdown()
        {
            _shutdown = await Dialogs.YesNoDialog(DataContext,
                AppData.AppLocalization.GetLocalizedString("app_quit"),
                AppData.AppLocalization.GetLocalizedString("reload_profile_dialog_caption")); ;
            if (_shutdown)
                System.Windows.Application.Current.Shutdown();
        }
    }
}