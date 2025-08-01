using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class UserCoupon
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CouponId { get; set; }

        public User User { get; set; }

        public Coupon Coupon { get; set; }
    }
}
