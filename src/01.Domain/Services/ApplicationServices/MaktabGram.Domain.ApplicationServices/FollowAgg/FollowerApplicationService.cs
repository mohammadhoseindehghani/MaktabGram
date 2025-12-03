using MaktabGram.Domain.Core.FollowerAgg.Contracts;
using MaktabGram.Domain.Services.FollowerAgg;

namespace MaktabGram.Domain.ApplicationServices.FollowAgg
{
    public class FollowerApplicationService (IFollowerService followerService) : IFollowerApplicationService
    {
        public async Task Follow(int userId, int FollowedId,CancellationToken cancellationToken)
        {
            await followerService.Follow(userId, FollowedId, cancellationToken);
        }

        public async Task UnFollow(int userId, int FollowedId , CancellationToken cancellationToken)
        {
            await followerService.UnFollow(userId, FollowedId,cancellationToken);
        }
    }
}
