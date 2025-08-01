using Core.Services.Interfaces;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Arsic.Controllers
{
    public class SiteMapController : Controller
    {
        IProductService _productService;

        public SiteMapController(IProductService productService)
        {
            _productService = productService;
        }

        //[Route("/sitemap2.xml")]
        public ActionResult Index()
        {
            // list of items to add
            var products = _productService.GetAllProduct();

            var siteMapBuilder = new SiteMapBuilder();

            // add the blog posts to the sitemap
            foreach (var product in products)
            {
                siteMapBuilder.AddUrl("https://nishcoffee.ir/Product/" + product.Slug, modified: product.CreateTime, changeFrequency: SitemapFrequency.Weekly, priority: 1.0);
            }

            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            return Content(xml, "text/xml", Encoding.UTF8);
        }
        [Route("/sitemap.xml")]
        public async Task<ContentResult> CreateSitemap()
        {
            List<SitemapNode> sitemapNodes = new List<SitemapNode>();

            var products = _productService.GetAllProduct();

            foreach (var product in products)
            {
                sitemapNodes.Add(new SitemapNode { Url = "https://nishcoffee.ir/Product/" + product.Slug, Priority = 1.0, Frequency = SitemapFrequency.Weekly, LastModified = product.CreateTime });
            }

            SiteMapGenerator.CreateSitemapXML(sitemapNodes, "");

            var sitemapPath = Path.Combine("", "sitemap.xml");
            string output = await System.IO.File.ReadAllTextAsync(sitemapPath);

            return new ContentResult
            {
                Content = output,
                ContentType = "application/xml",
                StatusCode = 200,
            };
        }
    }
}
