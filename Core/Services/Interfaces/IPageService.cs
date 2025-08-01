using Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IPageService
    {
        public Page GetPage(string slug);
        public Page GetPage(int id);

        public List<Page> GetAllPages();
        public List<Page> GetSpecificPages(int page, int countPerPage);

        public void DeletePage(int id);

        public string GetUnicSlug(string slug, int id);

        public void SavePage(Page page);

        public void SaveDatabse();

        public void InsertMajorShopping(string FirstName, string LastName, string Number, string Email, string message);

        public int GetUnreadMajorNumber();

        public List<MajorShopping> GetAllMajors();
        public List<MajorShopping> GetSpecificMajors(int page, int countPerPage);
        public MajorShopping GetMajor(int id);
        public void DeleteMajor(int id);
        public void DoUnreadAllMajor();

    }
}
