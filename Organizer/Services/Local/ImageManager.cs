using Organizer.Enums;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Linq;

namespace Organizer.Services.Local
{
    internal sealed class ImageManager : IImageManager<ImageType>
    {
        public ImageSource GetImage(ImageType source)
        {
            var sourceType = source.GetType().GetMember(source.ToString()).FirstOrDefault();

            if (Attribute.IsDefined(sourceType, typeof(DescriptionAttribute)))
            {
                var attribute = sourceType.GetCustomAttribute<DescriptionAttribute>(false);

                return new BitmapImage(new Uri(attribute.Description));
            }

            return null;
        }
    }
}
