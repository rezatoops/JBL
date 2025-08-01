using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Link { get; set; }

        public int? MediaId { get; set; }
        public int? MobileMediaId { get; set; }

        [MaxLength(20)]
        public string Type { get; set; }

        public Media Media { get; set; }
        public Media MobileMedia { get; set; }
    }
}
