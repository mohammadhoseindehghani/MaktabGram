using MaktabGram.Framework;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Infrastructure.FileService.Contracts;

namespace MaktabGram.Domain.Services.UserAgg
{
    public class UserService(IUserRepository userRepository, IFileService fileService) : IUserService
    {
        public async Task<List<GetUserSummaryDto>> GetUsersSummary(CancellationToken cancellationToken)
        {
            var users = await userRepository.GetUsersSummary(cancellationToken);

            users.ForEach(user =>
                user.CreateAtFa = user.CreateAt.ToPersianString("yyyy/MM/dd"));

            return users;
        }

        public async Task Active(int userId, CancellationToken cancellationToken)
            => await userRepository.Active(userId, cancellationToken);

        public async Task DeActive(int userId, CancellationToken cancellationToken)
            => await userRepository.DeActive(userId, cancellationToken);

        public async Task<UserLoginOutputDto?> Login(string mobile, string password, CancellationToken cancellationToken)
        {
            return await userRepository.Login(mobile, password, cancellationToken);
        }

        public async Task<UpdateGetUserDto> GetUpdateUserDetails(int userId, CancellationToken cancellationToken)
            => await userRepository.GetUpdateUserDetails(userId, cancellationToken);

        public async Task<Result<bool>> Update(int userId, UpdateGetUserDto model, CancellationToken cancellationToken)
        {
            if (model.ImgProfile is not null)
            {
                var existingImageUrl = await userRepository.GetImageProfileUrl(userId, cancellationToken);
                await fileService.Delete(existingImageUrl, cancellationToken);
                model.ImageProfileUrl = await fileService.Upload(model.ImgProfile!, "Profiles", cancellationToken);
            }

            if (model.Password is not null)
            {
                model.Password = model.Password.ToMd5Hex();
            }

            var result = await userRepository.Update(userId, model, cancellationToken);

            return result
                ? Result<bool>.Success("اطلاعات کاربر با موفقیت به‌روزرسانی شد.")
                : Result<bool>.Failure("به‌روزرسانی اطلاعات کاربر با خطا مواجه شد.");
        }

        public async Task<bool> IsActive(string mobile, CancellationToken cancellationToken)
            => await userRepository.IsActive(mobile, cancellationToken);

        public async Task<bool> MobileExists(string mobile, CancellationToken cancellationToken)
            => await userRepository.MobileExists(mobile, cancellationToken);

        public async Task<string> GetImageProfileUrl(int userId, CancellationToken cancellationToken)
            => await userRepository.GetImageProfileUrl(userId, cancellationToken);

        public async Task<List<int>> GetUserIdsBy(List<string> userNames, CancellationToken cancellationToken)
            => await userRepository.GetUserIdsBy(userNames, cancellationToken);

        public async Task<Result<bool>> Register(RegisterUserInputDto model, CancellationToken cancellationToken)
        {
            var mobileExist = await MobileExists(model.Mobile, cancellationToken);
            if (mobileExist)
                return Result<bool>.Failure("کاربر با این شماره موجود می باشد.");

            if (model.ProfileImg is not null)
            {
                model.ProfileImageUrl = await fileService.Upload(model.ProfileImg, "Profiles", cancellationToken);
            }

            model.Password = model.Password.ToMd5Hex();

            await userRepository.Register(model, cancellationToken);

            return Result<bool>.Success("ثبت نام با موفقیت انجام شد.");
        }

        public async Task<GetUserProfileDto> GetProfile(int searchedUserId, int curentUserId, CancellationToken cancellationToken)
        {
            if (await userRepository.IsFolllow(searchedUserId, curentUserId, cancellationToken))
            {
                var user = await userRepository.GetProfileWithPosts(searchedUserId, curentUserId, cancellationToken);
                user.IsFollower = true;
                return user;
            }

            return await userRepository.GetProfile(searchedUserId, cancellationToken);
        }

        public async Task<List<SearchResultDto>> Search(string username, int userId, CancellationToken cancellationToken)
            => await userRepository.Search(username, userId, cancellationToken);
    }

}
