using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class CleaningFromModsViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly RelayCommand _saveCommand;

        public CleaningFromModsViewModel(IDialogManager dialogManager,
                                         RelayCommand saveCommand,
                                         ICleaningService cleaningService)
        {
            _dialogManager = dialogManager;
            _saveCommand = saveCommand;
            CleaningService = cleaningService;
        }

        public ICleaningService CleaningService { get; }
        public RelayCommand UpdateEntityList => new(obj => CleaningService?.LoadEntitiesList());

        public RelayCommand SelectAll => new(obj => CleaningService?.MarkAll(true));

        public RelayCommand DeselectAll => new(obj => CleaningService?.MarkAll(false));

        public RelayCommand RemoveSelected => new(obj => CleaningService?.RemoveSelected(_saveCommand, _dialogManager));
    }
}