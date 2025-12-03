using MaktabGram.Domain.Core.FollowerAgg.Contracts;
using MaktabGram.Domain.Core.FollowerAgg.Entities;
using MaktabGram.Infrastructure.EfCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaktabGram.Infrastructure.EfCore.Repositories.FollowerAgg
{
    public class FollowerRepository(AppDbContext dbContext) : IFollowerRepository
    {
        public async Task Follow(int userId, int followedId, CancellationToken cancellationToken)
        {
            var entity = new Follower
            {
                FollowerId = userId,
                FollowedId = followedId,
                FollowAt = DateTime.Now
            };

            await dbContext.Followers.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnFollow(int userId, int followedId, CancellationToken cancellationToken)
        {
            var follow = await dbContext.Followers
                .FirstOrDefaultAsync(f => f.FollowerId == userId && f.FollowedId == followedId, cancellationToken);

            if (follow is not null)
            {
                dbContext.Followers.Remove(follow);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

}
