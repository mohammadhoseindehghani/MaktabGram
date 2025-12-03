using Azure;
using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.PostAgg.Contracts;
using MaktabGram.Domain.Core.PostAgg.Dtos;
using MaktabGram.Domain.Core.UserAgg.Contracts;
using MaktabGram.Infrastructure.EfCore.Persistence;
using MaktabGram.Infrastructure.FileService.Contracts;

namespace MaktabGram.Domain.Services.PostAgg
{
    public class PostService(IPostRepository postRepository, IUserRepository userRepository, IFileService fileService)
        : IPostService
    {
        public async Task<Result<bool>> Create(CreatePostInputDto model, CancellationToken cancellationToken)
        {
            try
            {
                model.ImgUrl = await fileService.Upload(model.Img, "Posts", cancellationToken);
                model.TaggedUsers = await SetUserTags(model.Tags, cancellationToken);
                var postId = await postRepository.Create(model, cancellationToken);
                return Result<bool>.Success("پست با موفقیت ذخیره شد.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("ایجاد پست با خطا روبرو شد.");
            }
        }

        public async Task<List<GetPostForFeedsDto>> GetFeedPosts(int userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            return await postRepository.GetFeedPosts(userId, page, pageSize, cancellationToken);
        }

        public async Task<int> GetPostCount(int userId, CancellationToken cancellationToken)
        {
            return await postRepository.GetPostCount(userId, cancellationToken);
        }

        public async Task Like(int userId, int postId, CancellationToken cancellationToken)
        {
            await postRepository.Like(userId, postId, cancellationToken);
        }

        public async Task<bool> UserLikePost(int userId, int postId, CancellationToken cancellationToken)
        {
            return await postRepository.UserLikePost(userId, postId, cancellationToken);
        }

        public async Task<List<int>> SetUserTags(string postTags, CancellationToken cancellationToken)
        {
            var tags = postTags.Split('#').Select(x => x.Trim()).ToList();
            return await userRepository.GetUserIdsBy(tags, cancellationToken);
        }

        public async Task DisLike(int userId, int postId, CancellationToken cancellationToken)
        {
            await postRepository.DisLike(userId, postId, cancellationToken);
        }

        public async Task<GetPostDetailsDto?> GetPostDetails(int postId, CancellationToken cancellationToken)
        {
            return await postRepository.GetPostDetails(postId, cancellationToken);
        }
    }

}
