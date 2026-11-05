using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Abstractions;
using Bloomia.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Bloomia.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private const string DocumentsPrefix = "documents/";

        private readonly IWebHostEnvironment _environment;
        private readonly FileStorageSettings _settings;

        public LocalFileStorageService(IWebHostEnvironment environment, IOptions<FileStorageSettings> settings)
        {
            _environment = environment;
            _settings = settings.Value;
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

            var folderPath = GetDocumentsFolder();
            Directory.CreateDirectory(folderPath);

            var originalFileName = Path.GetFileName(file.FileName);
            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folderPath, storedFileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, ct);

            var relativePath = $"{DocumentsPrefix}{storedFileName}";

            return (relativePath, storedFileName, originalFileName, extension);
        }

        public async Task<byte[]> ReadTherapistDocumentAsync(string relativePath, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new Exception("Document path is empty.");

            if (!relativePath.StartsWith(DocumentsPrefix, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Invalid document path.");

            var fullPath = Path.Combine(GetDocumentsFolder(), Path.GetFileName(relativePath));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Document file was not found.");

            return await File.ReadAllBytesAsync(fullPath, ct);
        }

        public void DeleteTherapistDocumentIfExists(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            if (!relativePath.StartsWith(DocumentsPrefix, StringComparison.OrdinalIgnoreCase))
                return;

            var fullPath = Path.Combine(GetDocumentsFolder(), Path.GetFileName(relativePath));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        private string GetDocumentsFolder()
            => Path.IsPathRooted(_settings.DocumentsPath)
                ? _settings.DocumentsPath
                : Path.Combine(_environment.ContentRootPath, _settings.DocumentsPath);

        public async Task<(string RelativePath, string StoredFileName)> SaveReportAsync(byte[] content, string fileName, CancellationToken ct)
        {
            if (content == null || content.Length == 0)
                throw new Exception("Report content is empty.");

            var folderPath = Path.Combine(_environment.ContentRootPath, "App_Data", "reports");

            Directory.CreateDirectory(folderPath);

            var storedFileName = $"{Guid.NewGuid()}.pdf";
            var fullPath = Path.Combine(folderPath, storedFileName);

            await File.WriteAllBytesAsync(fullPath, content, ct);

            var relativePath = $"reports/{storedFileName}";

            return (relativePath, storedFileName);
        }

        public async Task<byte[]> ReadReportAsync(string relativePath, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new Exception("Report path is empty.");

            if (!relativePath.StartsWith("reports/", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Invalid report path.");

            var fileName = Path.GetFileName(relativePath);

            var fullPath = Path.Combine(_environment.ContentRootPath, "App_Data", "reports", fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Report file was not found.");

            return await File.ReadAllBytesAsync(fullPath, ct);
        }

        public void DeleteReportIfExists(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return;

            if (!relativePath.StartsWith("reports/", StringComparison.OrdinalIgnoreCase))
                return;

            var fileName = Path.GetFileName(relativePath);

            var fullPath = Path.Combine(
                _environment.ContentRootPath,
                "App_Data",
                "reports",
                fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
