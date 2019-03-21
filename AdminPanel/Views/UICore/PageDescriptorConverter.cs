using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Reflection;
using AdminPanel.Views.Attributes;

namespace AdminPanel.Views.UICore
{
    internal class PageDescriptorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type pageType = value.GetType();

            if(Attribute.IsDefined(pageType, typeof(PageDescriptorAttribute)))
            {
                var attribute = pageType.GetCustomAttribute<PageDescriptorAttribute>(false);

                if(parameter.ToString() == "Image")
                {
                    return attribute.ImageSource;
                }
                else if(parameter.ToString() == "Label")
                {
                    return attribute.Label;
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
