using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ClosableDialogViewModel : BindableViewModel
    {
        private readonly object context;

        public ClosableDialogViewModel(object context) => this.context = context;

        public RelayCommand CancelCommand => new(async obj => await CloseDialog());

        public async Task CloseDialog()
        {
            // Skipping in nUnit tests
            if (System.Windows.Application.Current == null)
                return;
            BaseMetroDialog dialog = await App.DialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(context);
            await App.DialogCoordinator.HideMetroDialogAsync(context, dialog);
        }
    }
}