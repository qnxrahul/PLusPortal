using System.ComponentModel;
using System.Reflection;
using System;
using System.Linq;

namespace Steer73.RockIT.Enums
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            return field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() is DescriptionAttribute attribute ? attribute.Description : value.ToString();
        }
    }
}
