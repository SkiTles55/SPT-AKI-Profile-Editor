using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Handbook
    {
        public List<HandbookCategory> Categories { get; set; }

        public List<HandbookItem> Items { get; set; }
    }
}