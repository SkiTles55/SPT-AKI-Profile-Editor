using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerDatabase
    {
        public Dictionary<string, string> Heads { get; set; }
        public Dictionary<string, string> Voices { get; set; }
        public LocalesGlobal Global { get; set; }
    }
}