using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaktabGram.Presentation.RazorPages.Pages.Account
{

    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
    }
    public class RegisterModel(IUserApplicationService userApplicationService) : PageModel
    {
        [BindProperty]
        public RegisterViewModel Model { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            var userModel = new RegisterUserInputDto
            {
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                Mobile = Model.Mobile,
                Password = Model.Password,
                Username = Model.Username,
            };

            var registerResult = await userApplicationService.Register(userModel, cancellationToken);

            if (registerResult.IsSuccess)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                Message = registerResult.Message;
            }

            return Page();
        }
    }

}
