using Organizer.Utils;
using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Data;
using Organizer.Enums;

namespace Organizer.Views.Core.Converters
{
    internal sealed class FontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ProjectStatus status = ((int?)value).To<ProjectStatus>();

            switch (status)
            {
                case ProjectStatus.InWork:
                    return Brushes.Red;
                case ProjectStatus.OnCheck:
                    return Brushes.Orange;
                case ProjectStatus.Completed:
                    return Brushes.Green;
                default:
                    return Brushes.Black;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
