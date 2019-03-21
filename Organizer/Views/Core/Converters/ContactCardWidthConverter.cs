using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace Organizer.Views.Core.Converters
{
    internal sealed class ContactCardWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return new GridLength(0, GridUnitType.Star);
            }
            else
            {
                return new GridLength(0.6, GridUnitType.Star);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
