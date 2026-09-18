using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bloomia.Application.Abstractions
{
    public interface IFileStorageService
    {
        Task<string> SaveProfileImageAsync(IFormFile file, CancellationToken ct);
        Task<(string RelativePath, string StoredFileName, string OriginalFileName, string FileExtension)> SaveTherapistDocumentAsync(IFormFile file, CancellationToken ct);
        void DeleteIfExists(string? relativePath);
        Task<(string RelativePath, string StoredFileName)> SaveReportAsync(byte[] content, string fileName, CancellationToken ct);
        Task<byte[]> ReadReportAsync(string relativePath, CancellationToken ct);

        void DeleteReportIfExists(string? relativePath);
    }
}
