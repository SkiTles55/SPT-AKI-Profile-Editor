namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IHelperModManager
    {
        public HelperModStatus HelperModStatus { get; }
        public bool UpdateAvailable { get; }
        public bool IsInstalled { get; }
        public bool DbFilesExist { get; }

        public void InstallMod();
        public void RemoveMod();
        public void UpdateMod();
    }

    public class HelperModManager : IHelperModManager
    {
        public HelperModStatus HelperModStatus => HelperModStatus.NotInstalled;
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus == HelperModStatus.Installed;

        public bool DbFilesExist => false;

        public void InstallMod()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveMod()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateMod()
        {
            throw new System.NotImplementedException();
        }
    }
}