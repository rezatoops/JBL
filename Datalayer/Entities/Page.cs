using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Page
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1000)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Slug { get; set; }

        public string? Content { get; set; }

        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
