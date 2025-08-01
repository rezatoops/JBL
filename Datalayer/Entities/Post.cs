using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MediaId { get; set; }

        [MaxLength(1000)]
        public string? Title { get; set; }
        [MaxLength(1000)]
        public string? Slug { get; set; }
        
        public string? Content { get; set; }

        public DateTime PublishDate { get; set; }
        [MaxLength(20)]
        public string? Status { get; set; }

        [MaxLength(1000)]
        public string? MetaDescription { get; set; }

        public Media Media { get; set; }
        public User User { get; set; }

        public List<PostTag> PostTag { get; set; }
        public List<PostCategory> PostCategory { get; set; }


    }
}
