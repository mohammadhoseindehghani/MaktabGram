using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaktabGram.Domain.Core.UserAgg.Contracts
{
    public interface IUserApplicationService
    {
        Task<Result<UserLoginOutputDto>> Login(string mobile, string password, CancellationToken cancellationToken);
        Task<Result<bool>> Register(RegisterUserInputDto model, CancellationToken cancellationToken);
        Task<List<GetUserSummaryDto>> GetUsersSummary(CancellationToken cancellationToken);
        Task<UpdateGetUserDto> GetUpdateUserDetails(int userId, CancellationToken cancellationToken);
        Task Active(int userId, CancellationToken cancellationToken);
        Task DeActive(int userId, CancellationToken cancellationToken);
        Task<Result<bool>> Update(int userId, UpdateGetUserDto model, CancellationToken cancellationToken);
        Task<GetUserProfileDto> GetProfile(int searchedUserId, int curentUserId, CancellationToken cancellationToken);
        Task<List<SearchResultDto>> Search(string username, int userId, CancellationToken cancellationToken);
        Task<string> GetImageProfileUrl(int userId, CancellationToken cancellationToken);
    }

}
