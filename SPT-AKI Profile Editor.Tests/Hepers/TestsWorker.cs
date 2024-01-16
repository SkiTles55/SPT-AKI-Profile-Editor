using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsWorker : IWorker
    {
        public bool AddTaskCalled { get; set; } = false;

        public void AddTask(WorkerTask task)
        {
            AddTaskCalled = true;
            task.Action.Invoke();
        }
    }
}