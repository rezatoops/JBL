using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class PostCategory
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public int BlogCategoryId { get; set; }

        public Post Post { get; set; }

        public BlogCategory BlogCategory { get; set; }


    }
}
