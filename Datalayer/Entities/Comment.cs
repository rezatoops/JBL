using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public int? ProductId { get; set; }
        public DateTime CreationDate { get; set; }

        public bool IsRead { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(4000)]
        public string? Message { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Comment ParentComment { get; set; }

        [ForeignKey("ProductId")]

        public Product Product { get; set; }
    }
}
