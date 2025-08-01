using Azure;
using Core.Services.Interfaces;
using Datalayer.Context;
using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CategoryService : ICategoryService
    {
        private ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }


        public Category AddNewOrEditCat(int CatId, string CatTitle, string ParentCategorySelected)
        {

            if (CatId == 0)
            {
                var q = (from a in _context.Categories where a.Title == CatTitle select a);

                if (q.Count() > 0)
                {
                    return null;
                }

                Category cat;
                cat = new Category()
                {
                    Title = CatTitle,
                    ParentId = string.IsNullOrEmpty(ParentCategorySelected) ? null : int.Parse(ParentCategorySelected),
                };

                _context.Categories.Add(cat);
                _context.SaveChanges();
                return cat;
            }
            else
            {

                Category cat = (from b in _context.Categories where b.Id == CatId select b).FirstOrDefault();
                cat.Title = CatTitle;
                cat.ParentId = string.IsNullOrEmpty(ParentCategorySelected) ? null : int.Parse(ParentCategorySelected);

                _context.SaveChanges();
                return cat;
            }


        }

        public List<Category> GetAllCategory()
        {
            return _context.Categories.ToList();

        }
        public List<Category> GetAllCategory(int? parentId)
        {
            return _context.Categories.Where(c=>c.ParentId == parentId).OrderBy(o=>o.Sort).ThenByDescending(o=>o.Id).Include(c=>c.Media).ToList();

        }

        public List<Product> GetAllProdcutInCategory(int CatId, int number = 0, bool IsOnlyAvailable = false)
        {
            List<Product> FilteredProducts = new List<Product>();

            if (number == 0)
            {
                FilteredProducts = _context.ProductCategories
                    .Include(p => p.Product).ThenInclude(p => p.Photo)
                    .Include(p => p.Product.Variables)
                    .Where(c => c.CategoryId == CatId && c.Product.IsDeleted == false && c.Product.Status == "Publish" && !c.Product.IsHidden).Select(p => p.Product).OrderByDescending(o => o.Id).ToList();
            }
            else
            {
                FilteredProducts = _context.ProductCategories
                    .Include(p => p.Product).ThenInclude(p => p.Photo)
                    .Include(p=>p.Product.Variables)
                    .Where(c => c.CategoryId == CatId && c.Product.IsDeleted == false && c.Product.Status == "Publish" && !c.Product.IsHidden).Select(p => p.Product).OrderByDescending(o => o.Id).Take(number).ToList();
            }

            if (IsOnlyAvailable)
                return FilteredProducts.Where(p => p.IsInStock).ToList();

            return FilteredProducts;
        }

        public List<Category> GetCategoriesByParentId(bool IsHideInclude, int? parentId = null)
        {
            if (IsHideInclude)
            {
                return _context.Categories.Include(c => c.Media).Where(c => c.ParentId == parentId && c.IsHide == false).OrderBy(o => o.Sort).ThenByDescending(o => o.Id).ToList();
            }
            else
            {
                return _context.Categories.Include(c => c.Media).Where(c => c.ParentId == parentId).OrderBy(o => o.Sort).ThenByDescending(o => o.Id).ToList();
            }
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Include(c => c.ParentCategory).Include(c=>c.Media).Where(c => c.Id == id).FirstOrDefault();
        }

        public Category GetCategory(string slug)
        {
            return _context.Categories.Include(c=>c.ParentCategory).Where(c => c.Slug == slug).FirstOrDefault();
        }


        public List<Product> GetSpecificProductsInCategory(int CategoryId, int pageId, int countPerPage)
        {
            var AllProductCategoryTable = _context.ProductCategories;
            var AvaliableProducts = AllProductCategoryTable.Where(c => c.CategoryId == CategoryId && c.Product.IsDeleted == false && c.Product.Status == "Publish" && !c.Product.IsHidden && c.Product.Variables.Sum(v=>v.NumberInStock) > 0).Include(p => p.Product).ThenInclude(p => p.Photo).Select(p => p.Product).OrderByDescending(p => p.Id)/*.Skip(countPerPage * (pageId - 1)).Take(countPerPage)*/.ToList();

            var NotAvalbaleProducts = AllProductCategoryTable.Where(c => c.CategoryId == CategoryId && c.Product.IsDeleted == false && c.Product.Status == "Publish" && !c.Product.IsHidden && c.Product.Variables.Sum(v => v.NumberInStock) == 0).Include(p => p.Product).ThenInclude(p => p.Photo).Select(p => p.Product).OrderByDescending(p => p.Id)/*.Skip(countPerPage * (pageId - 1)).Take(countPerPage)*/.ToList();

            var result = new List<Product>();
            result.AddRange(AvaliableProducts);
            result.AddRange(NotAvalbaleProducts);

            return result.Skip(countPerPage * (pageId - 1)).Take(countPerPage).ToList();
        }

        public void DeleteCategory(int Id)
        {
            var category = GetCategory(Id);

            var subCats = GetCategoriesByParentId(false,category.Id);
            foreach (var sub in subCats)
            {
                sub.ParentId = null;
            }

            var shopRow = _context.ShopDesigns.Where(s => s.CategoryId == Id);
            foreach (var row in shopRow)
            {
                if(row.Type != Datalayer.Enums.ShopRawEnum.ProductSliderByCategory)
                {
                    row.CategoryId = null;
                }
                else
                {
                    _context.ShopDesigns.Remove(row);
                }
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public string GetUnicSlug(string slug, int id)
        {
            bool isSlugExist = false;
            while (!isSlugExist)
            {
                var q = from a in _context.Categories where a.Slug == slug select a;
                if (q.Count() > 0)
                {
                    if (q.Count() == 1 && q.FirstOrDefault().Id == id)
                    {
                        isSlugExist = true;
                    }
                    else
                    {
                        slug = slug + "_1";
                        isSlugExist = false;
                    }
                }
                else
                {
                    isSlugExist = true;
                }
            }

            return slug;
        }

        public void SaveCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void SaveDatabase()
        {
            _context.SaveChanges();
        }
    }
}
