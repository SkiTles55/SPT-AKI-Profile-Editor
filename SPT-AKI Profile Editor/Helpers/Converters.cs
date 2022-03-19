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

    public class QuestStatusValueConverter : IValueConverter
    {
        public static string[] Strings => GetStrings();

        public static string GetString(QuestStatus status) => status.ToString();

        public static string[] GetStrings()
        {
            List<string> list = new();
            foreach (QuestStatus format in Enum.GetValues(typeof(QuestStatus)))
                list.Add(GetString(format));

            return list.ToArray();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QuestStatus status)
                return GetString(status);

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