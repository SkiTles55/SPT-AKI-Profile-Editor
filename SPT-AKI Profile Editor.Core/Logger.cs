using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Core
{
    class Logger
    {
        static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        static string _fileName;

        static Logger()
        {
            DirectoryInfo dir = new DirectoryInfo(LogPath);
            if (!dir.Exists)
                dir.Create();
            _fileName = "log" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        }

        public static void Log(string text)
        {
            try
            {
                StreamWriter file = new StreamWriter(Path.Combine(LogPath, _fileName), true);
                file.WriteLine(DateTime.Now.ToString() + ": " + text);
                file.Close();
            }
            catch (Exception) { }
        }
    }
}