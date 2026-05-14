using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.Basics;

namespace Bloomia.Application.Modules.Therapists.Commands.DeleteDocument
{
    public sealed class DeleteTherapistDocumentCommandHandler(IAppDbContext context, IFileStorageService fileStorageService) : IRequestHandler<DeleteTherapistDocumentCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTherapistDocumentCommand request, CancellationToken ct)
        {
            var document = await context.Documents.Include(x => x.Therapist)
                            .ThenInclude(x => x.User)
                            .FirstOrDefaultAsync(x => x.Id == request.DocumentId, ct);

            if (document == null)
                throw new BloomiaNotFoundException("Document not found");

            if (document.Therapist == null || document.Therapist.UserId != request.UserId)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Not authorized to delete this document");

            var filePath = document.FilePath;

            context.Documents.Remove(document);
            await context.SaveChangesAsync(ct);

            fileStorageService.DeleteIfExists(filePath);

            return Unit.Value;
        }
    }
}
