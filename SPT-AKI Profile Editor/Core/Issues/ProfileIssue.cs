using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public abstract class ProfileIssue
    {
        protected ProfileIssue(string targetId)
        {
            TargetId = targetId;
        }

        public string TargetId { get; }
        public abstract Action FixAction { get; }
        public abstract string Description { get; }
    }
}