using MahApps.Metro.Converters;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace SPT_AKI_Profile_Editor.Helpers
{
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

        public static string GetString(object status) => status.ToString();

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

        private static string[] GetStrings()
        {
            List<string> list = new();
            foreach (QuestStatus format in AppData.AppSettings.standartQuestStatuses)
                list.Add(GetString(format));

            return list.ToArray();
        }
    }

    public class IssuesActionValueConverter : IValueConverter
    {
        public static Dictionary<string, string> Strings => GetStrings();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IssuesAction status)
                return GetString(status);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
                return Enum.Parse(typeof(IssuesAction), s);
            return null;
        }

        private static string GetString(IssuesAction action) => action.ToString();

        private static Dictionary<string, string> GetStrings()
        {
            Dictionary<string, string> list = new();
            foreach (IssuesAction action in Enum.GetValues(typeof(IssuesAction)))
                list.Add(GetString(action), AppData.AppLocalization.GetLocalizedString($"issue_action_{GetString(action).ToLower()}"));
            return list;
        }
    }

    public class StashTypeValueConverter : IValueConverter
    {
        public static Dictionary<string, string> Strings => GetStrings();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StashType status)
                return GetString(status);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
                return Enum.Parse(typeof(StashType), s);
            return null;
        }

        private static string GetString(StashType stashType) => stashType.ToString();

        private static Dictionary<string, string> GetStrings()
        {
            Dictionary<string, string> list = new();
            foreach (StashType stashType in Enum.GetValues(typeof(StashType)))
                list.Add(GetString(stashType), AppData.AppLocalization.GetLocalizedString($"stash_type_{GetString(stashType).ToLower()}"));
            return list;
        }
    }

    public class HelperModStatusNameValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HelperModStatus status)
                switch (status)
                {
                    case HelperModStatus.NotInstalled:
                        return AppData.AppLocalization.GetLocalizedString("helper_mod_status_not_installed");

                    case HelperModStatus.Installed:
                        return AppData.AppLocalization.GetLocalizedString("helper_mod_status_installed");

                    case HelperModStatus.UpdateAvailable:
                        return AppData.AppLocalization.GetLocalizedString("helper_mod_status_outdated");
                }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class HelperModStatusNameColorConverter : IValueConverter
    {
        public static SolidColorBrush SuccessColor => new(Color.FromRgb(0, 102, 0));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HelperModStatus status)
                switch (status)
                {
                    case HelperModStatus.NotInstalled:
                        return parameter as SolidColorBrush;

                    case HelperModStatus.Installed:
                        return SuccessColor;

                    case HelperModStatus.UpdateAvailable:
                        return new SolidColorBrush(Color.FromRgb(255, 153, 51));
                }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}