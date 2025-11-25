using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class ExaminedItem(string id, string name, BitmapSource icon)
    {
        public string Id { get; set; } = id;
        public string Name { get; set; } = name;
        public BitmapSource Icon { get; set; } = icon;
    }
}