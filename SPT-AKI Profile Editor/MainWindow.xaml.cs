using MahApps.Metro.Controls;
using System;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
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
    }
}