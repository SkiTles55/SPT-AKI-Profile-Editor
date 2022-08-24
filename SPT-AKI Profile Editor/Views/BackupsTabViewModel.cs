using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class BackupsTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWorker _worker;

        public BackupsTabViewModel(IDialogManager dialogManager, IWorker worker)
        {
            _dialogManager = dialogManager;
            _worker = worker;
        }

        public static BackupService BackupService => AppData.BackupService;

        public RelayCommand RemoveCommand => new(async obj =>
        {
            if (obj is string file && await _dialogManager.YesNoDialog("remove_backup_dialog_title", "remove_backup_dialog_caption"))
                RemoveBackupAction(file);
        });

        public RelayCommand RestoreCommand => new(async obj =>
        {
            if (obj is string file && await _dialogManager.YesNoDialog("restore_backup_dialog_title", "restore_backup_dialog_caption"))
                RestoreBackupAction(file);
        });

        private void RemoveBackupAction(string file)
        {
            _worker.AddTask(new WorkerTask
            {
                Action = () => { BackupService.RemoveBackup(file); },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("remove_backup_dialog_title")
            });
        }

        private void RestoreBackupAction(string file)
        {
            _worker.AddTask(new WorkerTask
            {
                Action = () => { BackupService.RestoreBackup(file); },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("restore_backup_dialog_title")
            });
            _worker.AddTask(new WorkerTask
            {
                Action = AppData.StartupEvents,
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
            });
        }
    }
}