using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class CleaningFromModsViewModel : BindableViewModel
    {
        public CleaningService CleaningService { get; } = new();

        public RelayCommand UpdateEntityList => new(obj => CleaningService.LoadEntitesList());
    }
}