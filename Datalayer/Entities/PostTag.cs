using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class PostTag
    {
        [Key]
        public int Id { get; set; }

        public int PostId { get; set; }
        public int BlogTagId { get; set; }

        public Post Post { get; set; }

        public BlogTag BlogTag { get; set; }
    }
}
