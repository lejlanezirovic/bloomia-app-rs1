using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bloomia.Application.Modules.Therapists.Commands.UploadDocument
{
    public sealed class UploadTherapistDocumentCommand : IRequest<UploadTherapistDocumentCommandDto>
    {
        public IFormFile File { get; init; } = null!;
        public DocumentType DocumentType { get; init; }
        [BindNever]
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
