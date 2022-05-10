using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class BackupsTabViewModel : BindableViewModel
    {
        public static BackupService BackupService => AppData.BackupService;

        public RelayCommand RemoveCommand => new(async obj =>
         {
             if (obj == null)
                 return;
             if (await Dialogs.YesNoDialog(this, "remove_backup_dialog_title", "remove_backup_dialog_caption"))
             {
                 App.Worker.AddAction(new WorkerTask
                 {
                     Action = () => { BackupService.RemoveBackup(obj.ToString()); },
                     Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                     Description = AppLocalization.GetLocalizedString("remove_backup_dialog_title")
                 });
             }
         });

        public RelayCommand RestoreCommand => new(async obj =>
         {
             if (obj == null)
                 return;
             if (await Dialogs.YesNoDialog(this, "restore_backup_dialog_title", "restore_backup_dialog_caption"))
             {
                 App.Worker.AddAction(new WorkerTask
                 {
                     Action = () => { BackupService.RestoreBackup(obj.ToString()); },
                     Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                     Description = AppLocalization.GetLocalizedString("restore_backup_dialog_title")
                 });
                 App.Worker.AddAction(new WorkerTask
                 {
                     Action = AppData.StartupEvents,
                     Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                     Description = AppLocalization.GetLocalizedString("progress_dialog_caption")
                 });
             }
         });
    }
}