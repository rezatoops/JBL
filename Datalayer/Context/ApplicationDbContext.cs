using Azure;
using Datalayer.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalayer.Context
{
    public class ApplicationDbContext : DbContext, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Variable> Variables { get; set; }
        public DbSet<Datalayer.Entities.Attribute> Attributes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ShopDesign> ShopDesigns { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<UserCoupon> UserCoupons { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<MajorShopping> MajorShoppings { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShopDesign>(e =>
            {
                e.HasOne(p => p.Banner1)
                .WithMany(g => g.MediaForBanner1)
                .HasForeignKey(p => p.Banner1ImgId);

                e.HasOne(p => p.Banner2)
                .WithMany(g => g.MediaForBanner2)
                .HasForeignKey(p => p.Banner2ImgId);

                e.HasOne(p => p.Banner3)
                .WithMany(g => g.MediaForBanner3)
                .HasForeignKey(p => p.Banner3ImgId);

                e.HasOne(p => p.Banner4)
                .WithMany(g => g.MediaForBanner4)
                .HasForeignKey(p => p.Banner4ImgId);

                e.HasOne(p => p.BannerWonder)
                .WithMany(g => g.MediaForWonder)
                .HasForeignKey(p => p.BannerWonderImgId);
            });

            modelBuilder.Entity<Slider>(e =>
            {
                e.HasOne(p => p.Media)
                .WithMany(g => g.Sliders)
                .HasForeignKey(p => p.MediaId);

                e.HasOne(p => p.MobileMedia)
                .WithMany(g => g.MobileSliders)
                .HasForeignKey(p => p.MobileMediaId);

            });
        }

    }
}
