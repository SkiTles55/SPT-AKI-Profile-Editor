using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
    internal class FileDownloader
    {
        private static readonly CancellationTokenSource cancelTokenSource = new();
        private static readonly CancellationToken cancellationToken = cancelTokenSource.Token;
        private static ProgressDialogController progressDialog;
        private static IProgress<float> progressIndicator;

        private static MetroDialogSettings DialogSettings => new()
        {
            NegativeButtonText = "cancel",
            DialogResultOnCancel = MessageDialogResult.Canceled,
        };

        public static async Task Download(string url, string filePath)
        {
            progressDialog = await App.DialogCoordinator.ShowProgressAsync(
                MainWindowViewModel.Instance,
                "Download File",
                "File Url",
                true,
                DialogSettings);
            progressIndicator = new Progress<float>(ReportProgress);
            progressDialog.Canceled += DownloadCanceled;
            if (await DownloadFromUrl(url, filePath))
                await CloseProgressWithMessage("Download File", "File downloaded");
        }

        private static void DownloadCanceled(object sender, EventArgs e) => cancelTokenSource.Cancel();

        private static void ReportProgress(float value)
        {
            if (progressDialog != null)
            {
                progressDialog.SetProgress(value);
                progressDialog.SetMessage($"Progress: {value * 100}");
            }
        }

        private static async Task<bool> DownloadFromUrl(string url, string filePath)
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(5);
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

        private static async Task CloseProgressWithMessage(string title, string message, bool show = true)
        {
            if (progressDialog.IsOpen)
                await progressDialog.CloseAsync();
            if (show)
                await Dialogs.ShowOkMessageAsync(MainWindowViewModel.Instance, title, message);
        }

        private static async Task DownloadAsync(HttpClient client, string requestUri, Stream destination)
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

        private static async Task CopyToAsync(Stream source, Stream destination, int bufferSize, IProgress<long> progress = null)
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