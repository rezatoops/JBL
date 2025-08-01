using Core.Services;
using Core.Services.Interfaces;
using Datalayer.Entities;
using Datalayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace PandS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class SettingController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        IWebHostEnvironment _webHostEnvironment;

        public SettingController(IUserService userService, ISettingService settingService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _settingService = settingService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShopDesign()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }

            return View(_settingService.GetAllShopDesigns());
        }

        public IActionResult PublishShopDesign()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public IActionResult PublishShopDesign(string Name, string Title, int Type, int catSelected, string Banner1Link, string banner1ImgUrl,
            string Banner2Link, string banner2ImgUrl, string Banner3Link, string banner3ImgUrl, string Banner4Link, string banner4ImgUrl,
            string BannerWonderLink, string bannerWonderImgUrl, int? sort, int? NumberOfProduct, bool IsOnlyAvailable)
        {

            if (ModelState.IsValid)
            {
                ShopDesign p = new ShopDesign();
                p.Title = Title;
                p.Type = (ShopRawEnum)Type;
                if (catSelected != 0)
                {
                    p.CategoryId = catSelected;
                }
                else
                {
                    p.CategoryId = null;
                }
                p.Banner1Link = Banner1Link;
                p.Banner2Link = Banner2Link;
                p.Banner3Link = Banner3Link;
                p.Banner4Link = Banner4Link;
                p.BannerWonderLink = BannerWonderLink;
                p.Sort = sort == null ? 0 : sort.Value;
                p.NumberOfProduct = NumberOfProduct == null ? 0 : NumberOfProduct.Value;
                p.IsOnlyAvailable = IsOnlyAvailable;
                p.Name = Name;

                if (banner1ImgUrl != null)
                {
                    p.Banner1ImgId = int.Parse(banner1ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner1ImgId = null;
                }

                if (banner2ImgUrl != null)
                {
                    p.Banner2ImgId = int.Parse(banner2ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner2ImgId = null;
                }
                if (banner3ImgUrl != null)
                {
                    p.Banner3ImgId = int.Parse(banner3ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner3ImgId = null;
                }

                if (banner4ImgUrl != null)
                {
                    p.Banner4ImgId = int.Parse(banner4ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner4ImgId = null;
                }

                if (bannerWonderImgUrl != null)
                {
                    p.BannerWonderImgId = int.Parse(bannerWonderImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.BannerWonderImgId = null;
                }

                _settingService.SaveShopDesign(p);

                TempData["Message"] = "ردیف محصول با موفقیت منتشر شد";

                return RedirectToAction("ShopDesign", "Setting");
            }
            return View();
        }

        public IActionResult EditShopDesign(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }

            var shopDesign = _settingService.GetShopDesignById(id);

            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            return View(shopDesign);
        }

        [HttpPost]
        public IActionResult EditShopDesign(int ShopDesignId,string Name, string Title, int Type, int catSelected, string Banner1Link, string banner1ImgUrl,
            string Banner2Link, string banner2ImgUrl, string Banner3Link, string banner3ImgUrl, string Banner4Link, string banner4ImgUrl,
            string BannerWonderLink, string bannerWonderImgUrl, int? sort, int? NumberOfProduct, bool IsOnlyAvailable)
        {

            ShopDesign p = _settingService.GetShopDesignById(ShopDesignId);
            if (ModelState.IsValid)
            {
                p.Title = Title;
                p.Type = (ShopRawEnum)Type;
                if (catSelected != 0)
                {
                    p.CategoryId = catSelected;
                }
                else
                {
                    p.CategoryId = null;
                }
                p.Banner1Link = Banner1Link;
                p.Banner2Link = Banner2Link;
                p.Banner3Link = Banner3Link;
                p.Banner4Link = Banner4Link;
                p.BannerWonderLink = BannerWonderLink;
                p.Sort = sort == null ? 0 : sort.Value;
                p.NumberOfProduct = NumberOfProduct == null ? 0 : NumberOfProduct.Value;
                p.IsOnlyAvailable = IsOnlyAvailable;
                p.Name = Name;

                if (banner1ImgUrl != null)
                {
                    p.Banner1ImgId = int.Parse(banner1ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner1ImgId = null;
                }

                if (banner2ImgUrl != null)
                {
                    p.Banner2ImgId = int.Parse(banner2ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner2ImgId = null;
                }
                if (banner3ImgUrl != null)
                {
                    p.Banner3ImgId = int.Parse(banner3ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner3ImgId = null;
                }

                if (banner4ImgUrl != null)
                {
                    p.Banner4ImgId = int.Parse(banner4ImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.Banner4ImgId = null;
                }

                if (bannerWonderImgUrl != null)
                {
                    p.BannerWonderImgId = int.Parse(bannerWonderImgUrl.Replace("image_", string.Empty));

                }
                else
                {
                    p.BannerWonderImgId = null;
                }

                _settingService.SaveDatabase();

                TempData["Message"] = "ردیف محصول با موفقیت دخیره شد";

                return RedirectToAction("EditShopDesign", "Setting", new { id = p.Id });
            }
            return View();
        }

        public void DeleteShopDesign(int id)
        {
            _settingService.DeleteShopDesign(id);
        }

        public IActionResult HeaderSettings()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HeaderSettings(IFormFile Logo, IFormFile Fav, string footerText, string instagram,
            string whatsup, string linkedin, string youtube, string Address, string FooterPhone, string FooterEmail, string Analytics,
            string Enamad, string Goftino, string StoreTime1, string StoreTime2, string PhoneInMobile,
            string FLinkText1, string FLink1, string FLinkText2, string FLink2, string FLinkText3, string FLink3, string FLinkText4, string FLink4,
            string FLinkText5, string FLink5, string FLinkText6, string FLink6, string FLinkText7, string FLink7, string FLinkText8, string FLink8, string SiteTitle)
        {

            _settingService.SetSettingValue("footerText", footerText);
            _settingService.SetSettingValue("instagram", instagram);
            _settingService.SetSettingValue("whatsup", whatsup);
            _settingService.SetSettingValue("linkedin", linkedin);
            _settingService.SetSettingValue("youtube", youtube);
            _settingService.SetSettingValue("Address", Address);
            _settingService.SetSettingValue("FooterPhone", FooterPhone);
            _settingService.SetSettingValue("FooterEmail", FooterEmail);
            _settingService.SetSettingValue("Analytics", Analytics);
            _settingService.SetSettingValue("Enamad", Enamad);
            _settingService.SetSettingValue("Goftino", Goftino);
            _settingService.SetSettingValue("StoreTime1", StoreTime1);
            _settingService.SetSettingValue("StoreTime2", StoreTime2);
            _settingService.SetSettingValue("PhoneInMobile", PhoneInMobile);
            _settingService.SetSettingValue("SiteTitle", SiteTitle);

            _settingService.SetSettingValue("FLinkText1", FLinkText1);
            _settingService.SetSettingValue("FLink1", FLink1);
            _settingService.SetSettingValue("FLinkText2", FLinkText2);
            _settingService.SetSettingValue("FLink2", FLink2);
            _settingService.SetSettingValue("FLinkText3", FLinkText3);
            _settingService.SetSettingValue("FLink3", FLink3);
            _settingService.SetSettingValue("FLinkText4", FLinkText4);
            _settingService.SetSettingValue("FLink4", FLink4);
            _settingService.SetSettingValue("FLinkText5", FLinkText5);
            _settingService.SetSettingValue("FLink5", FLink5);
            _settingService.SetSettingValue("FLinkText6", FLinkText6);
            _settingService.SetSettingValue("FLink6", FLink6);
            _settingService.SetSettingValue("FLinkText7", FLinkText7);
            _settingService.SetSettingValue("FLink7", FLink7);
            _settingService.SetSettingValue("FLinkText8", FLinkText8);
            _settingService.SetSettingValue("FLink8", FLink8);

            if (Logo != null)
            {
                if (Logo.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "logo.png");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Logo.CopyToAsync(stream);
                    }
                }
            }

            if (Fav != null)
            {
                if (Fav.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Fav.png");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Fav.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SaveDatabase();

            ViewBag.Message = "اطلاعات با موفقیت ذخیره شد";

            return View();
        }

        public IActionResult WonderSettings()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WonderSettings(IFormFile WBanner, IFormFile WBannerMobile, string WonderStatus,
            string ProductInformation, string WonderBannerLink, string SMSText, string SMSTextCompleted, string SMSTextCanceled,
            string SMSTextRefund)
        {
            if (WBanner != null)
            {
                if (WBanner.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "wBanner.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await WBanner.CopyToAsync(stream);
                    }
                }
            }

            if (WBannerMobile != null)
            {
                if (WBannerMobile.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "wBannerMobile.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await WBannerMobile.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SetSettingValue("ShowWonderful", (!string.IsNullOrEmpty(WonderStatus)) ? "true" : "false");
            _settingService.SetSettingValue("ProductInformation", ProductInformation);
            _settingService.SetSettingValue("WonderBannerLink", WonderBannerLink);
            _settingService.SetSettingValue("SMSText", SMSText);
            _settingService.SetSettingValue("SMSTextCompleted", SMSTextCompleted);
            _settingService.SetSettingValue("SMSTextCanceled", SMSTextCanceled);
            _settingService.SetSettingValue("SMSTextRefund", SMSTextRefund);

            _settingService.SaveDatabase();

            ViewBag.Message = "اطلاعات با موفقیت ذخیره شد";

            return View();
        }

        public IActionResult AboutAndContact()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AboutAndContact(IFormFile AboutImg, IFormFile AboutImgMobile, string AboutusText,
            string ContactAddress, string ContactusPhone, string fax, string ContactusEmail, string GoogleMap)
        {
            if (AboutImg != null)
            {
                if (AboutImg.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "aboutus.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await AboutImg.CopyToAsync(stream);
                    }
                }
            }

            if (AboutImgMobile != null)
            {
                if (AboutImgMobile.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "aboutusmobile.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await AboutImgMobile.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SetSettingValue("AboutusText", AboutusText);
            _settingService.SetSettingValue("ContactAddress", ContactAddress);
            _settingService.SetSettingValue("ContactusPhone", ContactusPhone);
            _settingService.SetSettingValue("fax", fax);
            _settingService.SetSettingValue("ContactusEmail", ContactusEmail);
            _settingService.SetSettingValue("GoogleMap", GoogleMap);

            _settingService.SaveDatabase();


            ViewBag.Message = "تغییرات با موفقیت اعمال شد";
            return View();
        }

        public IActionResult MainPageSetting()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MainPageSetting(string MainPageAboutText,
            IFormFile Img1, string Part1Title, string Part1Text, string Part1Link, string Part1BtnText,
            IFormFile Img2, string Part2Title, string Part2Text, string Part2Link, string Part2BtnText,
            IFormFile Img3, string Part3Title, string Part3Text, string Part3Link, string Part3BtnText,
            IFormFile Img4, string Part4Title, string Part4Text, string Part4Link, string Part4BtnText,
            IFormFile Img5, string Part5Title, string Part5Text, string Part5Link, string Part5BtnText,
            IFormFile Img6, string Part6Title, string Part6Text, string Part6Link, string Part6BtnText,
            IFormFile Img7, string Part7Title, string Part7Text, string Part7Link, string Part7BtnText,
            IFormFile MainPageBlog, string MainPageBlogTitle, string MainPageBlogText, string MainPageBlogLink, string MainPageBlogBtnText)
        {
            _settingService.SetSettingValue("MainPageAboutText", MainPageAboutText);

            if (Img1 != null)
            {
                if (Img1.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "CatImg1.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img1.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SetSettingValue("Part1Title", Part1Title);
            _settingService.SetSettingValue("Part1Text", Part1Text);
            _settingService.SetSettingValue("Part1Link", Part1Link);
            _settingService.SetSettingValue("Part1BtnText", Part1BtnText);

            if (Img2 != null)
            {
                if (Img2.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "CatImg2.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img2.CopyToAsync(stream);
                    }
                }
            }
            _settingService.SetSettingValue("Part2Title", Part2Title);
            _settingService.SetSettingValue("Part2Text", Part2Text);
            _settingService.SetSettingValue("Part2Link", Part2Link);
            _settingService.SetSettingValue("Part2BtnText", Part2BtnText);

            if (Img3 != null)
            {
                if (Img3.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "CatImg3.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img3.CopyToAsync(stream);
                    }
                }
            }
            _settingService.SetSettingValue("Part3Title", Part3Title);
            _settingService.SetSettingValue("Part3Text", Part3Text);
            _settingService.SetSettingValue("Part3Link", Part3Link);
            _settingService.SetSettingValue("Part3BtnText", Part3BtnText);

            if (Img4 != null)
            {
                if (Img4.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "CatImg4.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img4.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SetSettingValue("Part4Title", Part4Title);
            _settingService.SetSettingValue("Part4Text", Part4Text);
            _settingService.SetSettingValue("Part4Link", Part4Link);
            _settingService.SetSettingValue("Part4BtnText", Part4BtnText);

            if (Img5 != null)
            {
                if (Img5.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "CatImg5.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img5.CopyToAsync(stream);
                    }
                }
            }
            _settingService.SetSettingValue("Part5Title", Part5Title);
            _settingService.SetSettingValue("Part5Text", Part5Text);
            _settingService.SetSettingValue("Part5Link", Part5Link);
            _settingService.SetSettingValue("Part5BtnText", Part5BtnText);

            if (Img6 != null)
            {
                if (Img6.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "CatImg6.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img6.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SetSettingValue("Part6Title", Part6Title);
            _settingService.SetSettingValue("Part6Text", Part6Text);
            _settingService.SetSettingValue("Part6Link", Part6Link);
            _settingService.SetSettingValue("Part6BtnText", Part6BtnText);

            if (Img7 != null)
            {
                if (Img7.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "CatImg7.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img7.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SetSettingValue("Part7Title", Part7Title);
            _settingService.SetSettingValue("Part7Text", Part7Text);
            _settingService.SetSettingValue("Part7Link", Part7Link);
            _settingService.SetSettingValue("Part7BtnText", Part7BtnText);


            if (MainPageBlog != null)
            {
                if (MainPageBlog.Length > 0)
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "MainPageBlog.jpg");
                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "assets");

                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await MainPageBlog.CopyToAsync(stream);
                    }
                }
            }

            _settingService.SetSettingValue("MainPageBlogTitle", MainPageBlogTitle);
            _settingService.SetSettingValue("MainPageBlogText", MainPageBlogText);
            _settingService.SetSettingValue("MainPageBlogLink", MainPageBlogLink);
            _settingService.SetSettingValue("MainPageBlogBtnText", MainPageBlogBtnText);

            _settingService.SaveDatabase();


            ViewBag.Message = "تغییرات با موفقیت اعمال شد";
            return View();
        }

        public IActionResult LoadRawTypeView(int typeId, int ShopRawId = 0)
        {
            ShopDesign? Model = null;
            if (ShopRawId > 0) { 
                Model = _settingService.GetShopDesignById(ShopRawId);
            }
            switch (typeId)
            {
                case 0:
                    return PartialView("ShopRawLayouts/_rowByNewest",Model);
                case 1:
                    return PartialView("ShopRawLayouts/_rowByCategory", Model);
                case 2:
                    return PartialView("ShopRawLayouts/_rowByWonder", Model);
                case 3:
                    return PartialView("ShopRawLayouts/_rowByWonderWithTimer", Model);
                case 4:
                    return PartialView("ShopRawLayouts/_rowBySortingProduct", Model);
                case 5:
                    return PartialView("ShopRawLayouts/_rowByAds", Model);
                default:
                    return PartialView("ShopRawLayouts/_rowByNewest", Model);
            }
        }
    }
}
