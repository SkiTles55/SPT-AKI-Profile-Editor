using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для SkillGrid.xaml
    /// </summary>
    public partial class SkillGrid : GridControl
    {
        public static readonly DependencyProperty FirstCollumnTitleProperty =
            DependencyProperty.Register(nameof(FirstCollumnTitle), typeof(string), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty SecondCollumnTitleProperty =
            DependencyProperty.Register(nameof(SecondCollumnTitle), typeof(string), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty SetAllTitleProperty =
            DependencyProperty.Register(nameof(SetAllTitle), typeof(string), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty SetAllButtonTitleProperty =
            DependencyProperty.Register(nameof(SetAllButtonTitle), typeof(string), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty SetAllValueProperty =
            DependencyProperty.Register(nameof(SetAllValue), typeof(float), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(float), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty FilterValueProperty =
            DependencyProperty.Register(nameof(FilterValue), typeof(string), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty SetAllCommandProperty =
            DependencyProperty.Register(nameof(SetAllCommand), typeof(ICommand), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty HasMissingItemsProperty =
            DependencyProperty.Register(nameof(HasMissingItems), typeof(bool), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty AddMissingItemsProperty =
            DependencyProperty.Register(nameof(AddMissingItems), typeof(ICommand), typeof(SkillGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty AddMissingItemsTitleProperty =
            DependencyProperty.Register(nameof(AddMissingItemsTitle), typeof(string), typeof(SkillGrid), new PropertyMetadata(null));

        public SkillGrid()
        {
            InitializeComponent();
        }

        public string FirstCollumnTitle
        {
            get { return (string)GetValue(FirstCollumnTitleProperty); }
            set { SetValue(FirstCollumnTitleProperty, value); }
        }

        public string SecondCollumnTitle
        {
            get { return (string)GetValue(SecondCollumnTitleProperty); }
            set { SetValue(SecondCollumnTitleProperty, value); }
        }

        public string SetAllTitle
        {
            get { return (string)GetValue(SetAllTitleProperty); }
            set { SetValue(SetAllTitleProperty, value); }
        }

        public string SetAllButtonTitle
        {
            get { return (string)GetValue(SetAllButtonTitleProperty); }
            set { SetValue(SetAllButtonTitleProperty, value); }
        }

        public float SetAllValue
        {
            get { return (float)GetValue(SetAllValueProperty); }
            set { SetValue(SetAllValueProperty, value); }
        }

        public float MaxValue
        {
            get { return (float)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public string FilterValue
        {
            get { return (string)GetValue(FilterValueProperty); }
            set { SetValue(FilterValueProperty, value); }
        }

        public ICommand SetAllCommand
        {
            get { return (ICommand)GetValue(SetAllCommandProperty); }
            set { SetValue(SetAllCommandProperty, value); }
        }

        public bool HasMissingItems
        {
            get { return (bool)GetValue(HasMissingItemsProperty); }
            set { SetValue(HasMissingItemsProperty, value); }
        }

        public ICommand AddMissingItems
        {
            get { return (ICommand)GetValue(AddMissingItemsProperty); }
            set { SetValue(AddMissingItemsProperty, value); }
        }

        public string AddMissingItemsTitle
        {
            get { return (string)GetValue(AddMissingItemsTitleProperty); }
            set { SetValue(AddMissingItemsTitleProperty, value); }
        }

        private static void ApplyFilter(IEnumerable source, string filter)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(source);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(filter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    CharacterSkill p = o as CharacterSkill;
                    return p.LocalizedName.Contains(filter, System.StringComparison.CurrentCultureIgnoreCase);
                };
            }
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter(skillsGrid.ItemsSource, FilterValue);
    }
}