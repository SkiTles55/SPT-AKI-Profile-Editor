using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class IssuesDialogViewModel : ClosableDialogViewModel
    {
        public IssuesDialogViewModel(RelayCommand saveCommand)
        {
            SaveCommand = saveCommand;
        }

        public static IssuesService IssuesService => AppData.IssuesService;
        public RelayCommand SaveCommand { get; }
        public RelayCommand FixCommand => new(async obj => await ExecuteFixCommand(obj));
        public RelayCommand IgnoreCommand => new(obj => ExecuteSaveCommand(IssuesAction.AlwaysIgnore));
        public RelayCommand FixAllCommand => new(obj => ExecuteSaveCommand(IssuesAction.AlwaysFix));
        public bool RemeberAction { get; set; } = false;

        private async Task ExecuteFixCommand(object obj)
        {
            if (obj is Action command)
            {
                command.Invoke();
                IssuesService.GetIssues();
            }
            if (!IssuesService.HasIssues)
            {
                await CloseDialog();
                SaveCommand.Execute(null);
            }
        }

        private async void ExecuteSaveCommand(IssuesAction issuesAction)
        {
            await CloseDialog();
            if (RemeberAction)
                AppData.AppSettings.IssuesAction = issuesAction;
            switch (issuesAction)
            {
                case IssuesAction.AlwaysFix:
                    IssuesService.FixAllIssues();
                    SaveCommand.Execute(null);
                    break;

                case IssuesAction.AlwaysIgnore:
                    SaveCommand.Execute(null);
                    break;
            }
        }
    }
}