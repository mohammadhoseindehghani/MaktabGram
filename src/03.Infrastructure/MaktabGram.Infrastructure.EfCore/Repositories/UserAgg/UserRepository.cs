using MaktabGram.Domain.Core.PostAgg.Entities;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Domain.Core.UserAgg.Dtos;
using MaktabGram.Domain.Core.UserAgg.Entities;
using MaktabGram.Domain.Core.UserAgg.ValueObjects;
using MaktabGram.Infrastructure.EfCore.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MaktabGram.Infrastructure.EfCore.Repositories.UserAgg
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        public async Task Active(int userId, CancellationToken cancellationToken)
        {
            await dbContext.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsActive, true), cancellationToken);
        }

        public async Task DeActive(int userId, CancellationToken cancellationToken)
        {
            await dbContext.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsActive, false), cancellationToken);
        }

        public async Task<List<GetUserSummaryDto>> GetUsersSummary(CancellationToken cancellationToken)
        {
            return await dbContext.Users
                .Select(u => new GetUserSummaryDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Profile.Email,
                    Mobile = u.Mobile.Value,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    IsAdmin = u.IsAdmin,
                    Status = u.IsActive,
                    CreateAt = u.CreatedAt,
                    ImageProfileUrl = u.Profile.ProfileImageUrl
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsActive(string mobile, CancellationToken cancellationToken)
        {
            var mobileValue = Mobile.Create(mobile);
            return await dbContext.Users.AnyAsync(u => u.Mobile.Value == mobileValue.Value && u.IsActive, cancellationToken);
        }

        public async Task<UserLoginOutputDto?> Login(string mobile, string password, CancellationToken cancellationToken)
        {
            return await dbContext.Users
                .Where(u => u.Mobile.Value == mobile && u.PasswordHash == password)
                .Select(u => new UserLoginOutputDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Profile.Email,
                    Mobile = mobile,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    IsAdmin = u.IsAdmin
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> MobileExists(string mobile, CancellationToken cancellationToken)
        {
            return await dbContext.Users.AnyAsync(u => u.Mobile.Value == mobile, cancellationToken);
        }

        public async Task<bool> Register(RegisterUserInputDto model, CancellationToken cancellationToken)
        {
            var entity = new User
            {
                Mobile = Mobile.Create(model.Mobile),
                Username = model.Username,
                PasswordHash = model.Password,
                CreatedAt = DateTime.Now,
                Profile = new UserProfile
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfileImageUrl = model.ProfileImageUrl
                }
            };

            await dbContext.Users.AddAsync(entity, cancellationToken);
            return await dbContext.SaveChangesAsync(cancellationToken) > 1;
        }

        public async Task<UpdateGetUserDto> GetUpdateUserDetails(int userId, CancellationToken cancellationToken)
        {
            return await dbContext.Users
                .Where(x => x.Id == userId)
                .AsNoTracking()
                .Select(u => new UpdateGetUserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Profile.Email,
                    Mobile = u.Mobile.Value,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    IsAdmin = u.IsAdmin,
                    ImageProfileUrl = u.Profile.ProfileImageUrl
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> Update(int userId, UpdateGetUserDto model, CancellationToken cancellationToken)
        {
            try
            {
                var user = await dbContext.Users
                    .Include(u => u.Profile)
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

                if (user is not null)
                {
                    user.Username = model.Username;
                    user.Mobile.Value = model.Mobile;
                    user.IsAdmin = model.IsAdmin;
                    user.Profile.Email = model.Email;
                    user.Profile.FirstName = model.FirstName;
                    user.Profile.LastName = model.LastName;
                    user.PasswordHash = (!string.IsNullOrEmpty(model.Password)) ? model.Password : user.PasswordHash;

                    if (!string.IsNullOrEmpty(model.ImageProfileUrl))
                        user.Profile.ProfileImageUrl = model.ImageProfileUrl;

                    await dbContext.SaveChangesAsync(cancellationToken);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetImageProfileUrl(int userId, CancellationToken cancellationToken)
        {
            var imgAddress = await dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Profile.ProfileImageUrl)
                .FirstOrDefaultAsync(cancellationToken);

            if (imgAddress is null)
                throw new NullReferenceException("Profile image URL not found.");

            return imgAddress;
        }

        public async Task<List<int>> GetUserIdsBy(List<string> userNames, CancellationToken cancellationToken)
        {
            return await dbContext.Users
                .Where(u => userNames.Contains(u.Username))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<GetUserProfileDto> GetProfile(int searchedUserId, CancellationToken cancellationToken)
        {
            return await dbContext.Users
                .Where(u => u.Id == searchedUserId)
                .Select(u => new GetUserProfileDto
                {
                    Id = u.Id,
                    UserName = u.Username,
                    Bio = u.Profile.Bio,
                    ImgProfileUrl = u.Profile.ProfileImageUrl,
                    FollowerCount = u.Followers.Count,
                    FollowingCount = u.Followings.Count
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<GetUserProfileDto> GetProfileWithPosts(int searchedUserId, int curentUserId, CancellationToken cancellationToken)
        {
            return await dbContext.Users
                .Where(u => u.Id == searchedUserId)
                .Select(u => new GetUserProfileDto
                {
                    Id = u.Id,
                    UserName = u.Username,
                    Bio = u.Profile.Bio,
                    ImgProfileUrl = u.Profile.ProfileImageUrl,
                    FollowerCount = u.Followers.Count,
                    FollowingCount = u.Followings.Count,
                    Posts = u.Posts.Select(p => new GetUserProfilePostDto
                    {
                        PostId = p.Id,
                        CommentCount = p.Comments.Count,
                        LikeCount = p.PostLikes.Count,
                        ImgPostUrl = p.ImageUrl
                    }).ToList(),
                    SavedPosts = u.SavedPosts.Select(sp => new GetUserProfilePostDto
                    {
                        PostId = sp.PostId,
                        CommentCount = sp.Post.Comments.Count,
                        LikeCount = sp.Post.PostLikes.Count,
                        ImgPostUrl = sp.Post.ImageUrl
                    }).ToList(),
                    TagPosts = u.TaggedPosts.Select(tp => new GetUserProfilePostDto
                    {
                        PostId = tp.PostId,
                        CommentCount = tp.Post.Comments.Count,
                        LikeCount = tp.Post.PostLikes.Count,
                        ImgPostUrl = tp.Post.ImageUrl
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<SearchResultDto>> Search(string username, int userId, CancellationToken cancellationToken)
        {
            var query = dbContext.Users
                .OrderByDescending(x => x.CreatedAt)
                .Select(u => new SearchResultDto
                {
                    UserId = u.Id,
                    UserName = u.Username,
                    ImgProfileUrl = u.Profile.ProfileImageUrl,
                    IsFollowed = u.Followers.Any(f => f.FollowerId == userId)
                })
                .Take(10);

            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.UserName.Contains(username));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> IsFolllow(int searchedUserId, int curentUserId, CancellationToken cancellationToken)
        {
            return await dbContext.Followers
                .AnyAsync(f => f.FollowerId == curentUserId && f.FollowedId == searchedUserId, cancellationToken);
        }
    }

}