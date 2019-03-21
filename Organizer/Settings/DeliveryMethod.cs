using Organizer.Views.Core.Converters;
using System.ComponentModel;

namespace Organizer.Settings
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    internal enum DeliveryMethod
    {
        [Description("Отправление по сети")]
        Network = 0,
        [Description("Доставка внешним приложением")]
        SpecifiedPickupDirectory = 1,
        [Description("Используется IIS для доставки")]
        PickupDirectoryFromIis = 2
    }
}
