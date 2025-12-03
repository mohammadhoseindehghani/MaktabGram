using MaktabGram.Framework;
using MaktabGram.Domain.Services.UserAgg;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Infrastructure.FileService.Services;
using MaktabGram.Infrastructure.FileService.Contracts;


namespace MaktabGram.Domain.ApplicationServices.UserAgg
{
    public class UserApplicationService(IUserService userService, IFileService fileService) : IUserApplicationService
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
            var login = await userService.Login(mobile, password.ToMd5Hex(), cancellationToken);

            if (login is not null)
            {
                var isActive = await userService.IsActive(mobile, cancellationToken);

                return isActive
                    ? Result<UserLoginOutputDto>.Success("لاگین با موفقیت انجام شد.", login)
                    : Result<UserLoginOutputDto>.Failure("کاربر با این شماره فعال نمی‌باشد.");
            }
            else
            {
                return Result<UserLoginOutputDto>.Failure("نام کاربری یا کلمه عبور اشتباه می باشد.");
            }
        }

        public async Task<Result<bool>> Register(RegisterUserInputDto model, CancellationToken cancellationToken)
        {
            return await userService.Register(model, cancellationToken);
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
    }

}
