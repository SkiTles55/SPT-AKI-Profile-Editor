using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsWorker : IWorker
    {
        private readonly IDialogManager _dialogManager;

        public bool AddTaskCalled { get; set; } = false;

        public TestsWorker(IDialogManager dialogManager = null)
        {
            _dialogManager = dialogManager;
        }

        public void AddTask(WorkerTask task)
        {
            AddTaskCalled = true;
            task.Action.Invoke();
            if (task.WorkerNotification != null && _dialogManager != null)
                _dialogManager.ShowOkMessageAsync(task.WorkerNotification.NotificationTitle,
                                                  task.WorkerNotification.NotificationDescription);
        }
    }
}