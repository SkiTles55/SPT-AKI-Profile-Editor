using SPT_AKI_Profile_Editor.Core.ProgressTransfer;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для InfoTransferSettings.xaml
    /// </summary>
    public partial class InfoTransferSettings : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title),
                                        typeof(string),
                                        typeof(InfoTransferSettings),
                                        null);

        public static readonly DependencyProperty InfoGroupProperty =
            DependencyProperty.Register(nameof(InfoGroup),
                                        typeof(InfoGroup),
                                        typeof(InfoTransferSettings),
                                        null);

        public InfoTransferSettings() => InitializeComponent();

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public InfoGroup InfoGroup
        {
            get { return (InfoGroup)GetValue(InfoGroupProperty); }
            set { SetValue(InfoGroupProperty, value); }
        }
    }
}