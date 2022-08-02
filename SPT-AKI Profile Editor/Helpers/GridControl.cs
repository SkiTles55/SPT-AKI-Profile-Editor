using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class GridControl : ContentControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<BindableEntity>), typeof(GridControl), new PropertyMetadata(null));

        public IEnumerable<BindableEntity> ItemsSource
        {
            get { return (IEnumerable<BindableEntity>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
    }
}