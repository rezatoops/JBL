using Core.Services.Interfaces;
using Datalayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Core.ViewModel.ZarebinViewModel;

namespace Arsic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZarebinController : ControllerBase
    {
        IProductService _productService;

        public ZarebinController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Route("/Zarehbin/Products")]
        [Route("/Zarehbin/Products/{page}")]
        public Zarehbin GetProducts(int page = 1) 
        {
            int item_per_page = 100;
            List<Product> AllProducts = _productService.GetAllProduct();
            List<Product> resultProducts = AllProducts.Skip(item_per_page * (page - 1)).Take(item_per_page).ToList();

            List<ZarebinProduct> zProducts = new List<ZarebinProduct>();

            foreach (var pr in resultProducts)
            {
                var zProduct = new ZarebinProduct();
                zProduct.title = pr.Title;
                zProduct.subtitle = pr.EnglishTitle;
                zProduct.id = pr.Id;
                zProduct.categories = _productService.GetProductCategoriesTitle(pr.Id);
                zProduct.image_link = "https://nishcoffee.ir/" + pr.Photo.Url;
                zProduct.image_links = _productService.GetGalleryPhotos(pr.Gallery).Select(p => "https://nishcoffee.ir/" + p.Url).ToList();

                var mainVariable = _productService.GetMainVariable(pr.Id);
                if(mainVariable != null)
                {
                    if (mainVariable.SalePrice != null)
                    {
                        zProduct.current_price = mainVariable.SalePrice.Value.ToString();
                        zProduct.old_price = mainVariable.Price.Value.ToString();
                    }
                    else
                    {
                        zProduct.current_price = mainVariable.Price.Value.ToString();
                    }
                    zProduct.availability = (mainVariable.NumberInStock > 0)? "instock" : "outstock";
                }
                else
                {
                    zProduct.current_price = "0";
                    zProduct.availability = "outstock";
                }

                zProduct.page_url = "https://nishcoffee.ir/Product/" + pr.Slug;

                zProducts.Add(zProduct);
            }

            var zarehbin = new Zarehbin();
            zarehbin.products = zProducts;
            zarehbin.count = AllProducts.Count().ToString();
            zarehbin.total_pages_count = (AllProducts.Count() % item_per_page == 0) ? (AllProducts.Count() / item_per_page).ToString() : ((AllProducts.Count() / item_per_page) + 1).ToString();

            return zarehbin;
        }
    }
}
