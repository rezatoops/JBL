using Core.Services;
using Core.Services.Interfaces;
using Core.ViewModel;
using Datalayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PandS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CommentController : Controller
    {
        IUserService _userService;
        ICommentService _commentService;

        public CommentController(IUserService userService, ICommentService commentService)
        {
            _userService = userService;
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            if (_userService.isExistUser(User.Identity.Name).Role != 1)
            {
                return RedirectToAction("NotAccess", "User", new { area = "" });
            }
            return View(_commentService.GetAllComments());
        }

        public IActionResult GetComments(int pageNumber, int countPerPage, string search)
        {
            List<Comment> AllComments = new List<Comment>();
            if (string.IsNullOrEmpty(search))
            {
                AllComments = _commentService.GetAllComments();
            }
            else
            {
                AllComments = _commentService.GetAllComments(search);
            }

            int allpageNumber = (AllComments.Count() % countPerPage == 0) ? (AllComments.Count() / countPerPage) : (AllComments.Count() / countPerPage) + 1;

            CommentViewModel commentTable = new CommentViewModel
            {
                Comments = AllComments.Skip(countPerPage * (pageNumber - 1)).Take(countPerPage).ToList(),
                CurrentPage = pageNumber,
                AllPage = allpageNumber,
                CountPerPage = countPerPage,
                SearchTerm = search,

            };

            return PartialView("_commentTable", commentTable);
        }

        public void DoUnreadAllComments()
        {
            _commentService.DoUnreadAllComment();
        }

        public void DeleteComment(int id)
        {
            _commentService.DeleteComment(id);
        }

        public IActionResult ShowReplyView(int commentid,int productId)
        {
            ViewBag.commentId = commentid;
            ViewBag.productId = productId;
         
            return PartialView("_commentReplyView");
        }

        public IActionResult ReplyComment(int CommentId, int ProductId,string commentText)
        {
            var user = _userService.isExistUser(User.Identity.Name);

            var comment = _commentService.ReplyComment(CommentId, ProductId, commentText,user.FirstName + " " + user.LastName);

            return PartialView("_commentRow", comment);
        }
    }
}
