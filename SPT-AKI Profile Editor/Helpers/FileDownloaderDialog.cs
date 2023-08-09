using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class FileDownloaderDialog
    {
        private readonly CancellationTokenSource cancelTokenSource = new();
        private readonly CancellationToken cancellationToken;
        private readonly IDialogManager dialogManager;
        private IProgress<float> progressIndicator;

        public FileDownloaderDialog(IDialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
            cancellationToken = cancelTokenSource.Token;
        }

        private static MetroDialogSettings DialogSettings => new()
        {
            NegativeButtonText = AppData.AppLocalization.GetLocalizedString("button_cancel"),
            DialogResultOnCancel = MessageDialogResult.Canceled,
        };

        public async Task Download(string url, string filePath)
        {
            ReportProgress(0);
            progressIndicator = new Progress<float>(ReportProgress);
            dialogManager.ProgressDialogCanceled += DownloadCanceled;

            FileDownloader fileDownloader = new(progressIndicator, cancellationToken);

            try
            {
                await fileDownloader.DownloadFromUrl(url, filePath);
                await CloseProgressWithMessage(AppData.AppLocalization.GetLocalizedString("download_dialog_title"),
                                               AppData.AppLocalization.GetLocalizedString("download_dialog_success"));
            }
            catch (Exception ex)
            {
                await CloseProgressWithMessage(AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                                               ex.Message,
                                               ex is not OperationCanceledException);
                Logger.Log($"FileDownloaderDialog | {ex.Message}");
            }
        }

        private static string GetProgressString(float value)
            => string.Format("{0} {1:P2}.",
                             AppData.AppLocalization.GetLocalizedString("download_dialog_downloaded"),
                             value);

        private void DownloadCanceled(object sender, EventArgs e) => cancelTokenSource.Cancel();

        private async void ReportProgress(float value)
            => await dialogManager.ShowProgressDialog(AppData.AppLocalization.GetLocalizedString("download_dialog_title"),
                                                      GetProgressString(value),
                                                      false,
                                                      value,
                                                      true,
                                                      DialogSettings);

        private async Task CloseProgressWithMessage(string title, string message, bool showMessage = true)
        {
            await dialogManager.HideProgressDialog();
            if (showMessage)
                await dialogManager.ShowOkMessageAsync(title, message);
        }
    }
}