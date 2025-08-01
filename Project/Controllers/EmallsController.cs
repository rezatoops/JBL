using Core.Services.Interfaces;
using Datalayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Core.ViewModel.EmallsViewModel;

namespace Arsic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmallsController : ControllerBase
    {
        IProductService _productService;

        public EmallsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Route("GetProducts")]
        public Emalls GetProducts(int page,int item_per_page) 
        {
            List<Product> AllProducts = _productService.GetAllProduct();
            List<Product> resultProducts = AllProducts.Skip(item_per_page * (page - 1)).Take(item_per_page).ToList();

            List<EmProducts> emProducts = new List<EmProducts>();

            foreach (var pr in resultProducts)
            {
                var emProduct = new EmProducts();
                emProduct.title = pr.Title;
                emProduct.id = pr.Id.ToString();

                var mainVariable = _productService.GetMainVariable(pr.Id);
                if(mainVariable != null)
                {
                    if (mainVariable.SalePrice != null)
                    {
                        emProduct.price = mainVariable.SalePrice.Value;
                        emProduct.old_price = mainVariable.Price.Value;
                    }
                    else
                    {
                        emProduct.price = mainVariable.Price.Value;
                    }
                    emProduct.is_available = mainVariable.NumberInStock > 0;
                }
                else
                {
                    emProduct.price = 0;
                    emProduct.is_available = false;
                }

                emProduct.url = "https://nishcoffee.ir/Product/" + pr.Slug;

                emProducts.Add(emProduct);
            }

            var emalls = new Emalls();
            emalls.products = emProducts;
            emalls.pages_count = (AllProducts.Count() % item_per_page == 0) ? (AllProducts.Count() / item_per_page) : (AllProducts.Count() / item_per_page) + 1;

            return emalls;
        }
    }
}
