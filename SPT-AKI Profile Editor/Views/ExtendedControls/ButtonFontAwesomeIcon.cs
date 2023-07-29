using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    internal class ButtonFontAwesomeIcon : Button
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon),
                                        typeof(PackIconFontAwesomeKind),
                                        typeof(ButtonFontAwesomeIcon),
                                        new PropertyMetadata(PackIconFontAwesomeKind.PlusSolid));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize),
                                        typeof(double),
                                        typeof(ButtonFontAwesomeIcon),
                                        new PropertyMetadata((double)12));

        public PackIconFontAwesomeKind Icon
        {
            get { return (PackIconFontAwesomeKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }
    }
}