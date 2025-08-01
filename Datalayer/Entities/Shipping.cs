using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Shipping
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public int ShipPrice { get; set; }

        public bool IsTehran { get; set; }

        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }

        public List<Order> Orders { get; set; }
    }
}
