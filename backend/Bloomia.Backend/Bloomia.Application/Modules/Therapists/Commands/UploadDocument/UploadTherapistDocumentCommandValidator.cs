using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Commands.UploadDocument
{
    public sealed class UploadTherapistDocumentCommandValidator : AbstractValidator<UploadTherapistDocumentCommand>
    {
        public UploadTherapistDocumentCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("Document file is required.");

            RuleFor(x => x.DocumentType)
                .IsInEnum()
                .WithMessage("Document type is invalid.");
        }
    }
}
