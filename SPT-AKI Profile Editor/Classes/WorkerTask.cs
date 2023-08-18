using System;

namespace SPT_AKI_Profile_Editor.Classes
{
    public class WorkerTask
    {
        public WorkerTask()
        { }

        public WorkerTask(Action action, string title, string description)
        {
            Action = action;
            Title = title;
            Description = description;
        }

        public Action Action { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkerNotification WorkerNotification { get; set; }
    }
}