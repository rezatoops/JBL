using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class Media
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(2000)]
        public string Url { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Caption { get; set; }

        public string? Type { get; set; }

        public List<Category> Categories { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Slider> MobileSliders { get; set; }

        public List<ShopDesign> MediaForBanner1 { get; set; }
        public List<ShopDesign> MediaForBanner2 { get; set; }
        public List<ShopDesign> MediaForBanner3 { get; set; }
        public List<ShopDesign> MediaForBanner4 { get; set; }
        public List<ShopDesign> MediaForWonder { get; set; }
        public List<Post> Posts { get; set; }
    }
}
