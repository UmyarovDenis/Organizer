using System;
using System.Globalization;
using System.Windows.Data;

namespace Organizer.Views.Core.Converters
{
    internal sealed class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((DateTime)value).ToLongDateString();

            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
