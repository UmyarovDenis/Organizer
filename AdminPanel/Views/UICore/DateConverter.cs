using ContractLibrary.DataTransfer.Dto;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AdminPanel.Views.UICore
{
    internal class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ServiceDescriptionDto service = value as ServiceDescriptionDto;

            if(service != null)
            {
                if (service.IsStarted)
                {
                    return String.Format("{0}", service.StartDate.ToString());
                }
                else
                {
                    return String.Format("{0} | {1}", service.StopDate.ToString(),
                        service.StopDate.ToLocalTime() - service.StartDate.ToLocalTime());
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
