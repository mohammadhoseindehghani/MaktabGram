using MaktabGram.Framework;
using MaktabGram.Domain.Services.UserAgg;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Infrastructure.FileService.Services;
using MaktabGram.Infrastructure.FileService.Contracts;
using MaktabGram.Infrastructure.Notifications.Services;
using MaktabGram.Domain.Core.UserAgg.Contracts.User;
using MaktabGram.Domain.Core.UserAgg.Contracts.Otp;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;


namespace MaktabGram.Domain.ApplicationServices.UserAgg
{
    public class UserApplicationService(IUserService userService,
        IFileService fileService,
        ISmsService smsService,
        IOtpService otpService,
        SignInManager<IdentityUser<int>> signInManager,
        UserManager<IdentityUser<int>> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IHttpContextAccessor httpContextAccessor // <-- add this
        ) : IUserApplicationService
    {


        public async Task Active(int userId, CancellationToken cancellationToken)
        {
            await userService.Active(userId, cancellationToken);
        }

        public async Task DeActive(int userId, CancellationToken cancellationToken)
        {
            await userService.DeActive(userId, cancellationToken);
        }

        public async Task<UpdateGetUserDto> GetUpdateUserDetails(int userId, CancellationToken cancellationToken)
        {
            return await userService.GetUpdateUserDetails(userId, cancellationToken);
        }

        public async Task<List<GetUserSummaryDto>> GetUsersSummary(CancellationToken cancellationToken)
        {
            return await userService.GetUsersSummary(cancellationToken);
        }

        public async Task<Result<UserLoginOutputDto>> Login(string mobile, string password, CancellationToken cancellationToken)
        {
            var login = await signInManager.PasswordSignInAsync(mobile, password, false, false);

            if (login.Succeeded)
            {
                var isActive = await userService.IsActive(mobile, cancellationToken);

                var user = await userManager.FindByNameAsync(mobile);
                if (user != null)
                {

                    var roles = await userManager.GetRolesAsync(user);

                    // Get the user's claims
                    var userClaims = await userManager.GetClaimsAsync(user);

                    // Add your custom claim
                    var claims = new List<Claim>(userClaims)
                    {
                        new Claim(ClaimTypes.Gender, "Man")
                    };

                    // Add identity claims
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName ?? ""));
                    claims.Add(new Claim(ClaimTypes.Email, user.Email ?? ""));

                    foreach(var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));   
                    }

                    var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in with custom claims
                    await httpContextAccessor.HttpContext.SignInAsync(
                        IdentityConstants.ApplicationScheme,
                        principal
                    );
                }

                return isActive
                    ? Result<UserLoginOutputDto>.Success("لاگین با موفقیت انجام شد.", new UserLoginOutputDto())
                    : Result<UserLoginOutputDto>.Failure("کاربر با این شماره فعال نمی‌باشد.");
            }
            else
            {
                return Result<UserLoginOutputDto>.Failure("نام کاربری یا کلمه عبور اشتباه می باشد.");
            }
        }

        public async Task<Result<bool>> Register(RegisterUserInputDto model, CancellationToken cancellationToken)
        {
            //var otpIsValid = await otpService.Verify(model.Mobile, model.Otp, Core.UserAgg.Enum.OtpTypeEnum.Register, cancellationToken);

            //if(!otpIsValid)
            //{
            //    return new Result<bool> { Data = false , Message = "کد یکبار مصرف صحیح نمی باشد" };
            //}

            //var result = await userService.Register(model, cancellationToken);

            var user = new IdentityUser<int>
            {
                UserName = model.Mobile,
                PhoneNumber = model.Mobile,
                Email = model.Email,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                //await smsService.Send(model.Mobile, "به سایت مکتب گرام خوش آمدید");
                //await otpService.SetUsed(model.Mobile, model.Otp, Core.UserAgg.Enum.OtpTypeEnum.Register, cancellationToken);

                await userManager.AddToRoleAsync(user, "User");

                model.IdentityUserId = user.Id;
                await userService.Register(model, cancellationToken);


                return new Result<bool> { Message = "ورود موفق"};
            }

            return new Result<bool> { Message = result.Errors.First().Description};
        }

        public async Task<Result<bool>> Update(int userId, UpdateGetUserDto model, CancellationToken cancellationToken)
        {
            return await userService.Update(userId, model, cancellationToken);
        }

        public async Task<List<SearchResultDto>> Search(string username, int userId, CancellationToken cancellationToken)
        {
            return await userService.Search(username, userId, cancellationToken);
        }

        public async Task<GetUserProfileDto> GetProfile(int searchedUserId, int curentUserId, CancellationToken cancellationToken)
        {
            return await userService.GetProfile(searchedUserId, curentUserId, cancellationToken);
        }

        public async Task<string> GetImageProfileUrl(int userId, CancellationToken cancellationToken)
        {
            return await userService.GetImageProfileUrl(userId, cancellationToken);
        }

        public async Task SendRegisterOtp(string mobile, CancellationToken cancellationToken)
        {
            var otp = await smsService.SendOTP(mobile);
            await otpService.Create(mobile, otp,Core.UserAgg.Enum.OtpTypeEnum.Register,cancellationToken);
        }
    }
}
