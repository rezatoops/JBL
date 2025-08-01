using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface ICategoryService
    {
        public Category GetCategory(int id);

        public Category GetCategory(string slug);

        public List<Category> GetAllCategory();
        public List<Category> GetAllCategory(int? parentId);

        public List<Category> GetCategoriesByParentId(bool IsHideInclude, int? parentId = null);
        public Category AddNewOrEditCat(int CatId, string CatTitle, string ParentCategorySelected);

        public List<Product> GetAllProdcutInCategory(int CatId, int number = -1, bool IsOnlyAvailable = false);

        public List<Product> GetSpecificProductsInCategory(int CategoryId, int pageId, int countPerPage);

        public void DeleteCategory(int Id);

        public string GetUnicSlug(string slug, int id);

        public void SaveCategory(Category category);

        public void SaveDatabase();

    }
}
