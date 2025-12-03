using MaktabGram.Domain.ApplicationServices.PostAgg;
using MaktabGram.Domain.Core.PostAgg.Contracts;
using MaktabGram.Domain.Core.PostAgg.Dtos;
using MaktabGram.Presentation.RazorPages.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaktabGram.Presentation.RazorPages.Pages.Posts
{
    public class IndexModel(IPostApplicationService postApplicationService) : BasePageModel
    {
        public List<GetPostForFeedsDto> Feeds { get; set; }
        public int PostCount { get; set; }

        [BindProperty]
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }

        public async Task OnGetAsync(int nextPage, int id = 1, CancellationToken cancellationToken = default)
        {
            Page = nextPage != 0 ? nextPage : id;
            PageSize = 2;

            Feeds = await postApplicationService.GetFeedPosts(GetUserId(), Page, PageSize, cancellationToken);
            PostCount = await postApplicationService.GetPostCount(GetUserId(), cancellationToken);

            TotalPage = (int)Math.Ceiling((double)PostCount / PageSize);
        }

        public async Task<IActionResult> OnGetLikeAsync(int id, CancellationToken cancellationToken = default)
        {
            await postApplicationService.Like(GetUserId(), id, cancellationToken);
            return RedirectToPage("/Posts/Feeds");
        }

        public async Task<IActionResult> OnGetDisLikeAsync(int id, CancellationToken cancellationToken = default)
        {
            await postApplicationService.DisLike(GetUserId(), id, cancellationToken);
            return RedirectToPage("/Posts/Feeds");
        }
    }

}
