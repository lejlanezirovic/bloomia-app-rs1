using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bloomia.Application.Modules.Therapists.Commands.UploadDocument
{
    public sealed class UploadTherapistDocumentCommandValidator : AbstractValidator<UploadTherapistDocumentCommand>
    {
        public UploadTherapistDocumentCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("Document file is required.");

            RuleFor(x => x.File)
                .Must(IsPdf)
                .When(x => x.File != null)
                .WithMessage("Only .pdf files are allowed.");

            RuleFor(x => x.DocumentType)
                .IsInEnum()
                .WithMessage("Document type is invalid.");
        }

        private static bool IsPdf(IFormFile file)
            => string.Equals(Path.GetExtension(file.FileName), ".pdf", StringComparison.OrdinalIgnoreCase)
               && string.Equals(file.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase);
    }
}
