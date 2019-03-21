using System.Windows.Media;

namespace Organizer.Services.Local
{
    internal interface IImageManager<in T>
    {
        ImageSource GetImage(T source);
    }
}
