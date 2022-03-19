using System;

namespace SPT_AKI_Profile_Editor.Classes
{
    public class WorkerTask
    {
        public Action Action { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public WorkerNotification WorkerNotification { get; set; }
    }
}