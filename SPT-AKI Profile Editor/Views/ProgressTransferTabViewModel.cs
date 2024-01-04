using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ProgressTransfer;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ProgressTransferTabViewModel : BindableViewModel
    {
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IWorker _worker;

        public ProgressTransferTabViewModel(IWindowsDialogs windowsDialogs, IWorker worker)
        {
            _windowsDialogs = windowsDialogs;
            _worker = worker;
        }

        public SettingsModel SettingsModel { get; } = new();

        public RelayCommand SelectAll => new(_ => SettingsModel.ChangeAll(true));

        public RelayCommand DeselectAll => new(_ => SettingsModel.ChangeAll(false));

        public RelayCommand ExportProgress => new(_ => ExportProgressTask());

        public RelayCommand ImportProgress => new(_ => ImportProgressTask());

        private void ExportProgressTask()
        {
            var (success, path) = _windowsDialogs.SaveProfileProgressDialog(Profile.Characters.Pmc.Info.Nickname);
            if (success)
                _worker.AddTask(ProgressTask(() => ProgressTransferService.ExportProgress(SettingsModel, Profile, path),
                                             "tab_presets_export"));
        }

        private void ImportProgressTask()
        {
            var (success, path, _) = _windowsDialogs.OpenBuildDialog(false);
            if (success)
                _worker.AddTask(ProgressTask(() => ProgressTransferService.ImportProgress(SettingsModel, Profile, path),
                                             "tab_presets_import"));
        }
    }
}