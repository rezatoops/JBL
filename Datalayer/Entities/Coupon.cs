using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Code { get; set; }

        public int Percent { get; set; }

        public int? MaxValue { get; set; }

        public int? MinTotalOrder { get; set; }

        public bool OnlyFirstOrder { get; set; }

        public bool AnyUserOneTime { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool isDeleted { get; set; }

        public List<Order> Orders { get; set; }

        public List<UserCoupon> UserCoupons { get; set; }

    }
}
