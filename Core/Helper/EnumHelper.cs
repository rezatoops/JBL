using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class EnumHelper
    {
        public static List<SelectListItem> ToSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = Convert.ToInt32(e).ToString(),
                    Text = e.GetType()
                             .GetMember(e.ToString())[0]
                             .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
                }).ToList();
        }


    }
}
