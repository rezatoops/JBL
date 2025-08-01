using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Enums
{
    public enum ShopRawEnum
    {
        [Display(Name = "جدیدترین محصولات")]
        ProductSliderByNewst,
        [Display(Name = "نمایش محصولات یک دسته بندی")]
        ProductSliderByCategory,
        [Display(Name = "نمایش محصولات تخفیف شگفت انگیز")]
        ProductSliderByWonder,
        [Display(Name = "نمایش تخفیف های شگفت انگیز با تایمر")]
        Wonderfull,
        [Display(Name = "پر فروش ترین محصولات")]
        SortingProduct,
        [Display(Name = "تبلیغات")]
        Ads
    }
}
