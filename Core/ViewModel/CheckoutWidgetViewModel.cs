using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class CheckoutWidgetViewModel
    {
        public int totalNumber { get; set; }
        public int totalPrice { get; set; }

        public int totalRegularPrice { get; set; }
        public int shippingPrice { get; set; }
        public string? CoupnCode { get; set; }
        public int coupnPrice { get; set; }
    }
}
