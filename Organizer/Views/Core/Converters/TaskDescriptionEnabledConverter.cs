using System;
using System.Globalization;
using System.Windows.Data;

namespace Organizer.Views.Core.Converters
{
    internal sealed class TaskDescriptionEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
