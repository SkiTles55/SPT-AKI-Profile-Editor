﻿using SPT_AKI_Profile_Editor.Helpers;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsHelperModManager : IHelperModManager
    {
        public bool RemoveModCalled = false;
        public bool UpdateModCalled = false;
        private HelperModStatus helperModStatus;

        public TestsHelperModManager(HelperModStatus helperModStatus = HelperModStatus.NotInstalled)
            => this.helperModStatus = helperModStatus;

        public string DbPath => "";

        public HelperModStatus HelperModStatus => helperModStatus;

        public bool UpdateAvailable => false;

        public bool IsInstalled => false;

        public bool DbFilesExist => false;

        public Task DownloadUpdates() => Task.CompletedTask;

        public void InstallMod() => helperModStatus = HelperModStatus.Installed;

        public void RemoveMod()
        {
            RemoveModCalled = true;
            helperModStatus = HelperModStatus.NotInstalled;
        }

        public void UpdateMod()
        {
            UpdateModCalled = true;
            helperModStatus = HelperModStatus.Installed;
        }
    }
}