using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.Basics;

namespace Bloomia.Application.Modules.Therapists.Commands.UploadDocument
{
    public sealed class UploadTherapistDocumentCommandHandler(IAppDbContext context, IFileStorageService fileStorageService) 
        : IRequestHandler<UploadTherapistDocumentCommand, UploadTherapistDocumentCommandDto>
    {
        public async Task<UploadTherapistDocumentCommandDto> Handle(UploadTherapistDocumentCommand request, CancellationToken ct)
        {
            var therapist = await context.Therapists
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && !x.IsDeleted, ct);

            if (therapist == null)
                throw new BloomiaNotFoundException("Therapist not found.");

            var savedFile = await fileStorageService.SaveTherapistDocumentAsync(request.File, ct);

            var document = new DocumentEntity
            {
                TherapistId = therapist.Id,
                DocumentType = request.DocumentType,
                FilePath = savedFile.RelativePath,
                FileName = savedFile.OriginalFileName,
                FileExtension = savedFile.FileExtension,
                UploadedAt = DateTime.UtcNow
            };

            context.Documents.Add(document);
            await context.SaveChangesAsync(ct);

            return new UploadTherapistDocumentCommandDto
            {
                Note = "Document uploaded successfully.",
                DocumentId = document.Id,
                DocumentType = document.DocumentType.ToString(),
                FileName = document.FileName,
                FilePath = document.FilePath
            };
        }
    }
}
