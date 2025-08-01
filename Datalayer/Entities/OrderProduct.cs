using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }

        public int? OrderId { get; set; }
        public int? ProductId { get; set; }

        public int? VariableId { get; set; }
        [MaxLength(100)]
        public string? VariableTitle { get; set; }
        public int Price { get; set; }
        public int number { get; set; } = 1;

        public Order Order { get; set; }
        public Product Product { get; set; }

        public Variable? Variable { get; set; }

    }
}
