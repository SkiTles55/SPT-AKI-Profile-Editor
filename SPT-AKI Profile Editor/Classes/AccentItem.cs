using ControlzEx.Theming;

namespace SPT_AKI_Profile_Editor.Classes
{
    public class AccentItem
    {
        public AccentItem(Theme theme)
        {
            Name = theme.DisplayName;
            Color = theme.PrimaryAccentColor.ToString();
            Scheme = theme.Name;
        }

        public AccentItem(string name, string color, string scheme)
        {
            Name = name;
            Color = color;
            Scheme = scheme;
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public string Scheme { get; set; }
    }
}