using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class ExaminedItem
    {
        public ExaminedItem(string id, string name, BitmapSource icon)
        {
            Id = id;
            Name = name;
            Icon = icon;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public BitmapSource Icon { get; set; }
    }
}