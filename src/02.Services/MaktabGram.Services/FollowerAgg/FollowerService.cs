using MaktabGram.Domain.Core.FollowerAgg.Contracts;
using MaktabGram.Infrastructure.EfCore.Repositories.FollowerAgg;

namespace MaktabGram.Domain.Services.FollowerAgg
{
    public class FollowerService(IFollowerRepository followerRepository) : IFollowerService
    {
        public async Task Follow(int userId, int followedId, CancellationToken cancellationToken)
        {
            await followerRepository.Follow(userId, followedId, cancellationToken);
        }

        public async Task UnFollow(int userId, int followedId, CancellationToken cancellationToken)
        {
            await followerRepository.UnFollow(userId, followedId, cancellationToken);
        }
    }

}
