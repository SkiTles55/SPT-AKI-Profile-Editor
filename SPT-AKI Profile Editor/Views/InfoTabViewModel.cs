using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    class InfoTabViewModel : BindableViewModel
    {
        public static List<string> Sides => new() { "Bear", "Usec" };
    }
}