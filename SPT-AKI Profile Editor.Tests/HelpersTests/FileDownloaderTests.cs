using NUnit.Framework;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.IO;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Tests.HelpersTests
{
    internal class FileDownloaderTests
    {
        [Test]
        public void CanInitialize()
        {
            FileDownloaderDialog fileDownloader = MakeSUT();
            Assert.That(fileDownloader, Is.Not.Null);
        }

        [Test]
        public async Task CanDownloadFile()
        {
            if (File.Exists(TestHelpers.fileDownloaderTestSavePath))
                File.Delete(TestHelpers.fileDownloaderTestSavePath);
            FileDownloaderDialog fileDownloader = MakeSUT();
            await fileDownloader.Download(TestHelpers.fileDownloaderTestUrl, TestHelpers.fileDownloaderTestSavePath);
            Assert.That(File.Exists(TestHelpers.fileDownloaderTestSavePath), Is.True);
        }

        [Test]
        public async Task CanCatchException()
        {
            TestsDialogManager dialogManager = new();
            FileDownloaderDialog fileDownloader = MakeSUT(dialogManager);
            await fileDownloader.Download("https://test87894565.com/nonExistigFile.md", TestHelpers.fileDownloaderTestSavePath);
            Assert.That(dialogManager.LastOkMessage, Is.Not.Null);
        }

        private static FileDownloaderDialog MakeSUT(TestsDialogManager dialogManager = null)
                            => new(dialogManager ?? new TestsDialogManager());
    }
}