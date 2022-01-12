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
}
