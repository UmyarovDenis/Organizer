using Organizer.Views.Core.Converters;
using System.ComponentModel;

namespace Organizer.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    internal enum ProjectStatus
    {
        [Description("В работе")]
        InWork = 1,
        [Description("На проверке")]
        OnCheck,
        [Description("Выполнено")]
        Completed
    }
}
