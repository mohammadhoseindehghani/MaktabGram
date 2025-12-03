using MaktabGram.Domain.ApplicationServices.PostAgg;
using MaktabGram.Domain.Core.PostAgg.Contracts;
using MaktabGram.Domain.Core.PostAgg.Dtos;
using MaktabGram.Domain.Services.PostAgg;
using MaktabGram.Presentation.MVC.Database;
using MaktabGram.Presentation.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace MaktabGram.Presentation.MVC.Controllers
{
    public class PostController(IPostApplicationService postApplicationService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userId = Database.InMemoryDatabase.OnlineUser.Id;

            var posts = await postApplicationService.GetFeedPosts(userId, 0, 0, cancellationToken);
            return View(posts);
        }

        [HttpGet]
        public IActionResult Create(CreatePostInputDto? model)
        {
            return View(model);
        }

        [HttpGet]
        public IActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostInputDto model, CancellationToken cancellationToken)
        {
            model.UserId = InMemoryDatabase.OnlineUser.Id;

            var result = await postApplicationService.Create(model, cancellationToken);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = result.Message;
                return View("Create", model);
            }
        }
    }

}
