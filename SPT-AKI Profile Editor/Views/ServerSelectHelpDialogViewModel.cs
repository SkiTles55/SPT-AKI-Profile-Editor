using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ServerSelectHelpDialogViewModel : ClosableDialogViewModel
    {
        private readonly AppSettings AppSettings;

        public ServerSelectHelpDialogViewModel(AppSettings appSettings, object context) : base(context)
        {
            AppSettings = appSettings;
        }

        public string WhatIsServerFolder
        {
            get
            {
                var prefix = AppLocalization.GetLocalizedString("tab_settings_server") + " - ";
                var serverExe = AppSettings.FilesList.ContainsKey(SPTServerFile.serverexe)
                    ? AppSettings.FilesList[SPTServerFile.serverexe]
                    : "\"settings file corrupted\"";
                return prefix + AppLocalization.GetLocalizedString("what_is_server_folder", serverExe);
            }
        }
    }
}