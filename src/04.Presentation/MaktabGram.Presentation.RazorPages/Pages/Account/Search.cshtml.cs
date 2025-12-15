using MaktabGram.Domain.ApplicationServices.FollowAgg;
using MaktabGram.Domain.ApplicationServices.UserAgg;
using MaktabGram.Domain.Core.FollowerAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Contracts.User;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Domain.Core.UserAgg.Entities;
using MaktabGram.Presentation.RazorPages.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaktabGram.Presentation.RazorPages.Pages.Account
{
    public class SearchModel(IUserApplicationService userApplicationService,
        IFollowerApplicationService followerApplicationService) : BasePageModel
    {
        public List<SearchResultDto> SearchResult { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            SearchResult = await userApplicationService.Search(string.Empty, (int)GetUserId()!, cancellationToken);
        }

        [HttpPost]
        public async Task OnPostAsync(string username, CancellationToken cancellationToken)
        {
            SearchResult = await userApplicationService.Search(username, (int)GetUserId()!, cancellationToken);
        }

        public async Task<IActionResult> OnGetFollowAsync(int id, CancellationToken cancellationToken)
        {
            await followerApplicationService.Follow((int)GetUserId()!, id, cancellationToken);
            return RedirectToPage("/Account/Search");
        }

        public async Task<IActionResult> OnGetUnFollowAsync(int id, CancellationToken cancellationToken)
        {
            await followerApplicationService.UnFollow((int)GetUserId()!, id, cancellationToken);
            return RedirectToPage("/Account/Search");
        }
    }

}
