using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Utils
{
    internal static class EnumExt
    {
        public static TEnum To<TEnum>(this Enum @enum) where TEnum : struct
        {
            foreach(var value in Enum.GetValues(typeof(TEnum)))
            {
                if ((int)value == (int)Enum.Parse(@enum.GetType(), @enum.ToString()))
                    return (TEnum)Enum.Parse(typeof(TEnum), @enum.ToString());
            }

            return default(TEnum);
        }
        public static TEnum To<TEnum>(this int? enumValue) where TEnum : struct
        {
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                if ((int)value == enumValue)
                    return (TEnum)Enum.Parse(typeof(TEnum), enumValue.ToString());
            }

            return default(TEnum);
        }
    }
}
