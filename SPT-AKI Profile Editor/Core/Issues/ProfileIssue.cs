using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public abstract class ProfileIssue(string targetId)
    {
        public string TargetId { get; } = targetId;
        public abstract Action FixAction { get; }
        public abstract string Description { get; }
    }
}