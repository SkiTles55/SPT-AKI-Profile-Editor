namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class StringExtension
    {
        public static string Name(this string value) => $"{value} Name";

        public static string Nickname(this string value) => $"{value} Nickname";

        public static string QuestName(this string value) => $"{value} name";
    }
}