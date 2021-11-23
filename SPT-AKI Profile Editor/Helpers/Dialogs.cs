using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class Dialogs
    {
        public static async Task<MessageDialogResult> YesNoDialog(object context, string title, string caption) =>
            await App.dialogCoordinator.ShowMessageAsync(context,
                AppData.AppLocalization.GetLocalizedString(title),
                AppData.AppLocalization.GetLocalizedString(caption),
                MessageDialogStyle.AffirmativeAndNegative,
                DialogSettings);

        private static MetroDialogSettings DialogSettings => new()
        {
            DefaultButtonFocus = MessageDialogResult.Affirmative,
            AffirmativeButtonText = AppData.AppLocalization.GetLocalizedString("button_yes"),
            NegativeButtonText = AppData.AppLocalization.GetLocalizedString("button_no"),
            AnimateShow = true,
            AnimateHide = true
        };
    }
}