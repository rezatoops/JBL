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

    public class MediaController : Controller
    {
        IMediaService _mediaService;
        IUserService _userService;
        IWebHostEnvironment _webHostEnvironment;

        public MediaController(IMediaService mediaService, IWebHostEnvironment webHostEnvironment,IUserService userService)
        {
            _mediaService = mediaService;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetFirstImages()
        {
            var listImages = _mediaService.GetFirstMedias();

            return PartialView("_imageGallery", listImages);
        }

        public ActionResult GetMoreImages(int page)
        {
            var listImages = _mediaService.GetMoreMedias(page);

            return PartialView("_imageGallery", listImages);
        }

        public void UploadImageDropzone()
        {

            var files = HttpContext.Request.Form.Files;

            foreach (var file in files)
            {
                if (file.Length > 0)
                {

                    string FileName = file.FileName;
                    string Extention = Path.GetExtension(FileName);

                    string sName = Path.GetFileNameWithoutExtension(FileName).Replace("(", "").Replace(")", "");
                    bool isNameExist = false;
                    while (!isNameExist)
                    {
                        bool isName = _mediaService.IsNameExist(sName);
                        if (isName)
                        {
                            sName = sName + "_1";
                            isNameExist = false;
                        }
                        else
                        {
                            isNameExist = true;
                        }
                    }


                    DateTime dateNow = DateTime.Now;
                    string year = dateNow.Year.ToString();
                    string Month = dateNow.Month.ToString().PadLeft(2, '0');


                    string TargetPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/" + year + "/" + Month + "/", sName + Extention);

                    string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "Images/" + year + "/" + Month + "/");
                    Directory.CreateDirectory(TargetLocation);
                    using (var stream = new FileStream(TargetPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    string fileType = "File";
                    if (file.ContentType.Contains("video"))
                    {
                        fileType = "Video";
                    }
                    else if (file.ContentType.Contains("audio"))
                    {
                        fileType = "Audio";
                    }
                    else if (file.ContentType.Contains("image"))
                    {
                        fileType = "Photo";
                    }

                    _mediaService.SaveMediaInDatabase(sName,
                        "images/" + year + "/" + Month + "/" + sName + Extention, fileType);
                }
            }
        }

        public string UploadFile(IFormFile aUploadedFile)
        {
            string FileName = aUploadedFile.FileName;
            string Extention = Path.GetExtension(FileName);
            string sName = Path.GetFileNameWithoutExtension(FileName).Replace("(", "").Replace(")", "");

            bool isNameExist = false;
            while (!isNameExist)
            {
                bool isName = _mediaService.IsNameExist(sName);
                if (isName)
                {
                    sName = sName + "_1";
                    isNameExist = false;
                }
                else
                {
                    isNameExist = true;
                }
            }


            DateTime dateNow = DateTime.Now;
            string year = dateNow.Year.ToString();
            string Month = dateNow.Month.ToString().PadLeft(2, '0');


            string TargetPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/" + year + "/" + Month + "/", sName + Extention);

            string TargetLocation = Path.Combine(_webHostEnvironment.WebRootPath, "Images/" + year + "/" + Month + "/");

            Directory.CreateDirectory(TargetLocation);
            using (var stream = new FileStream(TargetPath, FileMode.Create))
            {
                aUploadedFile.CopyTo(stream);
            }


            string fileType = "File";
            if (aUploadedFile.ContentType.Contains("video"))
            {
                fileType = "Video";
            }
            else if (aUploadedFile.ContentType.Contains("audio"))
            {
                fileType = "Audio";
            }
            else if (aUploadedFile.ContentType.Contains("image"))
            {
                fileType = "Photo";
            }

            _mediaService.SaveMediaInDatabase(sName,
                "images/" + year + "/" + Month + "/" + sName + Extention, fileType);


            return "/images/" + year + "/" + Month + "/" + sName + Extention;

        }

        public IActionResult Sliders(string Type)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            ViewBag.Type = Type;
            return View(_mediaService.GetAllSliders(Type));
        }

        public IActionResult GetSliders(int pageNumber, int countPerPage, int allpage, string Type)
        {

            SliderViewModel sliderTable = new SliderViewModel
            {
                Sliders = _mediaService.GetSpecificSliders(pageNumber, countPerPage,Type),
                CurrentPage = pageNumber,
                AllPage = allpage,
                CountPerPage = countPerPage,
                Type = Type,
            };
            return PartialView("_sliderTable", sliderTable);
        }

        public IActionResult PublishSlider(string Type)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {

                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }

            return View(new Slider() { Type = Type });
        }

        [HttpPost]
        public IActionResult PublishSlider(string Title, string Link, string thumbUrl,string Type, string mobileThumbUrl)
        {
            bool isHaveError = false;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "تیتر خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(Link))
                {
                    ModelState.AddModelError("", "لینک اسلایدر خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(thumbUrl))
                {
                    ModelState.AddModelError("", "تصویر شاخص دسکتاپ انتخاب کنید");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(mobileThumbUrl))
                {
                    ModelState.AddModelError("", "تصویر شاخص موبایل انتخاب کنید");
                    isHaveError = true;
                }

                if (!isHaveError)
                {
                    Slider p = new Slider();
                    p.Title = Title;
                    p.Link = Link;
                    p.Type = Type;

                    if (!string.IsNullOrEmpty(thumbUrl))
                    {
                        p.MediaId = int.Parse(thumbUrl.Replace("image_", string.Empty));
                    }
                    if (!string.IsNullOrEmpty(mobileThumbUrl))
                    {
                        p.MobileMediaId = int.Parse(mobileThumbUrl.Replace("image_", string.Empty));
                    }

                    _mediaService.SaveSlider(p);

                    TempData["Message"] = "اسلایدر با موفقیت منتشر شد";


                    return RedirectToAction("EditSlider", "Media", new { id = p.Id });
                }

                else
                {
                    ViewBag.Thumb = thumbUrl;
                    ViewBag.MobileThumb = mobileThumbUrl;

                    return View(new Slider()
                    {
                        Title = Title,
                        Link = Link,
                        Type = Type,
                    });
                }


            }
            return View();
        }

        public IActionResult EditSlider(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role == 5)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            var slider = _mediaService.GetSlider(id);


            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();


            return View(slider);
        }

        [HttpPost]
        public IActionResult EditSlider(int SliderId, string Title, string Link, string thumbUrl, string mobileThumbUrl)
        {
            bool isHaveError = false;
            Slider p = _mediaService.GetSlider(SliderId);
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    ModelState.AddModelError("", "تیتر خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(Link))
                {
                    ModelState.AddModelError("", "لینک اسلایدر خالی است");
                    isHaveError = true;
                }
                if (string.IsNullOrEmpty(thumbUrl))
                {
                    ModelState.AddModelError("", "تصویر شاخص دسکتاپ انتخاب کنید");
                    isHaveError = true;
                }

                if (!isHaveError)
                {
                    p.Title = Title;
                    p.Link = Link;

                    p.MediaId = int.Parse(thumbUrl.Replace("image_", string.Empty));
                    p.MobileMediaId = int.Parse(mobileThumbUrl.Replace("image_", string.Empty));

                    _mediaService.SaveDatabase();

                    TempData["Message"] = "اسلایدر با موفقیت ویرایش شد";


                    return RedirectToAction("EditSlider", "Media", new { id = p.Id });
                }

                else
                {
                    ViewBag.Thumb = thumbUrl;
                    ViewBag.MobileThumb = mobileThumbUrl;
                    return View(new Slider()
                    {
                        Id = p.Id,
                        Title = Title,
                        Link = Link,
                    });
                }


            }
            return View();
        }

        public void DeleteSlider(int id)
        {
            _mediaService.DeleteSlider(id);
        }

        public bool DeleteMedia(int id)
        {
            return _mediaService.DeleteMedia(id);
        }

        public bool DeleteManyImages(int[] AllImageId)
        {
           return _mediaService.DeleteManyImages(AllImageId);
        }
    }
}
