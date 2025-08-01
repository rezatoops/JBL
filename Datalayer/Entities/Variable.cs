using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Variable
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [MaxLength(2000)]
        public string? Title { get; set; }

        public int? Price { get; set; }
        public int? SalePrice { get; set; }

        public int? NumberInStock { get; set; }

        public string? Description { get; set; }

        public Product Product { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
    }
}
