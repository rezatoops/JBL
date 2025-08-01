using Core.Services;
using Core.Services.Interfaces;
using Datalayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PandS.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class BlogController : Controller
    {
        IUserService _userService;
        IPostService _postService;
        IToolsService _toolsService;


        public BlogController(IUserService userService,IPostService postService, IToolsService toolsService)
        {
            _userService = userService;
            _postService = postService;
            _toolsService = toolsService;
        }
        public IActionResult Index()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            return View(_postService.GetAllPosts());
        }

        public IActionResult Publish()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public IActionResult Publish(string Title, string Slug, string Content, string tags, string[] SelectedCategory, string thumbUrl, string MetaDescription)
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
                if (string.IsNullOrEmpty(tags))
                {
                    ModelState.AddModelError("", "برچسب خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(Content))
                {
                    ModelState.AddModelError("", "متن خالی است");
                    isHaveError = true;
                }
                if (SelectedCategory.Count() == 0)
                {
                    ModelState.AddModelError("", "دسته بندی مشخص نشده است");
                    isHaveError = true;
                }

                if (!isHaveError)
                {
                    Post p = new Post();
                    p.Title = Title;
                    var newSlug = _toolsService.GenerateSlug(Slug);
                    p.Slug = _postService.GetUnicSlug(newSlug, -1);
                    p.MetaDescription = MetaDescription;
                    p.Content = Content;
                    p.PublishDate = DateTime.Now;
                    p.UserId = _userService.isExistUser(User.Identity.Name).Id;
                    if (!string.IsNullOrEmpty(thumbUrl))
                    {
                        p.MediaId = int.Parse(thumbUrl.Replace("image_", string.Empty));
                    }

                    p.Status = "published";

                    _postService.SavePost(p);

                    _postService.AddCategoryToPost(p.Id, SelectedCategory);

                    _postService.AddTagsToPost(tags, p.Id);

                    TempData["Message"] = "مطلب با موفقیت منتشر شد";


                    return RedirectToAction("Edit", "Blog", new { id = p.Id });
                }

                else
                {


                    ViewBag.SelectedCategory = SelectedCategory;

                    ViewBag.Thumb = thumbUrl;
                    ViewBag.Tags = tags;
                    return View(new Post()
                    {
                        Title = Title,
                        Slug = Slug,
                        Content = Content,
                        MetaDescription = MetaDescription,
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
            var post = _postService.GetPost(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();


            return View(post);
        }


        [HttpPost]
        public IActionResult Edit(int PostId, string Title, string Content, string tags, string[] SelectedCategory, string thumbUrl, string MetaDescription)
        {
            bool isHaveError = false;
            Post p = _postService.GetPost(PostId);
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "تیتر خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(tags))
                {
                    ModelState.AddModelError("", "برچسب خالی است");
                    isHaveError = true;
                }

                if (string.IsNullOrEmpty(Content))
                {
                    ModelState.AddModelError("", "متن خالی است");
                    isHaveError = true;
                }
                if (SelectedCategory.Count() == 0)
                {
                    ModelState.AddModelError("", "قالب مشخص نشده است");
                    isHaveError = true;
                }
                if (!isHaveError)
                {
                    p.Title = Title;
                    p.Content = Content;
                    p.MetaDescription = MetaDescription;
                    if (!string.IsNullOrEmpty(thumbUrl))
                    {
                        p.MediaId = int.Parse(thumbUrl.Replace("image_", string.Empty));
                    }

                    _postService.SaveDatabse();

                    _postService.UpdatePostCategories(p.Id, SelectedCategory);

                    _postService.UpdatePostTag(tags, p.Id);

                    TempData["Message"] = "مقاله با موفقیت ذخیره شد";


                    return RedirectToAction("Edit", "Blog", new { id = p.Id });
                }

                else
                {

                    ViewBag.SelectedCategory = SelectedCategory;

                    ViewBag.Thumb = thumbUrl;
                    ViewBag.Tags = tags;
                    return View(new Post()
                    {
                        Id = p.Id,
                        Title = Title,
                        Slug = p.Slug,
                        Content = Content,
                        MetaDescription = MetaDescription,
                    });
                }
            }


            return View();
        }

        public void DeletePost(int id)
        {
            _postService.DeletePost(id);
        }

        public IActionResult BlogCategories()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            var categories = _postService.GetAllCategories();

            return View(categories);
        }

        public IActionResult PublishCategory()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }

            return View(new BlogCategory());
        }

        [HttpPost]
        public ActionResult PublishCategory(string Title, string Slug)
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

                if (!isHaveError)
                {
                    BlogCategory p = new BlogCategory();
                    p.Title = Title;
                    var newSlug = _toolsService.GenerateSlug(Slug);
                    p.Slug = newSlug;

                    _postService.SaveBlogCategory(p);

                    TempData["Message"] = "دسته بندی با موفقیت منتشر شد";


                    return RedirectToAction("EditCategory", "Blog", new { id = p.Id });
                }

                else
                {

                    return View(new BlogCategory()
                    {
                        Title = Title,
                        Slug = Slug,
                    });
                }
            }


            return View();
        }

        public IActionResult EditCategory(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            var category = _postService.GetCategoryById(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();


            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(int CategoryId, string Title, string Slug)
        {
            bool isHaveError = false;
            BlogCategory p = _postService.GetCategoryById(CategoryId);
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
                if (!isHaveError)
                {
                    p.Title = Title;
                    var newSlug = _toolsService.GenerateSlug(Slug);
                    p.Slug = newSlug;

                    _postService.SaveDatabse();

                    TempData["Message"] = "دسته بندی با موفقیت ذخیره شد";


                    return RedirectToAction("EditCategory", "Blog", new { id = p.Id });
                }

                else
                {

                    return View(new BlogCategory()
                    {
                        Id = p.Id,
                        Slug = p.Slug,
                        Title = Title,
                    });
                }
            }


            return View();
        }

        public void DeleteCategory(int id)
        {
            _postService.DeleteCategory(id);
        }


    }
}
