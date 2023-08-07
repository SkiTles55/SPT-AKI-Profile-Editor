using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.IO;
using System.Net.Http;
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

            if (await DownloadFromUrl(url, filePath))
                await CloseProgressWithMessage(AppData.AppLocalization.GetLocalizedString("download_dialog_title"),
                                               AppData.AppLocalization.GetLocalizedString("download_dialog_success"));
        }

        private static string GetProgressString(float value) => string.Format("{0} {1:P2}.", AppData.AppLocalization.GetLocalizedString("download_dialog_downloaded"), value);

        private void DownloadCanceled(object sender, EventArgs e) => cancelTokenSource.Cancel();

        private async void ReportProgress(float value)
        {
            await dialogManager.ShowProgressDialog(AppData.AppLocalization.GetLocalizedString("download_dialog_title"),
                                                   GetProgressString(value),
                                                   false,
                                                   value,
                                                   true,
                                                   DialogSettings);
        }

        private async Task<bool> DownloadFromUrl(string url, string filePath)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(30);
                using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await DownloadAsync(client, url, file);
                return true;
            }
            catch (Exception ex)
            {
                await CloseProgressWithMessage(
                    AppData.AppLocalization.GetLocalizedString("invalid_server_location_caption"),
                    ex.Message,
                    ex is not OperationCanceledException);
                Logger.Log($"FileDownloader | {ex.Message}");
                return false;
            }
        }

        private async Task CloseProgressWithMessage(string title, string message, bool showMessage = true)
        {
            await dialogManager.HideProgressDialog();
            if (showMessage)
                await dialogManager.ShowOkMessageAsync(title, message);
        }

        private async Task DownloadAsync(HttpClient client, string requestUri, Stream destination)
        {
            using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            var contentLength = response.Content.Headers.ContentLength;

            using var download = await response.Content.ReadAsStreamAsync(cancellationToken);
            if (progressIndicator == null || !contentLength.HasValue)
            {
                await download.CopyToAsync(destination, cancellationToken);
                return;
            }
            var relativeProgress = new Progress<long>(totalBytes => progressIndicator.Report((float)totalBytes / contentLength.Value));
            await CopyToAsync(download, destination, 81920, relativeProgress);
            progressIndicator.Report(1);
        }

        private async Task CopyToAsync(Stream source, Stream destination, int bufferSize, IProgress<long> progress = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}