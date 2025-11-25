using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Issues;
using System;
using System.Collections.ObjectModel;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsIssuesService : IIssuesService
    {
        private ObservableCollection<ProfileIssue> profileIssues = [];

        public ObservableCollection<ProfileIssue> ProfileIssues { get => profileIssues; set => profileIssues = value; }

        public bool HasIssues => ProfileIssues != null && ProfileIssues.Count > 0;

        public void FixAllIssues()
        {
            while (HasIssues)
            {
                profileIssues[0].FixAction();
                GetIssues();
            }
        }

        public void GetIssues()
        {
        }

        public void UpdateIssues()
        {
        }
    }

    internal class TestsProfileIssue(Action fixAction, string targetId) : ProfileIssue(targetId)
    {
        public override Action FixAction { get; } = fixAction;

        public override string Description => "TestsProfileIssue";
    }
}