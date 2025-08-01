using Core.Services.Interfaces;
using dastgire.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dastgire.Controllers
{
    public class HomeController : Controller
    {
        IUserService _userService;
        IPageService _pageService;
        IProductService _productService;

        public HomeController(IUserService userService, IProductService productService,IPageService pageService)
        {
            _userService = userService;
            _productService = productService;
            _pageService = pageService;
        }

        public IActionResult Index()
        {
            //ViewBag.HiddenMainHeader = true;
            return View("shop");
        }

        [Route("/Shop")]
        public IActionResult Shop()
        {
            return RedirectToActionPermanent("Index");
        }

        [Route("/AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("/ContactUs")]
        public IActionResult ContactUs()
        {
            return View();
        }



        [Route("Profile")]
        public IActionResult Profile()
        {
            ViewBag.MenuSelected = 2;
            var user = _userService.isExistUser(User.Identity.Name);

            return View(user);
        }

        [HttpPost]
        [Route("Profile")]
        public IActionResult Profile(string firstName, string lastName, string Password)
        {
            var user = _userService.isExistUser(User.Identity.Name);
            _userService.ChangeProfile(user, firstName, lastName, Password);
            ViewBag.Message = "تغییرات با موفقیت اعمال شد";
            ViewBag.MenuSelected = 2;
            return View(user);
        }

        [Route("MyOrder")]
        public IActionResult MyOrder()
        {
            ViewBag.MenuSelected = 1;
            var user = _userService.isExistUser(User.Identity.Name);

            return View(user);
        }

        [Route("MyOrderDetails/{id}")]
        public IActionResult MyOrderDetails(int id)
        {
            return View(_productService.GetOrderById(id));
        }

        [Route("MajorShopping")]
        public IActionResult MajorShopping()
        {
            return View();
        }

        [HttpPost]
        [Route("MajorShopping")]
        public IActionResult MajorShopping(string FirstName, string LastName, string Number, string Email,string message)
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Number) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(message)) { 
                _pageService.InsertMajorShopping(FirstName, LastName, Number, Email, message);

                ViewBag.success = true;

                return View();
            }
            else
            {
                ViewBag.success = false;

                ViewBag.FirstName = FirstName;
                ViewBag.LastName = LastName;
                ViewBag.Number = Number;
                ViewBag.Email = Email;
                ViewBag.message = message;


                return View();
            }
        }
    }
}
