using MaktabGram.Domain.ApplicationServices.UserAgg;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Presentation.RazorPages.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaktabGram.Presentation.RazorPages.Pages.Account
{
    public class LoginViewModel
    {
        public string Mobile { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel(IUserApplicationService userApplicationService, ICookieService cookieService) : BasePageModel
    {
        [BindProperty]
        public LoginViewModel Model { get; set; }
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            if (UserIsLoggedIn())
            {
                return RedirectToPage("/Account/Profile");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            var loginResult = await userApplicationService.Login(Model.Mobile, Model.Password, cancellationToken);

            if (loginResult.IsSuccess)
            {
                cookieService.Set("Id", loginResult.Data.Id.ToString());
                cookieService.Set("IsAdmin", loginResult.Data.IsAdmin ? "1" : "0");
                cookieService.Set("Username", loginResult.Data.Username);

                return RedirectToPage("/Account/Profile");
            }
            else
            {
                Message = loginResult.Message;
            }

            return Page();
        }
    }

}
