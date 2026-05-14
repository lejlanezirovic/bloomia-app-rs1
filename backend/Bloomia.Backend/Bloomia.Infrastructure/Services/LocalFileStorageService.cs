using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Bloomia.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalFileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveProfileImageAsync(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Profile image file is empty.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only .jpg, .jpeg, .png and .webp files are allowed.");

            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new Exception("Invalid content type");

            const long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
                throw new Exception("Profile image must be smaller than 5 MB");

            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
                webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");

            var folderPath = Path.Combine(webRoot, "uploads", "profile-images");
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            return $"/uploads/profile-images/{fileName}";
        }

        public void DeleteIfExists(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            if (!relativePath.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
                return;

            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");
            }

            var normalized = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(webRoot, normalized);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<(string RelativePath, string StoredFileName, string OriginalFileName, string FileExtension)> SaveTherapistDocumentAsync(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Document file is empty.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(extension != ".pdf")
                throw new Exception("Only .pdf files are allowed.");
                

            var allowedContentTypes = new[]
            {
                "application/pdf"
            };

            if (!allowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new Exception("Invalid document content type.");

            const long maxSize = 10 * 1024 * 1024;
            if(file.Length > maxSize)
                throw new Exception("Document must be smaller than 10 MB.");

            var webRoot = _environment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
                webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");

            var folderPath = Path.Combine(webRoot, "uploads", "documents");
            Directory.CreateDirectory(folderPath);

            var originalFileName = Path.GetFileName(file.FileName);
            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folderPath, storedFileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            var relativePath = $"/uploads/documents/{storedFileName}";

            return (relativePath, storedFileName, originalFileName, extension);
        }
    }
}
