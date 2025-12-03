using MaktabGram.Domain.Core.PostAgg.Dtos;

namespace MaktabGram.Domain.Core.PostAgg.Contracts
{
    public interface IPostRepository
    {
        Task<int> Create(CreatePostInputDto model, CancellationToken cancellationToken);
        Task<List<GetPostForFeedsDto>> GetFeedPosts(int userId, int page, int pageSize, CancellationToken cancellationToken);
        Task<int> GetPostCount(int userId, CancellationToken cancellationToken);
        Task Like(int userId, int postId, CancellationToken cancellationToken);
        Task<bool> UserLikePost(int userId, int postId, CancellationToken cancellationToken);
        Task DisLike(int userId, int postId, CancellationToken cancellationToken);
        Task<GetPostDetailsDto?> GetPostDetails(int postId, CancellationToken cancellationToken);
    }

}
