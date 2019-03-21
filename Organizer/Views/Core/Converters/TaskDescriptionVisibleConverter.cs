using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Organizer.Views.Core.Converters
{
    internal sealed class TaskDescriptionVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
