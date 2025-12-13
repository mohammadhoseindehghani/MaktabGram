using MaktabGram.Domain.Core.UserAgg.Contracts.User;
using MaktabGram.Presentation.RazorPages.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaktabGram.Presentation.RazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel(IUserApplicationService userApplicationService): BasePageModel
    {
        
        public IActionResult OnGet()
        {
            var userId = GetUserId();
            if(userId != null)
            {
                if (IsAdmin())
                {
                    return RedirectToPage("/User/Index");
                }
                else
                {
                    return RedirectToPage("/UnAuthorized");
                }
            }
            return Page();
        }
    }
}
