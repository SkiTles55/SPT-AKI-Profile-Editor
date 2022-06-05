using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для WeaponBuildView.xaml
    /// </summary>
    public partial class WeaponBuildView : UserControl
    {
        public static readonly DependencyProperty BuildProperty =
            DependencyProperty.Register(nameof(Build), typeof(WeaponBuild), typeof(WeaponBuildView), new PropertyMetadata(null));

        public static readonly DependencyProperty LocalizationProperty =
            DependencyProperty.Register(nameof(LocalizationDict), typeof(Dictionary<string, string>), typeof(WeaponBuildView), new PropertyMetadata(null));

        public static readonly DependencyProperty PartsListScrollEnabledProperty =
            DependencyProperty.Register(nameof(PartsListScrollEnabled), typeof(bool), typeof(WeaponBuildView), new PropertyMetadata(true, null));

        public WeaponBuildView()
        {
            InitializeComponent();
        }

        public WeaponBuild Build
        {
            get { return (WeaponBuild)GetValue(BuildProperty); }
            set { SetValue(BuildProperty, value); }
        }

        public bool PartsListScrollEnabled
        {
            get { return (bool)GetValue(BuildProperty); }
            set { SetValue(BuildProperty, value); }
        }

        public Dictionary<string, string> LocalizationDict
        {
            get { return (Dictionary<string, string>)GetValue(LocalizationProperty); }
            set { SetValue(LocalizationProperty, value); }
        }
    }
}