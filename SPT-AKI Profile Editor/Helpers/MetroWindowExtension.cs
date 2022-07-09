using MahApps.Metro.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class MetroWindowExtension
    {
        public static void AllowDragging(this MetroWindow window)
        {
            window.MouseLeftButtonDown += delegate
            {
                if (Mouse.GetPosition(window).Y <= window.TitleBarHeight)
                    window.DragMove();
            };
        }
    }
}