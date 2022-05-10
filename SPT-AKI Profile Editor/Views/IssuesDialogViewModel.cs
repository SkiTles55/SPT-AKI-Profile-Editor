using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class IssuesDialogViewModel : BindableViewModel
    {
        public IssuesDialogViewModel(RelayCommand saveCommand)
        {
            SaveCommand = saveCommand;
        }

        public static RelayCommand CancelCommand => new(async obj => await CloseDailog());
        public static IssuesService IssuesService => AppData.IssuesService;
        public RelayCommand SaveCommand { get; }
        public RelayCommand FixCommand => new(async obj => await ExecuteFixCommand(obj));
        public RelayCommand IgnoreCommand => new(obj => ExecuteSaveCommand(IssuesAction.AlwaysIgnore));
        public RelayCommand FixAllCommand => new(obj => ExecuteSaveCommand(IssuesAction.AlwaysFix));
        public bool RemeberAction { get; set; } = false;

        private static async Task CloseDailog()
        {
            BaseMetroDialog dialog = await App.DialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(MainWindowViewModel.Instance);
            await App.DialogCoordinator.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
        }

        private async Task ExecuteFixCommand(object obj)
        {
            if (obj is Action command)
            {
                command.Invoke();
                IssuesService.GetIssues();
            }
            if (!IssuesService.HasIssues)
            {
                await CloseDailog();
                SaveCommand.Execute(null);
            }
        }

        private async void ExecuteSaveCommand(IssuesAction issuesAction)
        {
            await CloseDailog();
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