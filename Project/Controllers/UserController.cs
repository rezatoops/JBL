using Datalayer.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Services.Interfaces;

namespace dastgire.Controllers
{
    public class UserController : Controller
    {
        IUserService _userService;
        IToolsService _toolsService;
        public UserController(IUserService userService, IToolsService toolsService)
        {
            _userService = userService;
            _toolsService = toolsService;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return View();
        }
        public IActionResult LoginWithPassword(string phoneNumber, string Password)
        {
            var isCorrect = _userService.CheckLoginWithPassword(phoneNumber, Password);
            var user = _userService.isExistUser(phoneNumber);

            if (isCorrect)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, phoneNumber),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                user.ActiveCode = null;
                _userService.UpdateUser(user);


                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                //if (Url.IsLocalUrl(returnUrl))
                //    return Redirect(returnUrl);
                //else
                //    return RedirectToAction("Index", "Home");
                //, new { area = "Admin" }

                var res = new { Result = "Success"/*, ReturnUrl = returnUrl*/ };
                return Json(res);
            }
            else
            {
                ModelState.AddModelError("", "پسورد اشتباه است");
                return PartialView("_LoginWithPassword", phoneNumber);
            }

        }

        public IActionResult ShowLoginOrRegister(string phoneNumber,bool MustSendSms = true)
        {
            bool isHaveError = false;


            if (string.IsNullOrEmpty(phoneNumber))
            {
                ModelState.AddModelError("", "باید یک شماره موبایل وارد کنید");
                isHaveError = true;
            }
            else if (phoneNumber.Length > 0 && phoneNumber.Length < 11)
            {
                ModelState.AddModelError("", "شماره موبایل را به صورت صحیح وارد کنید");
                isHaveError = true;
            }


            if (isHaveError)
            {
                return PartialView("_loginModalForm");
            }
            else
            {
                var EnglishPhoneNumber = _toolsService.toEnglishNumber(phoneNumber);
                var user = _userService.isExistUser(EnglishPhoneNumber);

                Random generator = new Random();
                string random = generator.Next(100000, 1000000).ToString();
                if (user != null)
                {
                    if (user.IsActive == true)
                    {
                        ViewBag.MustSendSms = MustSendSms;
                        return PartialView("_LoginWithPassword", EnglishPhoneNumber);
                    }
                    else
                    {
                        user.ActiveCode = random;
                        _userService.UpdateUser(user);

                        _toolsService.SendCodeWithSMS(random, EnglishPhoneNumber);

                        return PartialView("_RegisterForm", EnglishPhoneNumber);
                    }

                }
                else
                {
                    User newUser = new User
                    {
                        ActiveCode = random,
                        PhoneNumber = EnglishPhoneNumber,
                        Role = 5,
                        RegisterDate = DateTime.Now,
                    };

                    _userService.AddUser(newUser);
                    _toolsService.SendCodeWithSMS(random, EnglishPhoneNumber);

                    return PartialView("_RegisterForm", EnglishPhoneNumber);

                }
            }
        }

        public IActionResult Register(string phoneNumber, string ActiveCode, string Password)
        {
            bool isHaveError = false;
            var user = _userService.isExistUser(phoneNumber);
            if (string.IsNullOrEmpty(ActiveCode))
            {
                ModelState.AddModelError("", "لطفا کد فعالسازی را وارد کنید");
                isHaveError = true;
            }
            if (string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("", "وارد کردن رمز عبور اجباری است");
                isHaveError = true;
            }

            if (!isHaveError)
            {
                var EnglishActiveCode = _toolsService.toEnglishNumber(ActiveCode);
                //var isVerifyCode = _userService.CheckIdentificitaionCode(IdenCode);
                if (user.ActiveCode != EnglishActiveCode)
                {
                    ModelState.AddModelError("", "کد فعالسازی اشتباه است");
                    return PartialView("_RegisterForm", phoneNumber);

                }
                else
                {
                    user.ActiveCode = null;
                    user.Password = Password;
                    user.IsActive = true;

                    _userService.UpdateUser(user);
                    //_userService.UseIdentificationCode(IdenCode);

                    var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, phoneNumber),
                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                    var res = new { Result = "Success"/*, ReturnUrl = returnUrl*/ };
                    return Json(res);
                }
            }
            else
            {
                return PartialView("_RegisterForm", phoneNumber);
            }
        }

        public IActionResult ShowLoginWithCode(string phoneNumber, bool isAgain = false,bool MustSendSms = true)
        {
            var EnglishPhoneNumber = _toolsService.toEnglishNumber(phoneNumber);
            var user = _userService.isExistUser(EnglishPhoneNumber);



            if(MustSendSms)
            {
                Random generator = new Random();
                string random = generator.Next(100000, 1000000).ToString();

                user.ActiveCode = random;
                _userService.UpdateUser(user);

                _toolsService.SendCodeWithSMS(random, EnglishPhoneNumber);
                isAgain = true;
                ViewBag.MustSendSms = false;
            }

            if (isAgain){
                ViewBag.minute = 1;
                ViewBag.second = 59;
            }
            else
            {
                ViewBag.minute = -1;
                ViewBag.second = -1;
            }

            return PartialView("_LoginWithCode", EnglishPhoneNumber);
        }

        public IActionResult LoginWithCode(string phoneNumber, string ActiveCode,int minutes, int seconds)
        {
            var user = _userService.isExistUser(phoneNumber);
            var EnglishActiveCode = _toolsService.toEnglishNumber(ActiveCode);


            if (user.ActiveCode == EnglishActiveCode)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, phoneNumber),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                user.ActiveCode = null;
                _userService.UpdateUser(user);


                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

                var res = new { Result = "Success" };
                return Json(res);
            }
            else
            {
                ModelState.AddModelError("", "کد ورود اشتباه است");
                ViewBag.minute = minutes;
                ViewBag.second = seconds;
                return PartialView("_LoginWithCode", phoneNumber);
            }
        }

        public IActionResult ShowLoginForm()
        {
            return PartialView("_loginModalForm");
        }

        [Route("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
