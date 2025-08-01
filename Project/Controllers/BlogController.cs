using Core.Services;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PandS.Controllers
{
    public class BlogController : Controller
    {
        IPostService _postService;
        IPageService _pageService;

        public BlogController(IPostService postService, IPageService pageService)
        {
            _postService = postService;
            _pageService = pageService;
        }

        [Route("Blog/{page}")]
        [Route("Blog")]
        public IActionResult Blog(int page = 1)
        {
            ViewBag.CurrentPage = page;
            int PostCountPerPage = 10;
            var AllPost = _postService.GetAllPosts();
            ViewBag.AllNumber = (AllPost.Count() % PostCountPerPage == 0) ? (AllPost.Count() / PostCountPerPage) : (AllPost.Count() / PostCountPerPage) + 1;

            var posts = AllPost.OrderByDescending(p => p.Id).Skip((page - 1) * PostCountPerPage).Take(PostCountPerPage).ToList();

            return View(posts);
        }

        [Route("/Article/{slug}")]
        public IActionResult Article(string slug)
        {
            var post = _postService.GetPostBySlug(slug);
            return View(post);
        }

        [Route("/Page/{slug}")]
        public IActionResult Page(string slug)
        {
            var page = _pageService.GetPage(slug);
            return View(page);
        }
    }
}
