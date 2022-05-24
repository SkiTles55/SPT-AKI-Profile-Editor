using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class Worker
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly BindableViewModel _viewModel;
        private readonly List<WorkerTask> tasks;
        private readonly List<WorkerNotification> workerNotifications;
        private ProgressDialogController progressDialog;
        private bool isBusy = false;

        public Worker(IDialogCoordinator dialogCoordinator, BindableViewModel viewModel)
        {
            tasks = new List<WorkerTask>();
            workerNotifications = new List<WorkerNotification>();
            _dialogCoordinator = dialogCoordinator;
            _viewModel = viewModel;
        }

        public async void AddAction(WorkerTask task)
        {
            tasks.Add(task);
            if (!isBusy)
            {
                isBusy = true;
                progressDialog = await _dialogCoordinator.ShowProgressAsync(_viewModel,
                    task.Title,
                    task.Description);
                progressDialog.SetIndeterminate();
                RunWorkerAsync();
            }
        }

        private async void RunWorkerAsync()
        {
            while (tasks.Count > 0)
            {
                progressDialog.SetTitle(tasks[0].Title);
                progressDialog.SetMessage(tasks[0].Description);
                try
                {
                    await Task.Run(() => tasks[0].Action());
                    if (tasks[0].WorkerNotification != null)
                        workerNotifications.Add(tasks[0].WorkerNotification);
                }
                catch (Exception ex)
                {
                    if (progressDialog.IsOpen)
                        await progressDialog.CloseAsync();
                    await Dialogs.ShowOkMessageAsync(_viewModel,
                        AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                        ex.Message);
                    Logger.Log($"LoadDataWorker | {ex.Message}");
                }
                tasks.RemoveAt(0);
            }
            if (progressDialog.IsOpen)
                await progressDialog.CloseAsync();
            while (workerNotifications.Count > 0)
            {
                await Dialogs.ShowOkMessageAsync(_viewModel, workerNotifications[0].NotificationTitle,
                    workerNotifications[0].NotificationDescription);
                workerNotifications.RemoveAt(0);
            }
            isBusy = false;
        }
    }
}