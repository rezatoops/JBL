using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface ISettingService
    {
        public List<ShopDesign> GetAllShopDesigns();

        public List<ShopDesign> GetSpecificShopDesigns(int page, int countPerPage);

        public ShopDesign GetShopDesignById(int id);

        public void DeleteShopDesign(int id);

        public void SaveShopDesign(ShopDesign shopDesign);

        public void SaveDatabase();

        public string? GetSettingValue(string key);

        public void SetSettingValue(string key, string value);

        public List<Setting> GetAllSettings();
    }
}
