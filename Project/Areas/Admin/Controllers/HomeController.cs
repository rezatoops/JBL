using Core.Services;
using Core.Services.Interfaces;
using Core.ViewModel;
using Datalayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace dastgire.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class HomeController : Controller
    {

        IUserService _userService;
        IProductService _productService;
        IWebHostEnvironment _webHostEnvironment;

        public HomeController(IUserService userService, IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            return View();
        }

        public IActionResult Brands()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            ViewBag.Message = TempData["Message"];
            ViewBag.Style = TempData["Style"];
            return View(_productService.GetAllBrands());
        }

        public IActionResult GetBrands(int pageNumber, int countPerPage, int allpage)
        {

            BrandViewModel brandTable = new BrandViewModel
            {
                Brands = _productService.GetSpecificBrands(pageNumber, countPerPage),
                CurrentPage = pageNumber,
                AllPage = allpage,
                CountPerPage = countPerPage
            };

            return PartialView("_brandTable", brandTable);
        }

        public ActionResult LoadBrandEditForm(int id)
        {
            Brand model = new Brand();


            if (id != 0)
            {
                model = _productService.GetBrand(id);
            }

            return PartialView("_BrandEditForm", model);
        }

        public IActionResult RefreshBrand()
        {
            var brands = _productService.GetAllBrands();
            int allPage = (brands.Count() % 1000 == 0) ? (brands.Count() / 1000) : (brands.Count() / 1000) + 1;

            BrandViewModel brandTable = new BrandViewModel
            {
                Brands = brands,
                CurrentPage = 1,
                CountPerPage = 1000,
                AllPage = allPage

            };
            return PartialView("_brandrTable", brandTable);
        }

        public async Task<ActionResult> AddNewOrEditBrand(int BrandId, string BrandName, string BrandLink, IFormFile BrandIcon)
        {
            if (string.IsNullOrEmpty(BrandName))
            {
                TempData["Message"] = "نام برند نمی تواند خالی باشد";
                TempData["Style"] = "alert-danger";
                return RedirectToAction("Brands", _productService.GetAllBrands());
            }
            if (BrandId == 0 && BrandIcon == null)
            {
                TempData["Message"] = "یک لوگو برای برند باید در نظر بگیرید";
                TempData["Style"] = "alert-danger";
                return RedirectToAction("Brands", _productService.GetAllBrands());
            }

            var brand = _productService.AddNewOrEditBrand(BrandId, BrandName, BrandLink);

            if (brand == null)
            {

                TempData["Message"] = "نام برند تکراری است";
                TempData["Style"] = "alert-danger";

                return RedirectToAction("Brands", _productService.GetAllBrands());
            }
            else
            {
                if (BrandIcon != null)
                {
                    if (BrandIcon.Length > 0)
                    {
                        var extention = Path.GetExtension(BrandIcon.FileName);
                        var FileName = brand.Id + extention;
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/brands", FileName);
                        string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets/brands");

                        Directory.CreateDirectory(TargetLocation);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await BrandIcon.CopyToAsync(stream);
                        }
                    }
                }

                if (BrandId != 0)
                {
                    TempData["Message"] = "برند " + brand.Name + " با موفقیت ویرایش شد";

                }
                else
                {

                    TempData["Message"] = "مشتری " + brand.Name + " با موفقیت اضافه شد";

                }
                TempData["Style"] = "alert-success";
                //return PartialView("_CatEditForm", cfv);
                return RedirectToAction("Brands", _productService.GetAllBrands());
            }
        }

        public void DeleteBrand(int id)
        {
            _productService.DeleteBrand(id);
        }
    }
}
