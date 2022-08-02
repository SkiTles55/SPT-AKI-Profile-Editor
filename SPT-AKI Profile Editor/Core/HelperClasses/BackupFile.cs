using System;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class BackupFile
    {
        public string Path { get; set; }
        public DateTime Date { get; set; }

        public string FormatedDate => Date.ToString("dd.MM.yyyy HH:mm:ss");
    }
}