using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ServerSelectHelpDialogViewModel(AppSettings appSettings, object context) : ClosableDialogViewModel(context)
    {
        public string WhatIsServerFolder
        {
            get
            {
                var prefix = AppLocalization.GetLocalizedString("tab_settings_server") + " - ";
                var serverExe = appSettings.FilesList.TryGetValue(SPTServerFile.serverexe, out string value)
                    ? value : "\"settings file corrupted\"";
                return prefix + AppLocalization.GetLocalizedString("what_is_server_folder", serverExe);
            }
        }
    }
}