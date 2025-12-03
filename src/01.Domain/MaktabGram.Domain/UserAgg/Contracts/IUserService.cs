using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.UserAgg.Dtos;

namespace MaktabGram.Domain.Core.UserAgg.Contracts
{
    public interface IUserService
    {
        Task<UserLoginOutputDto?> Login(string mobile, string password, CancellationToken cancellationToken);
        Task<Result<bool>> Register(RegisterUserInputDto model, CancellationToken cancellationToken);
        Task<bool> IsActive(string mobile, CancellationToken cancellationToken);
        Task<bool> MobileExists(string mobile, CancellationToken cancellationToken);
        Task<List<GetUserSummaryDto>> GetUsersSummary(CancellationToken cancellationToken);
        Task<UpdateGetUserDto> GetUpdateUserDetails(int userId, CancellationToken cancellationToken);
        Task Active(int userId, CancellationToken cancellationToken);
        Task DeActive(int userId, CancellationToken cancellationToken);
        Task<Result<bool>> Update(int userId, UpdateGetUserDto model, CancellationToken cancellationToken);
        Task<string> GetImageProfileUrl(int userId, CancellationToken cancellationToken);
        Task<List<int>> GetUserIdsBy(List<string> userNames, CancellationToken cancellationToken);
        Task<GetUserProfileDto> GetProfile(int searchedUserId, int curentUserId, CancellationToken cancellationToken);
        Task<List<SearchResultDto>> Search(string username, int userId, CancellationToken cancellationToken);
    }

}
