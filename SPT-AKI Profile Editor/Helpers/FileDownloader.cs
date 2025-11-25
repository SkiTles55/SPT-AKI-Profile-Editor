using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Helpers
{
#nullable enable

    public class FileDownloader(IProgress<float>? progressIndicator = null,
        CancellationToken? cancellationToken = null)
    {
        private readonly CancellationToken cancellationToken = cancellationToken ?? CancellationToken.None;

        public async Task DownloadFromUrl(string url, string filePath)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await DownloadAsync(client, url, file);
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
            progressIndicator?.Report(1);
        }

        private async Task CopyToAsync(Stream source, Stream destination, int bufferSize, IProgress<long>? progress)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (!source.CanRead)
                throw new ArgumentException("Has to be readable", nameof(source));
            ArgumentNullException.ThrowIfNull(destination);
            if (!destination.CanWrite)
                throw new ArgumentException("Has to be writable", nameof(destination));
            ArgumentOutOfRangeException.ThrowIfNegative(bufferSize);

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