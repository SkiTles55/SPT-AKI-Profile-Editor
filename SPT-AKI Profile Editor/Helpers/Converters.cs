using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Views.ExtendedControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class HandbookCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is not List<HandbookCategory> categories) return null;
            return categories.Select(x => new HandbookCategoryViewModel(x)).Where(x => x.IsNotHidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class FileSizeConverter : IValueConverter
    {
        private readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is not long size) return null;
            return SizeConverter(size);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();

        private string SizeConverter(long value)
        {
            if (value < 0) { return "-" + SizeConverter(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
        }
    }

    public class QuestStatusValueConverter : IValueConverter
    {
        public static string[] Strings => GetStrings();

        public static string GetString(QuestStatus status) => status.ToString();

        public static string[] GetStrings()
        {
            List<string> list = new();
            foreach (QuestStatus format in AppData.AppSettings.standartQuestStatuses)
                list.Add(GetString(format));

            return list.ToArray();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QuestStatus status)
                return GetString(status);
            if (value is QuestType type)
                return type.GetAvailableStatuses().Select(x => GetString(x));

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
                return Enum.Parse(typeof(QuestStatus), s);
            return null;
        }
    }
}