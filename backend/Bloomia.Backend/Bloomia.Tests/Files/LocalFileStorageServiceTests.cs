using Bloomia.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Tests.Files
{
    public class LocalFileStorageServiceTests
    {
        [Fact]
        public async Task SaveTherapistDocument_ShouldThrow_WhenFileIsNotPdf()
        {
            var stream = new MemoryStream(new byte[100]);
            var file = new FormFile(stream, 0, 100, "file", "cv.docx")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/msword"
            };
            var service = new LocalFileStorageService(null!);

            var ex = await Assert.ThrowsAsync<Exception>(
                () => service.SaveTherapistDocumentAsync(file, CancellationToken.None));

            Assert.Equal("Only .pdf files are allowed.", ex.Message);
        }
    }
}
