using Core.Services;
using Core.Services.Interfaces;
using Core.ViewModel;
using Datalayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arsic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PageController : Controller
    {
        IUserService _userService;
        IPageService _pageService;
        IToolsService _toolsService;

        public PageController(IPageService pageService, IUserService userService, IToolsService toolsService)
        {
            _userService = userService;
            _pageService = pageService;
            _toolsService = toolsService;
        }

        public IActionResult Index()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            return View(_pageService.GetAllPages());
        }

        public IActionResult GetPages(int pageNumber, int countPerPage, int allpage)
        {

            PageViewModel pageTable = new PageViewModel
            {
                Pages = _pageService.GetSpecificPages(pageNumber, countPerPage),
                CurrentPage = pageNumber,
                AllPage = allpage,
                CountPerPage = countPerPage,
            };

            return PartialView("_pageTable", pageTable);

        }

        public void DeletePage(int id)
        {
            _pageService.DeletePage(id);
        }

        public IActionResult Publish()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }

            return View(new Page());
        }

        [HttpPost]
        public ActionResult Publish(string Title, string Slug, string Content)
        {
            bool isHaveError = false;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "تیتر خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(Slug))
                {
                    ModelState.AddModelError("", "پیوند یکتا خالی است");
                    isHaveError = true;
                }

                if (string.IsNullOrEmpty(Content))
                {
                    ModelState.AddModelError("", "متن خالی است");
                    isHaveError = true;
                }
                if (!isHaveError)
                {
                    Page p = new Page();
                    p.Title = Title;
                    var newSlug = _toolsService.GenerateSlug(Slug);
                    p.Slug = _pageService.GetUnicSlug(newSlug, -1);
                    p.Content = Content;
                    p.CreationDate = DateTime.Now;
                    p.UserId = 1;

                    _pageService.SavePage(p);


                    TempData["Message"] = "برگه با موفقیت منتشر شد";


                    return RedirectToAction("Edit", "Page", new { id = p.Id });
                }

                else
                {

                    return View(new Page()
                    {
                        Title = Title,
                        Slug = Slug,
                        Content = Content,
                    });
                }
            }

            return View();
        }

        public IActionResult Edit(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            var page = _pageService.GetPage(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            return View(page);
        }

        [HttpPost]
        public ActionResult Edit(int PageId, string Title, string Content)
        {
            bool isHaveError = false;
            Page p = _pageService.GetPage(PageId);
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "تیتر خالی است");
                    isHaveError = true;
                }

                if (string.IsNullOrEmpty(Content))
                {
                    ModelState.AddModelError("", "متن خالی است");
                    isHaveError = true;
                }
                if (!isHaveError)
                {
                    p.Title = Title;
                    p.Content = Content;

                    _pageService.SaveDatabse();


                    TempData["Message"] = "برگه با موفقیت ذخیره شد";


                    return RedirectToAction("Edit", "Page", new { id = p.Id });
                }

                else
                {

                    return View(new Page()
                    {
                        Id = p.Id,
                        Slug = p.Slug,
                        Title = Title,
                        Content = Content,
                    });
                }
            }


            return View();
        }

        public IActionResult MajorShoppings()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View(_pageService.GetAllMajors());
        }

        public void DeleteMajor(int id)
        {
            _pageService.DeleteMajor(id);
        }

        public void DoUnreadAllMajors()
        {
            _pageService.DoUnreadAllMajor();
        }

        public IActionResult GetMajors(int pageNumber, int countPerPage, int allpage)
        {

            MajorViewModel majorTable = new MajorViewModel
            {
                MajorShoppings = _pageService.GetSpecificMajors(pageNumber, countPerPage),
                CurrentPage = pageNumber,
                AllPage = allpage,
                CountPerPage = countPerPage,
            };

            return PartialView("_majorTable", majorTable);

        }
    }
}
