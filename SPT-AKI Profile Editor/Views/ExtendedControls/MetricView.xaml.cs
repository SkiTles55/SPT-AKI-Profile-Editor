using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для MetricView.xaml
    /// </summary>
    public partial class MetricView : UserControl
    {
        public static readonly DependencyProperty CharacterMetricProperty =
            DependencyProperty.Register(nameof(CharacterMetric), typeof(CharacterMetric), typeof(MetricView), new PropertyMetadata(null));

        public static readonly DependencyProperty MetricNameProperty =
            DependencyProperty.Register(nameof(MetricName), typeof(string), typeof(MetricView), new PropertyMetadata(null));

        public MetricView()
        {
            InitializeComponent();
        }

        public CharacterMetric CharacterMetric
        {
            get { return (CharacterMetric)GetValue(CharacterMetricProperty); }
            set { SetValue(CharacterMetricProperty, value); }
        }

        public string MetricName
        {
            get { return (string)GetValue(MetricNameProperty); }
            set { SetValue(MetricNameProperty, value); }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}