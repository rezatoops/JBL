using Core.Services.Interfaces;
using Datalayer.Context;
using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class SettingService : ISettingService
    {
        private ApplicationDbContext _context;
        public SettingService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ShopDesign> GetAllShopDesigns()
        {
            return _context.ShopDesigns
                .Include(s => s.Category)
                .Include(s => s.Banner1)
                .Include(s => s.Banner2)
                .Include(s => s.Banner3)
                .Include(s => s.Banner4)
                .Include(s => s.BannerWonder)
                .OrderBy(s => s.Sort).ToList();
        }

        public List<ShopDesign> GetSpecificShopDesigns(int page, int countPerPage)
        {
            return _context.ShopDesigns.Include(s => s.Category).OrderBy(u => u.Sort).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();
        }

        public ShopDesign GetShopDesignById(int id)
        {
            return _context.ShopDesigns
                .Include(s => s.Category)
                .Include(s => s.Banner1)
                .Include(s => s.Banner2)
                .Include(s => s.Banner3)
                .Include(s => s.Banner4)
                .Include(s => s.BannerWonder)
                .FirstOrDefault(x => x.Id == id);
        }

        public void DeleteShopDesign(int id)
        {
            var shopDesign = GetShopDesignById(id);
            _context.ShopDesigns.Remove(shopDesign);
            _context.SaveChanges();
        }

        public void SaveShopDesign(ShopDesign shopDesign)
        {
            _context.ShopDesigns.Add(shopDesign);
            _context.SaveChanges();
        }

        public void SaveDatabase()
        {
            _context.SaveChanges();
        }

        public string? GetSettingValue(string key)
        {
            return _context.Settings.FirstOrDefault(s => s.Key == key).Value;
        }

        public void SetSettingValue(string key, string value)
        {
            _context.Settings.FirstOrDefault(s => s.Key == key).Value = value;
        }

        public List<Setting> GetAllSettings()
        {
            return _context.Settings.ToList();
        }
    }
}
