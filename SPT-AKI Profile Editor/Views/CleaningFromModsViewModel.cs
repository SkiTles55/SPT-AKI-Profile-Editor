using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class CleaningFromModsViewModel : BindableViewModel
    {
        public CleaningService CleaningService { get; } = new();

        public RelayCommand UpdateEntityList => new(obj => CleaningService.LoadEntitesList());

        public RelayCommand SelectAll => new(obj => CleaningService.MarkAll(true));

        public RelayCommand DeselectAll => new(obj => CleaningService.MarkAll(false));

        public RelayCommand RemoveSelected => new(obj => CleaningService.RemoveSelected());
    }
}