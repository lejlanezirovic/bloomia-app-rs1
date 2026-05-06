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
        Task<(string RelativePath, string FileName, string FileExtension)> SaveTherapistDocumentAsync(IFormFile file, CancellationToken ct);
        void DeleteIfExists(string? relativePath);
    }
}
