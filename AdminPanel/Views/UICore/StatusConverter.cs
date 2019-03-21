using AdminPanel.Enums;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Reflection;
using System.Linq;
using System.ComponentModel;

namespace AdminPanel.Views.UICore
{
    internal class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                ServerStatus serverStatus = (ServerStatus)Enum.Parse(typeof(ServerStatus), value.ToString());

                if (parameter == null)
                {
                    Type statusType = serverStatus.GetType();
                    MemberInfo info = statusType.GetMember(Enum.GetName(statusType, serverStatus)).FirstOrDefault();

                    if (Attribute.IsDefined(info, typeof(DescriptionAttribute)))
                    {
                        return info.GetCustomAttribute<DescriptionAttribute>().Description;
                    }

                    return null;
                }
                else
                {
                    switch (serverStatus)
                    {
                        case ServerStatus.Idle:
                            return Brushes.DarkCyan;
                        case ServerStatus.Active:
                            return Brushes.LightGreen;
                        case ServerStatus.Stoped:
                            return Brushes.Red;
                        default:
                            return Brushes.White;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
