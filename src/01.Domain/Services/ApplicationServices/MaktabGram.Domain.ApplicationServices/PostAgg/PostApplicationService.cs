using MaktabGram.Domain.Core._common.Entities;
using MaktabGram.Domain.Core.PostAgg.Contracts;
using MaktabGram.Domain.Core.PostAgg.Dtos;
using MaktabGram.Domain.Services.PostAgg;
using MaktabGram.Infrastructure.EfCore.Repositories.PostAgg;
using MaktabGram.Infrastructure.FileService.Contracts;
using MaktabGram.Infrastructure.FileService.Services;


namespace MaktabGram.Domain.ApplicationServices.PostAgg
{
    public class PostApplicationService(IPostService postService, IFileService fileService) : IPostApplicationService
    {
        public async Task<Result<bool>> Create(CreatePostInputDto model, CancellationToken cancellationToken)
        {
            try
            {
                model.ImgUrl =  await fileService.Upload(model.Img, "Posts",cancellationToken);
                model.TaggedUsers = await postService.SetUserTags(model.Tags, cancellationToken);
                var postId = await postService.Create(model, cancellationToken);
                return Result<bool>.Success("پست با موفقیت ذخیره شد.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("ایجاد پست با خطا روبرو شد.");
            }
        }

        public async Task<List<GetPostForFeedsDto>> GetFeedPosts(int userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            return await postService.GetFeedPosts(userId, page, pageSize, cancellationToken);
        }

        public async Task<int> GetPostCount(int userId, CancellationToken cancellationToken)
        {
            return await postService.GetPostCount(userId, cancellationToken);
        }

        public async Task Like(int userId, int postId, CancellationToken cancellationToken)
        {
            await postService.Like(userId, postId, cancellationToken);
        }

        public async Task<bool> UserLikePost(int userId, int postId, CancellationToken cancellationToken)
        {
            return await postService.UserLikePost(userId, postId, cancellationToken);
        }

        public async Task DisLike(int userId, int postId, CancellationToken cancellationToken)
        {
            await postService.DisLike(userId, postId, cancellationToken);
        }

        public async Task<GetPostDetailsDto?> GetPostDetails(int postId, CancellationToken cancellationToken)
        {
            return await postService.GetPostDetails(postId, cancellationToken);
        }
    }

}
