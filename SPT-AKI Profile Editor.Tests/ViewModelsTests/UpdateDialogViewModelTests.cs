using NUnit.Framework;
using ReleaseChecker.GitHub;
using System;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class UpdateDialogViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            UpdateDialogViewModel viewModel = new(null, null, GetTestRelease());
            Assert.That(viewModel, Is.Not.Null, "UpdateDialogViewModel is null");
            Assert.That(viewModel.DownloadRelease, Is.Not.Null, "DownloadRelease is null");
            Assert.That(viewModel.OpenReleaseUrl, Is.Not.Null, "OpenReleaseUrl is null");
            Assert.That(viewModel.Release, Is.Not.Null, "Release is null");
            Assert.That(viewModel.ReleaseFile, Is.Not.Null, "ReleaseFile is null");
            Assert.That(string.IsNullOrEmpty(viewModel.FormatedDate), Is.False, "FormatedDate is null or empty");
        }

        private static GitHubRelease GetTestRelease()
        {
            List<GithubReleaseFile> files = new()
            {
                new GithubReleaseFile()
                {
                    Name = "TestFile",
                    Url = "TestUrl"
                }
            };

            return new GitHubRelease()
            {
                Files = files.ToArray(),
                PublishDate = DateTime.Now
            };
        }
    }
}