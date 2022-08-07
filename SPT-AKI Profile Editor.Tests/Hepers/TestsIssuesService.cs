using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Issues;
using System;
using System.Collections.ObjectModel;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestsIssuesService : IIssuesService
    {
        private ObservableCollection<ProfileIssue> profileIssues = new();

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

    internal class TestsProfileIssue : ProfileIssue
    {
        public TestsProfileIssue(Action fixAction, string targetId) : base(targetId)
        {
            FixAction = fixAction;
        }

        public override Action FixAction { get; }

        public override string Description => "TestsProfileIssue";
    }
}