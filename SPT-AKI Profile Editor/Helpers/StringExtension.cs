namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class StringExtension
    {
        public static string Name(this string value) => $"{value} Name";

        public static string Description(this string value) => $"{value} Description";

        public static string Nickname(this string value) => $"{value} Nickname";

        public static string NameLowercased(this string value) => $"{value} name";

        public static string DescriptionLowercased(this string value) => $"{value} description";
    }
}