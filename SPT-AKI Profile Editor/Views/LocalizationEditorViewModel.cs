using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    public class LocalizationEditorViewModel : BindableViewModel
    {
        public AppLocalization Localization { get; set; }
        public bool CanSelectKey { get; set; }
        public Dictionary<string, string> AvailableKeys { get; set; }

        public LocalizationEditorViewModel(AppLocalization appLocalization = null)
        {
            Localization = appLocalization ?? AppLocalization;
            CanSelectKey = appLocalization == null;
        }
    }
}