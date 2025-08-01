using Datalayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Entities
{
    public class ShopDesign
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public int? CategoryId { get; set; }

        public ShopRawEnum Type { get; set; }

        [MaxLength(300)]
        public string? Name { get; set; }
        [MaxLength(300)]
        public string? Title { get; set; }

        public bool IsOnlyAvailable { get; set; }

        [MaxLength(2000)]
        public string? Banner1Link { get; set; }

        [MaxLength(2000)]
        public string? Banner2Link { get; set; }

        [MaxLength(2000)]
        public string? Banner3Link { get; set; }
        [MaxLength(2000)]
        public string? Banner4Link { get; set; }
        [MaxLength(2000)]
        public string? BannerWonderLink { get; set; }

        public int? Banner1ImgId { get; set; }
        public int? Banner2ImgId { get; set; }
        public int? Banner3ImgId { get; set; }
        public int? Banner4ImgId { get; set; }
        public int? BannerWonderImgId { get; set; }

        public int NumberOfProduct { get; set; } = 10;

        public int Sort { get; set; }

        public Category? Category { get; set; }

        public Media? Banner1 { get; set; }
        public Media? Banner2 { get; set; }
        public Media? Banner3 { get; set; }
        public Media? Banner4 { get; set; }
        public Media? BannerWonder { get; set; }
    }
}
