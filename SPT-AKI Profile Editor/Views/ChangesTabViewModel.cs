using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ChangesTabViewModel : BindableViewModel
    {
        private readonly IWindowsDialogs _windowsDialogs;
        private readonly IWorker _worker;
        private TemplateEntity profileChanges;

        public ChangesTabViewModel(IWindowsDialogs windowsDialogs, IWorker worker)
        {
            _windowsDialogs = windowsDialogs;
            _worker = worker;
        }

        public TemplateEntity ProfileChanges
        {
            get => profileChanges;
            set
            {
                profileChanges = value;
                OnPropertyChanged(nameof(ProfileChanges));
                OnPropertyChanged(nameof(HasChanges));
            }
        }

        public RelayCommand GetAllChanges => new(obj => { ProfileChanges = Profile.GetAllChanges(); });

        public RelayCommand LoadTemplate => new(obj =>
        {
            var (success, path, paths) = _windowsDialogs.OpenWeaponBuildDialog();
            if (success)
            {
                foreach (var file in paths)
                {
                    _worker.AddTask(new WorkerTask
                    {
                        Action = () => Profile.ImportBuildFromFile(file),
                        Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                        Description = AppLocalization.GetLocalizedString("tab_presets_import")
                    });
                }
            }
        });

        public RelayCommand SaveAsTemplate => new(obj =>
        {
            if (profileChanges != null)
            {
                var (success, path) = _windowsDialogs.SaveFileDialog("ProfileEditorTemplate.json", "*.json");
                if (success)
                {
                    _worker.AddTask(new WorkerTask
                    {
                        Action = () => profileChanges.Save(path),
                        Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                        Description = AppLocalization.GetLocalizedString("tab_changes_export")
                    });
                }
            }
        });

        public bool HasChanges => ProfileChanges?.NotEmpty == true;
    }
}