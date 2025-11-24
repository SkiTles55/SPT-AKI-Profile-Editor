using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    public class CleaningFromModsViewModel(IDialogManager dialogManager,
        RelayCommand saveCommand,
        ICleaningService cleaningService) : BindableViewModel
    {
        public static AppSettings AppSettings => AppData.AppSettings;
        public ICleaningService CleaningService { get; } = cleaningService;
        public RelayCommand UpdateEntityList => new(obj => CleaningService?.LoadEntitiesList());

        public RelayCommand SelectAll => new(obj => CleaningService?.MarkAll(true,
                                                                             GetTypeFromCommand(obj)));

        public RelayCommand DeselectAll => new(obj => CleaningService?.MarkAll(false,
                                                                               GetTypeFromCommand(obj)));

        public RelayCommand RemoveSelected => new(obj => CleaningService?.RemoveSelected(saveCommand, dialogManager));

        private static ModdedEntityType? GetTypeFromCommand(object source)
        {
            if (source is CollectionViewGroup group
                && group.ItemCount > 0
                && group.Items[0] is ModdedEntity entity)
                return entity.Type;
            return null;
        }
    }
}