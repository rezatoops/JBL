using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Slug { get; set; }

        [MaxLength(300)]
        public string Title { get; set; }

        public int Sort { get; set; }

        public bool IsHide { get; set; }

        public int? CategoryImageId { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Category ParentCategory { get; set; }

        //public List<ProductCat> ProductCat { get; set; }

        [ForeignKey("CategoryImageId")]
        public Media Media { get; set; }

        public List<ShopDesign> ShopDesigns { get; set; }

    }
}
