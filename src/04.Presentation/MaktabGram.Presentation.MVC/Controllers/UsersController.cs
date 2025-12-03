using MaktabGram.Domain.ApplicationServices.UserAgg;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Domain.Services.UserAgg;
using Microsoft.AspNetCore.Mvc;

namespace MaktabGram.Presentation.MVC.Controllers
{
    public class UsersController(IUserApplicationService userApplicationService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var users = await userApplicationService.GetUsersSummary(cancellationToken);
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterUserInputDto model, CancellationToken cancellationToken)
        {
            var result = await userApplicationService.Register(model, cancellationToken);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = result.Message;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Active(int userId, CancellationToken cancellationToken)
        {
            await userApplicationService.Active(userId, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeActive(int userId, CancellationToken cancellationToken)
        {
            await userApplicationService.DeActive(userId, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int userId, CancellationToken cancellationToken)
        {
            var result = await userApplicationService.GetUpdateUserDetails(userId, cancellationToken);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateGetUserDto model, CancellationToken cancellationToken)
        {
            var result = await userApplicationService.Update(model.Id, model, cancellationToken);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }

}
