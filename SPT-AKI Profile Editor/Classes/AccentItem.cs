using ControlzEx.Theming;

namespace SPT_AKI_Profile_Editor.Classes
{
    internal class AccentItem
    {
        public AccentItem(Theme theme)
        {
            Name = theme.DisplayName;
            Color = theme.PrimaryAccentColor.ToString();
            Scheme = theme.Name;
        }

        public string Name { get; set; }
        public string Color { get; set; }
        public string Scheme { get; set; }
    }
}