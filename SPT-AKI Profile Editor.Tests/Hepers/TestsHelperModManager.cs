using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsHelperModManager : IHelperModManager
    {
        public string DbPath => "";

        public HelperModStatus HelperModStatus => HelperModStatus.NotInstalled;

        public bool UpdateAvailable => false;

        public bool IsInstalled => false;

        public bool DbFilesExist => false;

        public void DownloadUpdates()
        {
        }

        public void InstallMod()
        {
        }

        public void RemoveMod()
        {
        }

        public void UpdateMod()
        {
        }
    }
}