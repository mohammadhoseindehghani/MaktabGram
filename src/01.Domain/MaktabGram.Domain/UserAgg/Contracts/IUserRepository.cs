using MaktabGram.Domain.Core.UserAgg.Dtos;

namespace MaktabGram.Domain.Core.UserAgg.Contracts
{
    public interface IUserRepository
    {
        public Task<UserLoginOutputDto?> Login(string mobile, string password,CancellationToken cancellationToken);
        public Task<bool> Register(RegisterUserInputDto model, CancellationToken cancellationToken);
        public Task<bool> IsActive(string mobile, CancellationToken cancellationToken);
        public Task<bool> MobileExists(string mobile, CancellationToken cancellationToken);
        public Task<List<GetUserSummaryDto>> GetUsersSummary(CancellationToken cancellationToken);
        public Task<UpdateGetUserDto> GetUpdateUserDetails(int userId, CancellationToken cancellationToken);
        public Task Active(int userId, CancellationToken cancellationToken);
        public Task DeActive(int userId, CancellationToken cancellationToken);
        public Task<bool> Update(int userId, UpdateGetUserDto model,CancellationToken cancellationToken);
        public Task<string> GetImageProfileUrl(int userId, CancellationToken cancellationToken);
        public Task<List<int>> GetUserIdsBy(List<string> userNames, CancellationToken cancellationToken);
        public Task<GetUserProfileDto> GetProfile(int searchedUserId, CancellationToken cancellationToken);
        public Task<List<SearchResultDto>> Search(string username,int userId,CancellationToken cancellationToken);
        public Task<GetUserProfileDto> GetProfileWithPosts(int searchedUserId, int curentUserId, CancellationToken cancellationToken);
        public Task<bool> IsFolllow(int searchedUserId, int curentUserId, CancellationToken cancellationToken);
    }
}
