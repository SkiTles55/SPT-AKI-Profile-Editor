using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core
{
    public class CleaningFromModsTab
    {
        public string IdFilter { get; set; }
        public string TplFilter { get; set; }
        public Dictionary<string, bool> TypesExpander { get; set; } = [];
    }
}