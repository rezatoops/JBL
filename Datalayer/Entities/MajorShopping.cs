using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class MajorShopping
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsRead { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(20)]
        public string? Number { get; set; }

        [MaxLength(4000)]
        public string? Message { get; set; }

    }
}
