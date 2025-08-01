using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())[0]
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? value.ToString();
        }
    }
}
