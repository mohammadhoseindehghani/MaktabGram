using MaktabGram.Infrastructure.FileService.Contracts;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MaktabGram.Infrastructure.FileService.Services
{
    public class FileService : IFileService
    {
        public async Task Delete(string fileName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath), cancellationToken);
            }
        }

        public async Task<string> Upload(IFormFile file, string folder, CancellationToken cancellationToken)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            return Path.Combine("Files", folder, uniqueFileName);
        }
    }

}
