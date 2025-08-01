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
    public class PageService : IPageService
    {
            
        private ApplicationDbContext _context;

        public PageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Page GetPage(string slug)
        {
            return _context.Pages.Include(p=>p.User).FirstOrDefault(x => x.Slug == slug);
        }

        public Page GetPage(int id)
        {
            return _context.Pages.FirstOrDefault(x => x.Id == id);
        }

        public List<Page> GetAllPages()
        {
            return _context.Pages.OrderByDescending(p => p.Id).ToList();
        }

        public List<Page> GetSpecificPages(int page, int countPerPage)
        {
            return _context.Pages.OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();

        }

        public void DeletePage(int id)
        {
            var page = GetPage(id);
            _context.Pages.Remove(page);

            _context.SaveChanges();
        }

        public string GetUnicSlug(string slug, int id)
        {
            bool isSlugExist = false;
            while (!isSlugExist)
            {
                var q = from a in _context.Pages where a.Slug == slug select a;
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

        public void SaveDatabse()
        {
            _context.SaveChanges();
        }

        public void SavePage(Page page)
        {
            _context.Pages.Add(page);
            _context.SaveChanges();
        }

        public void InsertMajorShopping(string FirstName, string LastName, string Number, string Email, string message) {
            var major = new MajorShopping();

            major.FirstName = FirstName;
            major.LastName = LastName;
            major.Number = Number;
            major.Email = Email;
            major.Message = message;
            major.CreationDate = DateTime.Now;

            _context.MajorShoppings.Add(major);
            _context.SaveChanges();
        }

        public int GetUnreadMajorNumber()
        {
            return _context.MajorShoppings.Where(c => c.IsRead == false).Count();
        }

        public List<MajorShopping> GetAllMajors()
        {
            return _context.MajorShoppings.OrderByDescending(s => s.CreationDate).ToList();
        }

        public List<MajorShopping> GetSpecificMajors(int page, int countPerPage)
        {
            return _context.MajorShoppings.OrderByDescending(u => u.CreationDate).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();
        }

        public MajorShopping GetMajor(int id)
        {
            return _context.MajorShoppings.FirstOrDefault(m => m.Id == id);
        }
        public void DeleteMajor(int id)
        {
            var major = GetMajor(id);
            _context.MajorShoppings.Remove(major);

            _context.SaveChanges();
        }


        public void DoUnreadAllMajor()
        {
            var majors = _context.MajorShoppings.Where(c => c.IsRead == false);
            foreach (var major in majors)
            {
                major.IsRead = true;
            }

            _context.SaveChanges();
        }
    }
}
