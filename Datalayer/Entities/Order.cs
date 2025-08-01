using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }


        public int CityId { get; set; }
        [MaxLength]
        public string? Address { get; set; }
        [MaxLength(10)]
        public string? PostalCode { get; set; }
        [MaxLength(11)]
        public string? PhoneNumber { get; set; }

        [MaxLength(20)]
        public string? HomePhone { get; set; }

        [MaxLength(20)]
        public string? MelliCode { get; set; }

        public User User { get; set; }
        public City? City { get; set; }

        public bool IsFinal { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? PaymentDate { get; set; }
        [MaxLength(100)]
        public string? PaymentCode { get; set; }
        [MaxLength(30)]
        public string Status { get; set; }
        [MaxLength(100)]
        public string? TrackingCode { get; set; }

        public bool IsReservedStack { get; set; }
        public int ShippingPrice { get; set; }
        public int? ShippingId { get; set; }

        public int OffPercent { get; set; }
        [MaxLength(4000)]
        public string? Description { get; set; }
        public int? CouponId { get; set; }
        [MaxLength(100)]
        public string? CouponName { get; set; }
        public int CouponOffValue { get; set; }

        public int SudeShoma { get; set; }
        public Shipping Shipping { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }

        public Coupon Coupon { get; set; }
    }
}
