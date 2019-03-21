using Organizer.Utils;
using System;
using System.Reflection;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using Organizer.Enums;

namespace Organizer.Views.Core.Converters
{
    class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ProjectStatus status = ((int?)value).To<ProjectStatus>();
            Type statusType = status.GetType();
            MemberInfo info = statusType.GetMember(Enum.GetName(statusType, status)).FirstOrDefault();

            if (Attribute.IsDefined(info, typeof(DescriptionAttribute)))
            {
                return info.GetCustomAttribute<DescriptionAttribute>().Description;
            }

            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
