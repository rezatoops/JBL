using Core.Services.Interfaces;
using Core.ViewModel;
using Datalayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dastgire.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class UserController : Controller
    {

        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            return View(_userService.GetAllUsers());
        }

        public IActionResult GetUsers(int pageNumber, int countPerPage, string search)
        {

            List<User> AllUsers = new List<User>();
            if (string.IsNullOrEmpty(search))
            {
                AllUsers = _userService.GetAllUsers();
            }
            else
            {
                AllUsers = _userService.GetAllUsers(search);
            }

            int allpageNumber = (AllUsers.Count() % countPerPage == 0) ? (AllUsers.Count() / countPerPage) : (AllUsers.Count() / countPerPage) + 1;

            UserViewModel productTable = new UserViewModel
            {
                Users = AllUsers.OrderBy(u => u.Role).ThenBy(p => p.Id).Skip(countPerPage * (pageNumber - 1)).Take(countPerPage).ToList(),
                CurrentPage = pageNumber,
                AllPage = allpageNumber,
                CountPerPage = countPerPage,
                SearchTerm = search

            };

            return PartialView("_userTable", productTable);
        }

        public void DeleteUser(int id)
        {
            _userService.DeleteUser(id);
        }

        public IActionResult SetRole(int userId, int roleId)
        {
            var user = _userService.SetRole(userId, roleId);

            return PartialView("_userRow", user);
        }

        public IActionResult Edit(int id)
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "Home", new { area = "" });
            }
            var user = _userService.GetUserById(id);

            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(int userId,string FirstName, string LastName)
        {
            var user = _userService.GetUserById(userId);

            user.FirstName = FirstName;
            user.LastName = LastName;

            _userService.UpdateUser(user);

            TempData["Message"] = "عملیات با موفقیت انجام شد";

            return RedirectToAction("Edit", "User", new { id = user.Id });

        }
    }
}
