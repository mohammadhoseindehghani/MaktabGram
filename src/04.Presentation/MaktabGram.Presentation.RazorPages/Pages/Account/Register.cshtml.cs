using Hangfire;
using MaktabGram.Domain.Core.UserAgg.Contracts.User;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Presentation.RazorPages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace MaktabGram.Presentation.RazorPages.Pages.Account
{

    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
        public int Otp { get; set; }
    }
    public class RegisterModel(IUserApplicationService userApplicationService,ILogger<RegisterModel> logger) : PageModel
    {
        [BindProperty]
        public RegisterViewModel Model { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {

            //throw new Exception("exception from RegisterModel");
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
                Otp = Model.Otp,
            };

            var registerResult = await userApplicationService.Register(userModel, cancellationToken);

            BackgroundJob.Enqueue<IMyServices>(x => x.Log());



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

        public async Task<IActionResult> OnGetSendOtp(string mobile,CancellationToken cancellationToken)
        { 
           await userApplicationService.SendRegisterOtp(mobile, cancellationToken);
            return Page();
        }
    }

}
