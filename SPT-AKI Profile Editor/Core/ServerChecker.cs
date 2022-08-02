using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public class ServerChecker
    {
        public static bool CheckProcess(string name = null, string path = null)
        {
            if (string.IsNullOrEmpty(name))
                name = Path.GetFileNameWithoutExtension(AppData.AppSettings.FilesList[SPTServerFile.serverexe]);
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.FilesList[SPTServerFile.serverexe]);
            Process[] processesArray = Process.GetProcessesByName(name);
            return processesArray.Where(x => x.MainModule.FileName.ToLower() == path.ToLower()).Any();
        }
    }
}