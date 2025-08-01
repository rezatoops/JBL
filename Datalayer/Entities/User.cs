using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }
        [MaxLength(100)]
        public string? Password { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool IsActive { get; set; }
        public string? ActiveCode { get; set; }

        public int Role { get; set; }


        public List<Product> Products { get; set; }
        public List<Post> Posts { get; set; }

        public List<UserCoupon> UserCoupons { get; set; }
        public List<Page> Pages { get; set; }

    }
}
