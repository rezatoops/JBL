using Core.Services;
using Core.Services.Interfaces;
using Datalayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace dastgire.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        IUserService _userService;
        ICategoryService _categoryService;
        IToolsService _toolsService;

        public CategoryController(IUserService userService, ICategoryService categoryService, IToolsService toolsService)
        {
            _userService = userService;
            _categoryService = categoryService;
            _toolsService = toolsService;
        }
        public IActionResult Index(int? ParentId)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            ViewBag.ParentId = ParentId;
            var categories = _categoryService.GetAllCategory(ParentId);

            return View(categories);
        }

        public IActionResult Publish(int? ParentId)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }

            return View(new Category { ParentId = ParentId });
        }

        [HttpPost]
        public ActionResult Publish(int? ParentId,string Title, string Slug, string thumbUrl)
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
                //if (string.IsNullOrEmpty(thumbUrl))
                //{
                //    ModelState.AddModelError("", "تصویر شاخص انتخاب کنید");
                //    isHaveError = true;
                //}
                if (!isHaveError)
                {
                    Category p = new Category();
                    p.Title = Title;
                    var newSlug = _toolsService.GenerateSlug(Slug);
                    p.Slug = _categoryService.GetUnicSlug(newSlug, -1);

                    if (!string.IsNullOrEmpty(thumbUrl))
                    {
                        p.CategoryImageId = int.Parse(thumbUrl.Replace("image_", string.Empty));

                    }
                    else
                    {
                        p.CategoryImageId = null;
                    }
                    p.ParentId = ParentId;
                    _categoryService.SaveCategory(p);

                    TempData["Message"] = "دسته بندی با موفقیت منتشر شد";


                    return RedirectToAction("Edit", "Category", new { id = p.Id });
                }

                else
                {


                    ViewBag.Thumb = thumbUrl;

                    return View(new Category()
                    {
                        Title = Title,
                        Slug = Slug,
                        ParentId = ParentId,
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
            var category = _categoryService.GetCategory(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();


            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(int CategoryId, string Title,string Slug, string thumbUrl, int Sort, bool IsHide)
        {
            bool isHaveError = false;
            Category p = _categoryService.GetCategory(CategoryId);
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
                //if (string.IsNullOrEmpty(thumbUrl))
                //{
                //    ModelState.AddModelError("", "تصویر شاخص انتخاب کنید");
                //    isHaveError = true;
                //}
                if (!isHaveError)
                {
                    p.Title = Title;
                    var newSlug = _toolsService.GenerateSlug(Slug);
                    p.Slug = _categoryService.GetUnicSlug(newSlug, CategoryId);
                    p.Sort = Sort;
                    p.IsHide = IsHide;
                    if (!string.IsNullOrEmpty(thumbUrl))
                    {
                        p.CategoryImageId = int.Parse(thumbUrl.Replace("image_", string.Empty));

                    }
                    else
                    {
                        p.CategoryImageId = null;
                    }

                    _categoryService.SaveDatabase();

                    TempData["Message"] = "دسته بندی با موفقیت ذخیره شد";


                    return RedirectToAction("Edit", "Category", new { id = p.Id });
                }

                else
                {


                    ViewBag.Thumb = thumbUrl;

                    return View(new Category()
                    {
                        Id = p.Id,
                        Slug = p.Slug,
                        Title = Title,
                        Sort = Sort,
                        IsHide = IsHide,
                    });
                }
            }


            return View();
        }

        public void DeleteCategory(int id)
        {
            _categoryService.DeleteCategory(id);
        }
    }
}
