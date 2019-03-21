using ContractLibrary.DataTransfer.Dto;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AdminPanel.Views.UICore
{
    internal class ConnectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UserDto user = value as UserDto;

            if(user != null)
            {
                if(parameter == null)
                {
                    if (user.IsConnected) return "онлайн";
                    else return "офлайн";
                }
                else
                {
                    if (user.IsConnected) return Brushes.Green;
                    else return Brushes.Red;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
