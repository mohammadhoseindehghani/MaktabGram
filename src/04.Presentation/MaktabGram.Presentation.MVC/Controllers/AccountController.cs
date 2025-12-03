using MaktabGram.Domain.ApplicationServices.FollowAgg;
using MaktabGram.Domain.ApplicationServices.UserAgg;
using MaktabGram.Domain.Core.FollowerAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Presentation.MVC.Database;
using MaktabGram.Presentation.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace MaktabGram.Presentation.MVC.Controllers
{
    public class AccountController(
        IUserApplicationService userApplicationService,
        IFollowerApplicationService followApplicationService)
        : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
        {
            var loginResult = await userApplicationService.Login(model.Mobile, model.Password, cancellationToken);

            if (loginResult.IsSuccess)
            {
                InMemoryDatabase.OnlineUser = new OnlineUser
                {
                    Id = loginResult.Data.Id,
                    IsAdmin = loginResult.Data.IsAdmin,
                    Username = loginResult.Data.Username
                };

                return loginResult.Data.IsAdmin
                    ? RedirectToAction("Index", "Admin")
                    : RedirectToAction("Index", "Post");
            }
            else
            {
                ViewBag.Error = loginResult.Message;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
        {
            var userModel = new RegisterUserInputDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Mobile = model.Mobile,
                Password = model.Password,
                Username = model.Username,
            };

            var registerResult = await userApplicationService.Register(userModel, cancellationToken);

            if (registerResult.IsSuccess)
            {
                // Registration succeeded, redirect or show message
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = registerResult.Message;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            var userId = 3; // InMemoryDatabase.OnlineUser.Id
            var profile = await userApplicationService.GetProfile(userId, 0, cancellationToken);

            if (profile is null)
            {
                return NotFound();
            }

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Search(CancellationToken cancellationToken)
        {
            var userId = 3;
            var results = await userApplicationService.Search(string.Empty, userId, cancellationToken);
            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string username, CancellationToken cancellationToken)
        {
            var userId = 3;
            var results = await userApplicationService.Search(username, userId, cancellationToken);
            return View(results);
        }

        public async Task<IActionResult> Follow(int id, CancellationToken cancellationToken)
        {
            var userId = 3;
            await followApplicationService.Follow(userId, id, cancellationToken);

            return RedirectToAction("Search");
        }

        public async Task<IActionResult> UnFollow(int id, CancellationToken cancellationToken)
        {
            var userId = 3;
            await followApplicationService.UnFollow(userId, id, cancellationToken);
            return RedirectToAction("Search");
        }
    }

}
