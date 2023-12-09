using SPT_AKI_Profile_Editor.Core.ProgressTransfer;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ProgressTransferTabViewModel : BindableViewModel
    {
        public SettingsModel SettingsModel { get; } = new();

        public RelayCommand SelectAll => new(_ => SettingsModel.ChangeAll(true));

        public RelayCommand DeselectAll => new(_ => SettingsModel.ChangeAll(false));

        public RelayCommand ExportProgress => new(_ => { });

        public RelayCommand ImportProgress => new(_ => { });
    }
}