using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class BackupsTabViewModel(IDialogManager dialogManager,
        IWorker worker,
        ICleaningService cleaningService) : BindableViewModel
    {
        public static BackupService BackupService => AppData.BackupService;

        public RelayCommand RemoveCommand => new(async obj =>
        {
            if (obj is string file && await dialogManager.YesNoDialog("remove_backup_dialog_title", "remove_backup_dialog_caption"))
                RemoveBackupAction(file);
        });

        public RelayCommand RestoreCommand => new(async obj =>
        {
            if (obj is string file && await dialogManager.YesNoDialog("restore_backup_dialog_title", "restore_backup_dialog_caption"))
                RestoreBackupAction(file);
        });

        private void RemoveBackupAction(string file)
            => worker.AddTask(ProgressTask(() => BackupService.RemoveBackup(file),
                AppLocalization.GetLocalizedString("remove_backup_dialog_title")));

        private void RestoreBackupAction(string file)
        {
            worker.AddTask(ProgressTask(() => BackupService.RestoreBackup(file),
                AppLocalization.GetLocalizedString("restore_backup_dialog_title")));
            worker.AddTask(ProgressTask(() => AppData.StartupEvents(cleaningService),
                AppLocalization.GetLocalizedString("progress_dialog_caption")));
        }
    }
}