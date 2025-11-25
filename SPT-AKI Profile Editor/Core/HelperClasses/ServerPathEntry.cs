namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class ServerPathEntry(string key, string path, bool isFounded)
    {
        public string Key { get; } = key;
        public string Path { get; set; } = path;
        public bool IsFounded { get; } = isFounded;

        public string LocalizedName => AppData.AppLocalization.GetLocalizedString($"{Key}_description");
    }
}