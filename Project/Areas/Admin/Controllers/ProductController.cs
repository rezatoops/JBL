using Core.Services;
using Core.Services.Interfaces;
using Core.ViewModel;
using Datalayer.Entities;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using System.Globalization;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Intrinsics.X86;

namespace dastgire.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class ProductController : Controller
    {
        IUserService _userService;
        IProductService _productService;
        ICategoryService _categoryService;
        IToolsService _toolsService;
        public ProductController(IUserService userService, IProductService productService, IToolsService toolsService, ICategoryService categoryService)
        {
            _userService = userService;
            _productService = productService;
            _toolsService = toolsService;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            var products = _productService.GetAllProduct().ToList();
            return View(products);
        }

        public IActionResult GetProducts(int pageNumber, int countPerPage, string search)
        {
            List<Product> AllProducts = new List<Product>();
            if (string.IsNullOrEmpty(search))
            {
                AllProducts = _productService.GetAllProduct();
            }
            else
            {
                AllProducts = _productService.GetAllProduct(search);
            }

            int allpageNumber = (AllProducts.Count() % countPerPage == 0) ? (AllProducts.Count() / countPerPage) : (AllProducts.Count() / countPerPage) + 1;

            ProductViewModel productTable = new ProductViewModel
            {
                products = AllProducts.Skip(countPerPage * (pageNumber - 1)).Take(countPerPage).ToList(),
                CurrentPage = pageNumber,
                AllPage = allpageNumber,
                CountPerPage = countPerPage,
                SearchTerm = search

            };

            return PartialView("_productTable", productTable);
        }

        public IActionResult Publish()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View();
        }

        public IActionResult AddVariableRow()
        {
            return PartialView("_complexPriceRow");
        }

        public IActionResult AddProductAttributeRow()
        {
            return PartialView("_productAttributeRow", new AttributeViewModel());

        }

        [HttpPost]
        public ActionResult Publish(string Title, string EnglishTitle, string Slug, string Review, string[] varTitle, string[] varDescription,
                string[] varPrice, string[] varSalePrice, string[] varNumberStock, string[] attributeKeys, string[] attributeValues,
                string[] ProductImageGallery, string[] SelectedCategory, string thumbUrl, string tags, string IsWonder,
                string WonderDate, string WonderTime, string MetaDescription, string Spec1, string Spec2, string Spec3,
                string ExtraDescription, bool IsHidden)
        {
            bool isHaveError = false;
            DateTime? WonderResult = null;

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
                if (varPrice.Count() == 0)
                {
                    ModelState.AddModelError("", "شما حداقل یک متغیر باید داشته باشید");
                    isHaveError = true;
                }
                if (varPrice.Any(string.IsNullOrEmpty))
                {
                    ModelState.AddModelError("", "یک یا چند متغیر قیمت اصلی ندارد");
                    isHaveError = true;
                }

                if (varTitle.Count() > 1 && varTitle.Any(string.IsNullOrEmpty))
                {
                    ModelState.AddModelError("", "اگر بیش از یک متغیر دارید، عنوان برای همه متغیرها اجباری است");
                    isHaveError = true;
                }

                if (string.IsNullOrEmpty(Review))
                {
                    ModelState.AddModelError("", "متن خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(tags))
                {
                    ModelState.AddModelError("", "برچسب خالی است");
                    isHaveError = true;
                }
                if (SelectedCategory.Count() == 0)
                {
                    ModelState.AddModelError("", "حداقل یک دسته بندی مشخص کنید");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(thumbUrl))
                {
                    ModelState.AddModelError("", "تصویر شاخص انتخاب کنید");
                    isHaveError = true;
                }
                if (IsWonder != null)
                {
                    if (string.IsNullOrEmpty(WonderDate))
                    {
                        ModelState.AddModelError("", "تاریخ پایان تخفیف شگفت انگیز محصول مشخص نشده است");
                        isHaveError = true;
                    }
                }
                if (!string.IsNullOrEmpty(WonderDate))
                {
                    string[] parts = WonderDate.Split('/');
                    if (parts.Length != 3)
                    {
                        ModelState.AddModelError("", "تاریخ پایان تخفیف شگفت انگیز محصول معتبر نیست");
                        isHaveError = true;
                    }
                    else
                    {
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        int day = int.Parse(parts[2]);

                        string[] timeParts = WonderTime.Split(':');
                        int hour = int.Parse(timeParts[0]);
                        int minute = int.Parse(timeParts[1]);

                        PersianCalendar pc = new PersianCalendar();

                        try
                        {
                            WonderResult = pc.ToDateTime(year, month, day, hour, minute, 0, 0);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            ModelState.AddModelError("", "تاریخ پایان تخفیف شگفت انگیز محصول معتبر نیست");
                            isHaveError = true;
                        }

                    }

                }
                if (!isHaveError)
                {
                    Product p = new Product();
                    p.Title = Title;
                    p.EnglishTitle = EnglishTitle;
                    var newSlug = _toolsService.GenerateSlug(Slug);
                    p.Slug = _productService.GetUnicSlug(newSlug, -1);
                    p.MetaDescription = MetaDescription;
                    p.Review = Review;
                    p.CreateTime = DateTime.Now;
                    p.UserId = _userService.isExistUser(User.Identity.Name).Id;
                    p.PhotoId = int.Parse(thumbUrl.Replace("image_", string.Empty));
                    p.Status = "Publish";
                    string imageGalleries = string.Join(",", ProductImageGallery);
                    p.Gallery = imageGalleries;
                    p.IsWonderProduct = (IsWonder != null) ? true : false;
                    p.Spec1 = Spec1;
                    p.Spec2 = Spec2;
                    p.Spec3 = Spec3;
                    p.ExtraDescription = ExtraDescription;
                    p.IsHidden = IsHidden;

                    p.WonderTime = WonderResult;

                    _productService.SaveProduct(p);

                    if (p.IsWonderProduct)
                    {
                        if (WonderResult > DateTime.Now)
                        {
                            var diffTime = WonderResult - DateTime.Now;
                            p.WonderJobId = BackgroundJob.Schedule<IProductService>(x => x.FinishWonder(p.Id), diffTime.Value);
                        }
                    }

                    _productService.AddVariableToProduct(p.Id, varTitle, varDescription, varPrice, varSalePrice, varNumberStock);
                    _productService.AddAttributeToProduct(p.Id, attributeKeys, attributeValues);
                    _productService.AddCategoryToProduct(p.Id, SelectedCategory);
                    _productService.AddTagsToProduct(tags, p.Id);

                    TempData["Message"] = "محصول با موفقیت منتشر شد";


                    return RedirectToAction("Edit", "Product", new { id = p.Id });
                }

                else
                {
                    ViewBag.varTitle = varTitle;
                    ViewBag.varDescription = varDescription;
                    ViewBag.varPrice = varPrice;
                    ViewBag.varSalePrice = varSalePrice;
                    ViewBag.varNumberStock = varNumberStock;
                    ViewBag.AttSelected = attributeKeys;
                    ViewBag.attributeValues = attributeValues;
                    ViewBag.ProductImageGallery = ProductImageGallery;
                    ViewBag.SelectedCategory = SelectedCategory;
                    ViewBag.Thumb = thumbUrl;
                    ViewBag.Tags = tags;
                    ViewBag.isWonder = (IsWonder != null) ? true : false;
                    ViewBag.WonderTime = WonderTime;
                    ViewBag.WonderDate = WonderDate;


                    return View(new Product()
                    {
                        Title = Title,
                        EnglishTitle = EnglishTitle,
                        Slug = Slug,
                        Review = Review,
                        MetaDescription = MetaDescription,
                        WonderTime = WonderResult,
                        Spec1 = Spec1,
                        Spec2 = Spec2,
                        Spec3 = Spec3,
                        ExtraDescription = ExtraDescription,
                        IsHidden = IsHidden,
                    });
                }
            }


            return View();
        }

        public IActionResult Edit(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            var product = _productService.GetProduct(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();


            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(int ProductId, string Title, string EnglishTitle, string Slug, string Review, string[] varTitle, string[] varDescription,
                string[] varPrice, string[] varSalePrice, string[] varNumberStock, string[] attributeKeys, string[] attributeValues,
                string[] ProductImageGallery, string[] SelectedCategory, string thumbUrl, string tags, string IsWonder,
                string WonderDate, string WonderTime, string MetaDescription, string submittype, string Spec1, string Spec2, string Spec3,
                string ExtraDescription, bool IsHidden)
        {
            bool isHaveError = false;
            DateTime? WonderResult = null;
            Product p = _productService.GetProduct(ProductId);
            var isDrafted = (p.Status == "Draft") ? true : false;
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
                if (varPrice.Count() == 0)
                {
                    ModelState.AddModelError("", "شما حداقل یک متغیر باید داشته باشید");
                    isHaveError = true;
                }
                if (varPrice.Any(string.IsNullOrEmpty))
                {
                    ModelState.AddModelError("", "یک یا چند متغیر قیمت اصلی ندارد");
                    isHaveError = true;
                }

                if (varTitle.Count() > 1 && varTitle.Any(string.IsNullOrEmpty))
                {
                    ModelState.AddModelError("", "اگر بیش از یک متغیر دارید، عنوان برای همه متغیرها اجباری است");
                    isHaveError = true;
                }

                if (string.IsNullOrEmpty(Review))
                {
                    ModelState.AddModelError("", "متن خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(tags))
                {
                    ModelState.AddModelError("", "برچسب خالی است");
                    isHaveError = true;
                }
                if (SelectedCategory.Count() == 0)
                {
                    ModelState.AddModelError("", "حداقل یک دسته بندی مشخص کنید");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(thumbUrl))
                {
                    ModelState.AddModelError("", "تصویر شاخص انتخاب کنید");
                    isHaveError = true;
                }
                if (IsWonder != null)
                {
                    if (string.IsNullOrEmpty(WonderDate))
                    {
                        ModelState.AddModelError("", "تاریخ پایان تخفیف شگفت انگیز محصول مشخص نشده است");
                        isHaveError = true;
                    }
                }
                if (!string.IsNullOrEmpty(WonderDate))
                {
                    string[] parts = WonderDate.Split('/');
                    if (parts.Length != 3)
                    {
                        ModelState.AddModelError("", "تاریخ پایان تخفیف شگفت انگیز محصول معتبر نیست");
                        isHaveError = true;
                    }
                    else
                    {
                        int year = int.Parse(parts[0]);
                        int month = int.Parse(parts[1]);
                        int day = int.Parse(parts[2]);

                        string[] timeParts = WonderTime.Split(':');
                        int hour = int.Parse(timeParts[0]);
                        int minute = int.Parse(timeParts[1]);

                        PersianCalendar pc = new PersianCalendar();

                        try
                        {
                            WonderResult = pc.ToDateTime(year, month, day, hour, minute, 0, 0);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            ModelState.AddModelError("", "تاریخ پایان تخفیف شگفت انگیز محصول معتبر نیست");
                            isHaveError = true;
                        }

                    }

                }


                if (submittype == "SaveDraft")
                {
                    isHaveError = false;
                }
                if (!isHaveError)
                {

                    if (submittype == "Edit")
                    {
                        p.Title = Title;
                        p.EnglishTitle = EnglishTitle;
                        p.MetaDescription = MetaDescription;
                        if (!string.IsNullOrEmpty(Slug))
                        {
                            var newSlug = _toolsService.GenerateSlug(Slug);
                            p.Slug = _productService.GetUnicSlug(newSlug, p.Id);
                        }
                        p.Review = Review;
                        p.PhotoId = int.Parse(thumbUrl.Replace("image_", string.Empty));
                        p.Status = "Publish";

                        p.IsWonderProduct = (IsWonder != null) ? true : false;

                        p.WonderTime = WonderResult;

                        string imageGalleries = string.Join(",", ProductImageGallery);
                        p.Gallery = imageGalleries;

                        p.Spec1 = Spec1;
                        p.Spec2 = Spec2;
                        p.Spec3 = Spec3;
                        p.ExtraDescription = ExtraDescription;
                        p.IsHidden = IsHidden;

                        _productService.SaveDatabse();

                        if (!string.IsNullOrEmpty(p.WonderJobId))
                        {
                            BackgroundJob.Delete(p.WonderJobId);
                            p.WonderJobId = null;
                        }

                        if (p.IsWonderProduct)
                        {
                            if (WonderResult > DateTime.Now)
                            {
                                var diffTime = WonderResult - DateTime.Now;
                                p.WonderJobId = BackgroundJob.Schedule<IProductService>(x => x.FinishWonder(p.Id), diffTime.Value);
                            }
                        }


                        _productService.UpdateProductVariables(p.Id, varTitle, varDescription, varPrice, varSalePrice, varNumberStock);
                        _productService.UpdateProductAttributes(p.Id, attributeKeys, attributeValues);
                        _productService.UpdateProductCategory(p.Id, SelectedCategory);
                        _productService.UpdateProductTag(tags, p.Id);

                        if (isDrafted)
                        {
                            TempData["Message"] = "محصول با موفقیت منتشر شد";

                        }
                        else
                        {
                            TempData["Message"] = "محصول با موفقیت ذخیره شد";

                        }
                    }
                    else
                    {
                        p.Title = (string.IsNullOrEmpty(Title)) ? "بدون تیتر" : Title;
                        p.EnglishTitle = (string.IsNullOrEmpty(EnglishTitle)) ? "بدون تیتر" : EnglishTitle;
                        var newSlug = _toolsService.GenerateSlug(Slug);
                        p.Slug = newSlug;
                        p.MetaDescription = MetaDescription;
                        p.Review = Review;
                        p.CreateTime = DateTime.Now;
                        p.UserId = _userService.isExistUser(User.Identity.Name).Id;

                        if (!string.IsNullOrEmpty(thumbUrl))
                            p.PhotoId = int.Parse(thumbUrl.Replace("image_", string.Empty));

                        p.IsWonderProduct = (IsWonder != null) ? true : false;
                        p.WonderTime = WonderResult;
                        p.Spec1 = Spec1;
                        p.Spec2 = Spec2;
                        p.Spec3 = Spec3;
                        p.ExtraDescription = ExtraDescription;
                        p.IsHidden = IsHidden;

                        p.Status = "Draft";
                        string imageGalleries = string.Join(",", ProductImageGallery);
                        p.Gallery = imageGalleries;

                        _productService.SaveDatabse();

                        _productService.UpdateProductVariables(p.Id, varTitle, varDescription, varPrice, varSalePrice, varNumberStock);
                        _productService.UpdateProductAttributes(p.Id, attributeKeys, attributeValues);
                        _productService.UpdateProductCategory(p.Id, SelectedCategory);

                        if (tags != null)
                            _productService.UpdateProductTag(tags, p.Id);
                        else
                            _productService.DeleteProductTag(p.Id);

                        TempData["Message"] = "پیش نویس محصول با موفقیت ذخیره شد";
                    }



                    return RedirectToAction("Edit", "Product", new { id = p.Id });
                }

                else
                {
                    ViewBag.varTitle = varTitle;
                    ViewBag.varDescription = varDescription;
                    ViewBag.varPrice = varPrice;
                    ViewBag.varSalePrice = varSalePrice;
                    ViewBag.varNumberStock = varNumberStock;
                    ViewBag.AttSelected = attributeKeys;
                    ViewBag.attributeValues = attributeValues;
                    ViewBag.ProductImageGallery = ProductImageGallery;
                    ViewBag.SelectedCategory = SelectedCategory;
                    ViewBag.Thumb = thumbUrl;
                    ViewBag.Tags = tags;
                    ViewBag.isWonder = (IsWonder != null) ? true : false;

                    return View(new Product()
                    {
                        Id = p.Id,
                        Title = Title,
                        EnglishTitle = EnglishTitle,
                        Slug = p.Slug,
                        Review = Review,
                        MetaDescription = MetaDescription,
                        Status = (isDrafted) ? "Draft" : "Publish",
                        WonderTime = WonderResult,
                        Spec1 = Spec1,
                        Spec2 = Spec2,
                        Spec3 = Spec3,
                        ExtraDescription = ExtraDescription,
                        IsHidden = IsHidden,
                    });
                }
            }


            return View();
        }

        public IActionResult SaveDraft(string Title, string EnglishTitle, string Slug, string Review, string[] varTitle, string[] varDescription,
                string[] varPrice, string[] varSalePrice, string[] varNumberStock, string[] attributeKeys, string[] attributeValues,
                string[] ProductImageGallery, string[] SelectedCategory, string thumbUrl, string tags, string IsWonder,
                string WonderDate, string WonderTime, string MetaDescription, string Spec1, string Spec2, string Spec3,
                string ExtraDescription, bool IsHidden)
        {


            Product p = new Product();
            p.Title = (string.IsNullOrEmpty(Title)) ? "بدون تیتر" : Title;
            p.EnglishTitle = EnglishTitle;
            var newSlug = _toolsService.GenerateSlug(Slug);
            p.Slug = newSlug;
            p.MetaDescription = MetaDescription;
            p.Review = Review;
            p.CreateTime = DateTime.Now;
            p.UserId = _userService.isExistUser(User.Identity.Name).Id;

            if (!string.IsNullOrEmpty(thumbUrl))
                p.PhotoId = int.Parse(thumbUrl.Replace("image_", string.Empty));

            p.IsWonderProduct = (IsWonder != null) ? true : false;

            if (!string.IsNullOrEmpty(WonderDate) && !string.IsNullOrEmpty(WonderTime))
            {
                string[] parts = WonderDate.Split('/');
                if (parts.Length != 3)
                {
                    p.WonderTime = null;
                }
                else
                {
                    int year = int.Parse(parts[0]);
                    int month = int.Parse(parts[1]);
                    int day = int.Parse(parts[2]);

                    string[] timeParts = WonderTime.Split(':');
                    int hour = int.Parse(timeParts[0]);
                    int minute = int.Parse(timeParts[1]);

                    PersianCalendar pc = new PersianCalendar();

                    try
                    {
                        p.WonderTime = pc.ToDateTime(year, month, day, hour, minute, 0, 0);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        p.WonderTime = null;
                    }
                }
            }
            else
            {
                p.WonderTime = null;
            }

            p.Spec1 = Spec1;
            p.Spec2 = Spec2;
            p.Spec3 = Spec3;
            p.ExtraDescription = ExtraDescription;
            p.IsHidden = IsHidden;

            p.Status = "Draft";

            string imageGalleries = string.Join(",", ProductImageGallery);
            p.Gallery = imageGalleries;

            _productService.SaveProduct(p);

            _productService.AddVariableToProduct(p.Id, varTitle, varDescription, varPrice, varSalePrice, varNumberStock);
            _productService.AddAttributeToProduct(p.Id, attributeKeys, attributeValues);
            _productService.AddCategoryToProduct(p.Id, SelectedCategory);

            if (tags != null)
                _productService.AddTagsToProduct(tags, p.Id);

            TempData["Message"] = "پیش نویس محصول با موفقیت ذخیره شد";


            return RedirectToAction("Edit", "Product", new { id = p.Id });
        }

        public void DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
        }

        public IActionResult Orders()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View(_productService.GetAllOrders().Where(o => o.IsFinal == true).ToList());
        }

        public IActionResult GetOrders(int pageNumber, int countPerPage, int allpage)
        {

            OrderViewModel orderTable = new OrderViewModel
            {
                Orders = _productService.GetSpecificOrders(pageNumber, countPerPage),
                CurrentPage = pageNumber,
                AllPage = allpage,
                CountPerPage = countPerPage
            };

            return PartialView("_orderTable", orderTable);
        }

        public IActionResult OrderDetails(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            var order = _productService.GetOrderById(id);
            return View(order);

        }

        [HttpPost]
        public IActionResult OrderDetails(int id, string StatusSelected, string TrackingCode)
        {
            var order = _productService.GetOrderById(id);
            _productService.SetStatusSelected(order, StatusSelected, TrackingCode);
            ViewBag.Message = "اطلاعات با موفقیت ذخیره شد";
            return View(order);

        }

        public void DeleteOrder(int id)
        {
            _productService.DeleteOrder(id);

        }

        public IActionResult CouponCodes()
        {
            return View(_productService.GetAllCoupons());
        }

        public IActionResult GetCoupons(int pageNumber, int countPerPage, int allpage)
        {
            CouponViewModel CouponTable = new CouponViewModel
            {
                Coupons = _productService.GetSpecificCouponCodes(pageNumber, countPerPage),
                CurrentPage = pageNumber,
                AllPage = allpage,
                CountPerPage = countPerPage
            };

            return PartialView("_couponTable", CouponTable);
        }

        public IActionResult PublishCoupon()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public IActionResult PublishCoupon(string Code, string Percent, string MaxValue, string StartDate, string EndDate, string MinTotalOrder, string OnlyFirstOrder, string AnyUserOneTime)
        {
            int? maxValueFinal = null;
            int? minValueFinal = null;
            DateTime? startDateFinal = null;
            DateTime? endDateFinal = null;

            bool IsHaveError = false;

            if (string.IsNullOrEmpty(Code))
            {
                ModelState.AddModelError("", "پر کردن کد تخفیف اجباری است");
                IsHaveError = true;
            }
            if (string.IsNullOrEmpty(Percent) || Percent == "0")
            {
                ModelState.AddModelError("", "پر کردن درصد تخفیف اجباری است");
                IsHaveError = true;
            }
            else
            {
                if (!int.TryParse(Percent, out int codePercent))
                {
                    ModelState.AddModelError("", "درصد تخفیف را فقط با اعداد انگلیسی پر کنید");
                    IsHaveError = true;
                }
                else
                {
                    if (codePercent <= 0 || codePercent >= 100)
                    {
                        ModelState.AddModelError("", "درصد تخفیف باید یک عدد بین 0 تا 100 باشد");
                        IsHaveError = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(MaxValue))
            {
                if (!int.TryParse(MaxValue, out int maxValue))
                {
                    ModelState.AddModelError("", "حداکثر تخفیف را فقط با اعداد انگلیسی پر کنید");
                    IsHaveError = true;
                }
                else
                {
                    maxValueFinal = maxValue;
                }

            }
            if (!string.IsNullOrEmpty(MinTotalOrder))
            {
                if (!int.TryParse(MinTotalOrder, out int minValue))
                {
                    ModelState.AddModelError("", "حداقل مبلغ را فقط با اعداد انگلیسی پر کنید");
                    IsHaveError = true;
                }
                else
                {
                    minValueFinal = minValue;
                }

            }
            if (!string.IsNullOrEmpty(StartDate))
            {
                if (!DateTime.TryParse(StartDate, out DateTime startDate))
                {
                    ModelState.AddModelError("", "مقدار تاریخ شروع را به درستی وارد کنید");
                    IsHaveError = true;
                }
                else
                {
                    startDateFinal = startDate;
                }
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                if (!DateTime.TryParse(EndDate, out DateTime endDate))
                {
                    ModelState.AddModelError("", "مقدار تاریخ پایان را به درستی وارد کنید");
                    IsHaveError = true;
                }
                else
                {
                    endDateFinal = endDate;
                }
            }

            if (_productService.IsExistCoupon(Code) != null)
            {
                ModelState.AddModelError("", "این کد تخفیف قبلا ایجاد شده است");
                IsHaveError = true;
            }

            bool only = (string.IsNullOrEmpty(OnlyFirstOrder) ? false : true);
            bool any = (string.IsNullOrEmpty(AnyUserOneTime) ? false : true);

            if (!IsHaveError)
            {
                int couponId = _productService.AddCouponCode(Code, int.Parse(Percent), maxValueFinal, startDateFinal, endDateFinal, minValueFinal, only, any);


                TempData["Message"] = "کد تخفیف با موفقیت ایجاد شد";


                return RedirectToAction("EditCoupon", "Product", new { id = couponId });
            }
            else
            {

                return View(new Coupon()
                {
                    Code = Code,
                    Percent = (int.TryParse(Percent, out int cPercent)) ? cPercent : 0,
                    MaxValue = maxValueFinal,
                    MinTotalOrder = minValueFinal,
                    StartDate = startDateFinal,
                    EndDate = endDateFinal,
                    OnlyFirstOrder = only,
                    AnyUserOneTime = any,
                });
            }


        }

        public IActionResult EditCoupon(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            var coupon = _productService.GetCoupon(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();


            return View(coupon);
        }

        [HttpPost]
        public IActionResult EditCoupon(int CouponId, string Percent, string MaxValue, string StartDate, string EndDate, string MinTotalOrder, string OnlyFirstOrder, string AnyUserOneTime)
        {
            int? maxValueFinal = null;
            int? minValueFinal = null;
            DateTime? startDateFinal = null;
            DateTime? endDateFinal = null;

            bool IsHaveError = false;

            if (string.IsNullOrEmpty(Percent) || Percent == "0")
            {
                ModelState.AddModelError("", "پر کردن درصد تخفیف اجباری است");
                IsHaveError = true;
            }
            else
            {
                if (!int.TryParse(Percent, out int codePercent))
                {
                    ModelState.AddModelError("", "درصد تخفیف را فقط با اعداد انگلیسی پر کنید");
                    IsHaveError = true;
                }
                else
                {
                    if (codePercent <= 0 || codePercent >= 100)
                    {
                        ModelState.AddModelError("", "درصد تخفیف باید یک عدد بین 0 تا 100 باشد");
                        IsHaveError = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(MaxValue))
            {
                if (!int.TryParse(MaxValue, out int maxValue))
                {
                    ModelState.AddModelError("", "حداکثر تخفیف را فقط با اعداد انگلیسی پر کنید");
                    IsHaveError = true;
                }
                else
                {
                    maxValueFinal = maxValue;
                }

            }
            if (!string.IsNullOrEmpty(MinTotalOrder))
            {
                if (!int.TryParse(MinTotalOrder, out int minValue))
                {
                    ModelState.AddModelError("", "حداقل مبلغ را فقط با اعداد انگلیسی پر کنید");
                    IsHaveError = true;
                }
                else
                {
                    minValueFinal = minValue;
                }

            }
            if (!string.IsNullOrEmpty(StartDate))
            {
                if (!DateTime.TryParse(StartDate, out DateTime startDate))
                {
                    ModelState.AddModelError("", "مقدار تاریخ شروع را به درستی وارد کنید");
                    IsHaveError = true;
                }
                else
                {
                    startDateFinal = startDate;
                }
            }
            if (!string.IsNullOrEmpty(EndDate))
            {
                if (!DateTime.TryParse(EndDate, out DateTime endDate))
                {
                    ModelState.AddModelError("", "مقدار تاریخ پایان را به درستی وارد کنید");
                    IsHaveError = true;
                }
                else
                {
                    endDateFinal = endDate;
                }
            }

            bool only = (string.IsNullOrEmpty(OnlyFirstOrder) ? false : true);
            bool any = (string.IsNullOrEmpty(AnyUserOneTime) ? false : true);

            if (!IsHaveError)
            {
                _productService.UpdateCouponCode(CouponId, int.Parse(Percent), maxValueFinal, startDateFinal, endDateFinal, minValueFinal, only, any);

                TempData["Message"] = "کد تخفیف با موفقیت ذخیره شد";

                return RedirectToAction("EditCoupon", "Product", new { id = CouponId });
            }
            else
            {

                return View(new Coupon()
                {
                    Id = CouponId,
                    Percent = (int.TryParse(Percent, out int cPercent)) ? cPercent : 0,
                    MaxValue = maxValueFinal,
                    MinTotalOrder = minValueFinal,
                    StartDate = startDateFinal,
                    EndDate = endDateFinal,
                    OnlyFirstOrder = only,
                    AnyUserOneTime = any,
                });
            }


        }

        public void DeleteCouponCode(int id)
        {
            _productService.DeleteCoupon(id);
        }

        public IActionResult PrintOrder(int orderId)
        {
            var orderProducts = _productService.GetProductsInOrder(orderId);
            double height = 30 + (orderProducts.Count * 8) + 8 + 8 + 8 + 30 + 18;
            ViewAsPdf report = new ViewAsPdf("PrintOrder", orderProducts)
            {
                FileName = "order_" + orderId + ".pdf",
                PageMargins = { Left = 0, Right = 0, Top = 1, Bottom = 1 }, // In millimeters.
                PageWidth = 80,
                PageHeight = height,
            };

            return report;

            //return View(orderFoods);
        }

        public IActionResult PrintShippingAddress(int orderId)
        {
            var order = _productService.GetOrderById(orderId);
            ViewAsPdf report = new ViewAsPdf("PrintShippingAddress", order)
            {
                FileName = "BillingOrder_" + orderId + ".pdf",
                PageMargins = { Left = 0, Right = 0, Top = 1, Bottom = 1 }, // In millimeters.
                PageWidth = 210,
                PageHeight = 148,
            };

            return report;
            //return View(order);
        }

        public IActionResult Shipping()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            var shippings = _productService.GetAllShipping().ToList();
            return View(shippings);
        }

        public IActionResult GetShippings(int pageNumber, int countPerPage)
        {
            List<Shipping> AllShippings = new List<Shipping>();

            AllShippings = _productService.GetAllShipping();


            int allpageNumber = (AllShippings.Count() % countPerPage == 0) ? (AllShippings.Count() / countPerPage) : (AllShippings.Count() / countPerPage) + 1;

            ShippingViewModel shippingTable = new ShippingViewModel
            {
                shippings = AllShippings.Skip(countPerPage * (pageNumber - 1)).Take(countPerPage).ToList(),
                CurrentPage = pageNumber,
                AllPage = allpageNumber,
                CountPerPage = countPerPage,
            };

            return PartialView("_shippingTable", shippingTable);
        }

        public IActionResult PublishShipping()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View();
        }


        public IActionResult EditShipping(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            var shipping = _productService.GetShipping(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();


            return View(shipping);
        }


        [HttpPost]
        public ActionResult PublishShipping(string Title, string Description, int ShipPrice, bool IsTehran, int MinPrice, int MaxPrice)
        {
            bool isHaveError = false;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "عنوان خالی است");
                    isHaveError = true;
                }

                if (!isHaveError)
                {
                    Shipping p = new Shipping();
                    p.Title = Title;
                    p.Description = Description;
                    p.ShipPrice = ShipPrice;
                    p.IsTehran = IsTehran;
                    p.MinPrice = MinPrice;
                    p.MaxPrice = MaxPrice;

                    _productService.SaveShipping(p);


                    TempData["Message"] = "شیوه ارسال با موفقیت منتشر شد";


                    return RedirectToAction("EditShipping", "Product", new { id = p.Id });
                }

                else
                {
                    return View(new Shipping()
                    {
                        Title = Title,
                        Description = Description,
                        IsTehran = IsTehran,
                        MinPrice = MinPrice,
                        MaxPrice = MaxPrice,
                        ShipPrice = ShipPrice,
                    });
                }
            }


            return View();
        }

        [HttpPost]
        public ActionResult EditShipping(int ShippingId, string Title, string Description, int ShipPrice, bool IsTehran, int MinPrice, int MaxPrice)
        {
            var p = _productService.GetShipping(ShippingId);
            bool isHaveError = false;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "عنوان خالی است");
                    isHaveError = true;
                }

                if (!isHaveError)
                {
                    p.Title = Title;
                    p.Description = Description;
                    p.ShipPrice = ShipPrice;
                    p.IsTehran = IsTehran;
                    p.MinPrice = MinPrice;
                    p.MaxPrice = MaxPrice;

                    _productService.SaveDatabse();


                    TempData["Message"] = "شیوه ارسال با موفقیت ذخیره شد";


                    return RedirectToAction("EditShipping", "Product", new { id = p.Id });
                }

                else
                {
                    return View(new Shipping()
                    {
                        Id = ShippingId,
                        Title = Title,
                        Description = Description,
                        IsTehran = IsTehran,
                        MinPrice = MinPrice,
                        MaxPrice = MaxPrice,
                        ShipPrice = ShipPrice,
                    });
                }
            }


            return View();
        }

        public IActionResult DeleteProductOrder(int id, int orderId)
        {
            var po = _productService.GetOrderProductById(id);
            if (po.Variable != null)
            {
                po.Variable.NumberInStock += po.number;
            }

            _productService.DeleteProductFromOrder(id);

            var order = _productService.GetOrderById(orderId);
            return PartialView("_orderProductTable", order);
        }

        public IActionResult LoadVariables(int prId)
        {
            return PartialView("_variableSelector", prId);
        }

        public IActionResult AddVariableToOrder(int OrderId, int VariableSelected, int Number, int Price)
        {
            if (Number > 0 && Price > 0)
            {
                var variable = _productService.GetSelectedVariable(VariableSelected);
                if (variable != null)
                {
                    var newProductOrder = new OrderProduct()
                    {
                        number = Number,
                        Price = Price,
                        ProductId = variable.ProductId,
                        VariableTitle = variable.Title,
                        OrderId = OrderId,
                    };

                    _productService.AddCustomVariableProductToOrder(newProductOrder);
                }
            }

            var order = _productService.GetOrderById(OrderId);
            return PartialView("_orderProductTable", order);

        }

        public IActionResult DeleteCouponOrder(int orderId)
        {
            _productService.DeleteCouponOrder(orderId);

            var order = _productService.GetOrderById(orderId);
            return PartialView("_orderProductTable", order);
        }
    }
}
