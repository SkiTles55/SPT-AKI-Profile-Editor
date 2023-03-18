using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility True { get; set; } = Visibility.Collapsed;
        public Visibility False { get; set; } = Visibility.Visible;

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture) => value is bool boolean && boolean ? True : False;

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture) => throw new NotImplementedException();
    }
}