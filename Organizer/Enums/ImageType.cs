using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Enums
{
    internal enum ImageType
    {
        [Description("pack://application:,,,/OrganizerStyles;component/Resources/contact.png")]
        ContactImage,

        [Description("pack://application:,,,/OrganizerStyles;component/Resources/office-buildings.png")]
        OrganizationImage,

        [Description("pack://application:,,,/OrganizerStyles;component/Resources/tasks.png")]
        TaskImage
    }
}
