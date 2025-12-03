namespace MaktabGram.Domain.Core.FollowerAgg.Contracts
{
    public interface IFollowerRepository
    {
        Task Follow(int userId, int followedId, CancellationToken cancellationToken);
        Task UnFollow(int userId, int followedId, CancellationToken cancellationToken);
    }

}
