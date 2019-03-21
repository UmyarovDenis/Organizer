using ContractLibrary.DataTransfer.Dto;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AdminPanel.Views.UICore
{
    internal class TimeActivityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ServiceDescriptionDto serviceDto = value as ServiceDescriptionDto;

            if(serviceDto != null)
            {
                if(serviceDto.IsStarted)
                {
                    return "Время запуска";
                }
                else
                {
                    return "Время закрытия | Время работы";
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
