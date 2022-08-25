using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IWorker
    {
        public void AddTask(WorkerTask task);
    }

    public class Worker : IWorker
    {
        private readonly IDialogManager _dialogManager;
        private readonly List<WorkerTask> tasks;
        private readonly List<WorkerNotification> workerNotifications;
        private bool isBusy = false;

        public Worker(IDialogManager dialogManager)
        {
            tasks = new List<WorkerTask>();
            workerNotifications = new List<WorkerNotification>();
            _dialogManager = dialogManager;
        }

        public void AddTask(WorkerTask task)
        {
            tasks.Add(task);
            if (!isBusy)
                RunWorkerAsync();
        }

        private async void RunWorkerAsync()
        {
            isBusy = true;
            while (tasks.Count > 0)
            {
                await RunTask(tasks[0]);
                tasks.RemoveAt(0);
            }
            await _dialogManager.HideProgressDialog();
            while (workerNotifications.Count > 0)
            {
                await _dialogManager.ShowOkMessageAsync(workerNotifications[0].NotificationTitle,
                                                        workerNotifications[0].NotificationDescription);
                workerNotifications.RemoveAt(0);
            }
            isBusy = false;
        }

        private async Task RunTask(WorkerTask task)
        {
            await _dialogManager.ShowProgressDialog(task.Title, task.Description);
            try
            {
                await Task.Run(() => task.Action());
                if (task.WorkerNotification != null)
                    workerNotifications.Add(task.WorkerNotification);
            }
            catch (Exception ex)
            {
                await _dialogManager.HideProgressDialog();
                await _dialogManager.ShowOkMessageAsync(AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                                                        ex.Message);
                Logger.Log($"Run Worker Error | {ex.Message}");
            }
        }
    }
}