using Microsoft.AspNetCore.Http;
namespace MaktabGram.Infrastructure.FileService.Contracts
{
    public interface IFileService
    {
        Task<string> Upload(IFormFile file, string folder, CancellationToken cancellationToken);
        Task Delete(string fileName, CancellationToken cancellationToken);
    }

}
