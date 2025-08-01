using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        

        [MaxLength(100)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public City? ParentCity { get; set; }

        public List<Order> Orders { get; set; }
    }
}
