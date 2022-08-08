using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class BackupsTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;

        public BackupsTabViewModel(IDialogManager dialogManager) => _dialogManager = dialogManager;

        public static BackupService BackupService => AppData.BackupService;

        public RelayCommand RemoveCommand => new(async obj =>
        {
            if (obj is string file && await _dialogManager.YesNoDialog(this, "remove_backup_dialog_title", "remove_backup_dialog_caption"))
                RemoveBackupAction(file);
        });

        public RelayCommand RestoreCommand => new(async obj =>
        {
            if (obj is string file && await _dialogManager.YesNoDialog(this, "restore_backup_dialog_title", "restore_backup_dialog_caption"))
                RestoreBackupAction(obj);
        });

        private static void RemoveBackupAction(string file)
        {
            App.Worker.AddTask(new WorkerTask
            {
                Action = () => { BackupService.RemoveBackup(file); },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("remove_backup_dialog_title")
            });
        }

        private static void RestoreBackupAction(object obj)
        {
            App.Worker.AddTask(new WorkerTask
            {
                Action = () => { BackupService.RestoreBackup(obj.ToString()); },
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("restore_backup_dialog_title")
            });
            App.Worker.AddTask(new WorkerTask
            {
                Action = AppData.StartupEvents,
                Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
            });
        }
    }
}