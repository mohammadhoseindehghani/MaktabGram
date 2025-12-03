using MaktabGram.Domain.ApplicationServices.FollowAgg;
using MaktabGram.Domain.ApplicationServices.UserAgg;
using MaktabGram.Domain.Core.FollowerAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Domain.Core.UserAgg.Entities;
using MaktabGram.Presentation.RazorPages.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaktabGram.Presentation.RazorPages.Pages.Account
{
    public class ProfileModel(IUserApplicationService userApplicationService,
        IFollowerApplicationService followerApplicationService) : BasePageModel
    {
        public GetUserProfileDto Profile { get; set; }

        public async Task OnGetAsync(int? userId, CancellationToken cancellationToken)
        {
            Profile = await userApplicationService.GetProfile((int)userId, GetUserId(), cancellationToken);
        }

        public async Task<IActionResult> OnGetFollowAsync(int id, CancellationToken cancellationToken)
        {
            await followerApplicationService.Follow(GetUserId(), id, cancellationToken);
            return RedirectToPage("/Account/Profile", new { userId = id });
        }

        public async Task<IActionResult> OnGetUnFollowAsync(int id, CancellationToken cancellationToken)
        {
            await followerApplicationService.UnFollow(GetUserId(), id, cancellationToken);
            return RedirectToPage("/Account/Profile", new { userId = id });
        }
    }

}
