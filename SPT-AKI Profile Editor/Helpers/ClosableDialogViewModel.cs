using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ClosableDialogViewModel : BindableViewModel
    {
        public static RelayCommand CancelCommand => new(async obj => await CloseDialog());

        public static async Task CloseDialog()
        {
            // Skipping in nUnit tests
            if (System.Windows.Application.Current == null)
                return;
            BaseMetroDialog dialog = await App.DialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(MainWindowViewModel.Instance);
            await App.DialogCoordinator.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
        }
    }
}