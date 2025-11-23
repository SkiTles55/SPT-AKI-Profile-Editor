using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    public class InfoTabViewModel : BindableViewModel
    {
        public static List<string> Sides => [PMCSide.Bear.ToString(), PMCSide.Usec.ToString()];
    }
}