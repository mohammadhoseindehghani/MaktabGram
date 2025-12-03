using Azure;
using MaktabGram.Domain.Core.PostAgg.Contracts;
using MaktabGram.Domain.Core.PostAgg.Dtos;
using MaktabGram.Domain.Core.PostAgg.Entities;
using MaktabGram.Framework;
using MaktabGram.Infrastructure.EfCore.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace MaktabGram.Infrastructure.EfCore.Repositories.PostAgg
{
    public class PostRepository(AppDbContext appDbContext) : IPostRepository
    {
        public async Task<GetPostDetailsDto?> GetPostDetails(int postId, CancellationToken cancellationToken)
        {
            var post = await appDbContext.Posts
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Where(x => x.Id == postId)
                .Select(p => new GetPostDetailsDto
                {
                    Id = p.Id,
                    Caption = p.Caption,
                    ImgPostUrl = p.ImageUrl,
                    LikeCount = p.PostLikes.Count,
                    Username = p.User.Username,
                    CreateAt = p.CreatedAt.ToPersianString("dddd, dd MMMM,yyyy"),
                    ProfileImgUrl = p.User.Profile.ProfileImageUrl,
                    Comments = p.Comments,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return post;
        }

        public async Task<List<GetPostForFeedsDto>> GetFeedPosts(int userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var posts = await appDbContext.Posts
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Where(x => x.User.Followers.Any(x => x.FollowerId == userId))
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Select(p => new GetPostForFeedsDto
                {
                    Id = p.Id,
                    Caption = p.Caption,
                    ImgPostUrl = p.ImageUrl,
                    LikeCount = p.PostLikes.Count,
                    Username = p.User.Username,
                    CreateAt = p.CreatedAt.ToPersianString("dddd, dd MMMM,yyyy"),
                    ProfileImgUrl = p.User.Profile.ProfileImageUrl,
                    Comments = p.Comments,
                    UserLikeThisPost = p.PostLikes.Any(x => x.UserId == userId),
                })
                .ToListAsync(cancellationToken);

            return posts;
        }

        public async Task<int> Create(CreatePostInputDto model, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Caption = model.Caption,
                ImageUrl = model.ImgUrl!,
                OpenComment = model.ShowComment,
                UserId = model.UserId,
                CreatedAt = DateTime.Now,
                TaggedUsers = model.TaggedUsers.Select(x => new PostTag { TaggedUserId = x }).ToList(),
            };

            await appDbContext.Posts.AddAsync(post, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return post.Id;
        }

        public async Task<int> GetPostCount(int userId, CancellationToken cancellationToken)
        {
            return await appDbContext.Posts
                .CountAsync(x => x.User.Followers.Any(x => x.FollowerId == userId), cancellationToken);
        }

        public async Task Like(int userId, int postId, CancellationToken cancellationToken)
        {
            var postLike = new PostLike
            {
                LikedAt = DateTime.Now,
                PostId = postId,
                UserId = userId
            };

            await appDbContext.PostLikes.AddAsync(postLike, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UserLikePost(int userId, int postId, CancellationToken cancellationToken)
        {
            return await appDbContext.PostLikes
                .AnyAsync(x => x.UserId == userId && x.PostId == postId, cancellationToken);
        }

        public async Task DisLike(int userId, int postId, CancellationToken cancellationToken)
        {
            await appDbContext.PostLikes
                .Where(x => x.UserId == userId && x.PostId == postId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }

}
