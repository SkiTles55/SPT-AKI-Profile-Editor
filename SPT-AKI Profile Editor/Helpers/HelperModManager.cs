namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IHelperModManager
    {
        public HelperModStatus HelperModStatus { get; }
        public bool UpdateAvailable { get; }
        public bool IsInstalled { get; }
    }

    public class HelperModManager : IHelperModManager
    {
        public HelperModStatus HelperModStatus => HelperModStatus.NotInstalled;
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus == HelperModStatus.Installed;
    }
}