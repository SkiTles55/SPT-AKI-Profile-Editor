using System;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для MerchantsTab.xaml
    /// </summary>
    public partial class MerchantsTab : UserControl
    {
        public static readonly DependencyProperty BorderMarginProperty =
            DependencyProperty.Register(nameof(BorderMargin), typeof(double), typeof(MerchantsTab), new PropertyMetadata((double)5));

        public static readonly DependencyProperty CollumnsCountProperty =
            DependencyProperty.Register(nameof(CollumnsCount), typeof(int), typeof(MerchantsTab), new PropertyMetadata(2));

        private readonly int minWidth = 240;

        public MerchantsTab() => InitializeComponent();

        public double BorderMargin
        {
            get { return (double)GetValue(BorderMarginProperty); }
            set { SetValue(BorderMarginProperty, value); }
        }

        public int CollumnsCount
        {
            get { return (int)GetValue(CollumnsCountProperty); }
            set { SetValue(CollumnsCountProperty, value); }
        }

        private void UniformGrid_SizeChanged(object sender, SizeChangedEventArgs e) =>
            CollumnsCount = (int)Math.Floor(e.NewSize.Width / (minWidth + BorderMargin * 2));
    }
}