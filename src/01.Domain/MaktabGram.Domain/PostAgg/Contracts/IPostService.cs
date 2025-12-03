using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.PostAgg.Dtos;
using MaktabGram.Domain.Core.UserAgg.Dtos;

namespace MaktabGram.Domain.Core.PostAgg.Contracts
{
    public interface IPostService
    {
        Task<Result<bool>> Create(CreatePostInputDto model, CancellationToken cancellationToken);
        Task<List<GetPostForFeedsDto>> GetFeedPosts(int userId, int page, int pageSize, CancellationToken cancellationToken);
        Task<List<int>> SetUserTags(string postTags, CancellationToken cancellationToken);
        Task<int> GetPostCount(int userId, CancellationToken cancellationToken);
        Task Like(int userId, int postId, CancellationToken cancellationToken);
        Task<bool> UserLikePost(int userId, int postId, CancellationToken cancellationToken);
        Task DisLike(int userId, int postId, CancellationToken cancellationToken);
        Task<GetPostDetailsDto?> GetPostDetails(int postId, CancellationToken cancellationToken);
    }

}
