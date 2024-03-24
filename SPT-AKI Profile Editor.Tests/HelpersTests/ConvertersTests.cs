using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.HelpersTests
{
    internal class ConvertersTests
    {
        [Test]
        public void FileSizeConverterCanIgnoreNullValues()
        {
            FileSizeConverter fileSizeConverter = new();
            var result = fileSizeConverter.Convert(null, null, null, null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void FileSizeConverterCanIgnoreNonLongValues()
        {
            FileSizeConverter fileSizeConverter = new();
            var result = fileSizeConverter.Convert("test", null, null, null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void FileSizeConverterCanConvert()
        {
            FileSizeConverter fileSizeConverter = new();
            long size = 1024;
            var result = fileSizeConverter.Convert(size, null, null, null);
            Assert.That(result is string, Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToString(), Is.EqualTo("1,0 KB"));
        }

        [Test]
        public void FileSizeConverterCanConvertNegativeValue()
        {
            FileSizeConverter fileSizeConverter = new();
            long size = -1024;
            var result = fileSizeConverter.Convert(size, null, null, null);
            Assert.That(result is string, Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ToString(), Is.EqualTo("-1,0 KB"));
        }

        [Test]
        public void FileSizeConverterCanThrow()
        {
            FileSizeConverter fileSizeConverter = new();
            Assert.Throws<NotSupportedException>(() => fileSizeConverter.ConvertBack("1,0 KB", null, null, null));
        }

        [Test]
        public void QuestStatusValueConverterCanConvertQuestStatus()
        {
            QuestStatusValueConverter questStatusValueConverter = new();
            foreach (var status in Enum.GetValues(typeof(QuestStatus)))
            {
                var result = questStatusValueConverter.Convert(status, null, null, null);
                Assert.That(result is string, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.ToString(), Is.EqualTo(status.ToString()));
            }
        }

        [Test]
        public void QuestStatusValueConverterCanConvertQuestType()
        {
            QuestStatusValueConverter questStatusValueConverter = new();
            foreach (var type in Enum.GetValues(typeof(QuestType)))
            {
                var result = questStatusValueConverter.Convert(type, null, null, null);
                Assert.That(result is IEnumerable<string>, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That((IEnumerable<string>)result, Is.EqualTo(((QuestType)type).GetAvailableStatuses().Select(x => x.ToString())));
            }
        }

        [Test]
        public void QuestStatusValueConverterCanIgnoreWrongValue()
        {
            QuestStatusValueConverter questStatusValueConverter = new();
            var result = questStatusValueConverter.Convert("test", null, null, null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void QuestStatusValueConverterCanConvertBackQuestStatus()
        {
            QuestStatusValueConverter questStatusValueConverter = new();
            foreach (var status in Enum.GetValues(typeof(QuestStatus)))
            {
                var result = questStatusValueConverter.ConvertBack(status.ToString(), null, null, null);
                Assert.That(result is QuestStatus, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(status));
            }
        }

        [Test]
        public void QuestStatusValueConverterConvertBackCanIgnoreWrongValue()
        {
            QuestStatusValueConverter questStatusValueConverter = new();
            var result = questStatusValueConverter.ConvertBack(QuestStatus.AvailableForFinish, null, null, null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void IssuesActionValueConverterStringsNotEmpty()
        {
            Assert.That(IssuesActionValueConverter.Strings, Is.Not.Empty);
            var actionsCount = Enum.GetValues(typeof(IssuesAction)).Length;
            Assert.That(IssuesActionValueConverter.Strings.Count, Is.EqualTo(actionsCount));
        }

        [Test]
        public void IssuesActionValueConverterCanConvert()
        {
            IssuesActionValueConverter issuesActionValueConverter = new();
            foreach (var action in Enum.GetValues(typeof(IssuesAction)))
            {
                var result = issuesActionValueConverter.Convert(action, null, null, null);
                Assert.That(result is string, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.ToString(), Is.EqualTo(action.ToString()));
            }
        }

        [Test]
        public void IssuesActionValueConverterCanIgnoreWrongValue()
        {
            IssuesActionValueConverter issuesActionValueConverter = new();
            var result = issuesActionValueConverter.Convert("test", null, null, null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void IssuesActionValueConverterCanConvertBackQuestStatus()
        {
            IssuesActionValueConverter issuesActionValueConverter = new();
            foreach (var action in Enum.GetValues(typeof(IssuesAction)))
            {
                var result = issuesActionValueConverter.ConvertBack(action.ToString(), null, null, null);
                Assert.That(result is IssuesAction, Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(action));
            }
        }

        [Test]
        public void IssuesActionValueConverterConvertBackCanIgnoreWrongValue()
        {
            IssuesActionValueConverter issuesActionValueConverter = new();
            var result = issuesActionValueConverter.ConvertBack(QuestStatus.AvailableForFinish, null, null, null);
            Assert.That(result, Is.Null);
        }
    }
}