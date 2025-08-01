using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class BlogCategory
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(300)]
        public string? Title { get; set; }

        [MaxLength(300)]
        public string? Slug { get; set; }

        public List<PostCategory> PostCategory { get; set; }
    }
}
