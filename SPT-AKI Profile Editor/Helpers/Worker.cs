using MahApps.Metro.Controls.Dialogs;
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
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IDialogManager _dialogManager;
        private readonly BindableViewModel _viewModel;
        private readonly List<WorkerTask> tasks;
        private readonly List<WorkerNotification> workerNotifications;
        private ProgressDialogController progressDialog;
        private bool isBusy = false;

        public Worker(IDialogCoordinator dialogCoordinator, BindableViewModel viewModel, IDialogManager dialogManager)
        {
            tasks = new List<WorkerTask>();
            workerNotifications = new List<WorkerNotification>();
            _dialogCoordinator = dialogCoordinator;
            _viewModel = viewModel;
            _dialogManager = dialogManager;
        }

        public async void AddTask(WorkerTask task)
        {
            tasks.Add(task);
            if (!isBusy)
            {
                isBusy = true;
                await CreateProgressDialog(task.Title, task.Description);
                progressDialog?.SetIndeterminate();
                RunWorkerAsync();
            }
        }

        private async Task CreateProgressDialog(string title, string description)
        {
            try
            {
                progressDialog = await _dialogCoordinator?.ShowProgressAsync(_viewModel, title, description);
            }
            catch (Exception ex)
            {
                Logger.Log($"Worker CreateProgressDialog Error | {ex.Message}");
            }
        }

        private async void RunWorkerAsync()
        {
            while (tasks.Count > 0)
            {
                progressDialog?.SetTitle(tasks[0].Title);
                progressDialog?.SetMessage(tasks[0].Description);
                try
                {
                    await Task.Run(() => tasks[0].Action());
                    if (tasks[0].WorkerNotification != null)
                        workerNotifications.Add(tasks[0].WorkerNotification);
                }
                catch (Exception ex)
                {
                    if (progressDialog?.IsOpen ?? false)
                        await progressDialog?.CloseAsync();
                    await _dialogManager.ShowOkMessageAsync(AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                                                            ex.Message);
                    Logger.Log($"Run Worker Error | {ex.Message}");
                }
                tasks.RemoveAt(0);
            }
            if (progressDialog?.IsOpen ?? false)
                await progressDialog?.CloseAsync();
            while (workerNotifications.Count > 0)
            {
                await _dialogManager.ShowOkMessageAsync(workerNotifications[0].NotificationTitle,
                                                        workerNotifications[0].NotificationDescription);
                workerNotifications.RemoveAt(0);
            }
            isBusy = false;
        }
    }
}