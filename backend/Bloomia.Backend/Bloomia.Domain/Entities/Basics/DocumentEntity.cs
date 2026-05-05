using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.Enums;

namespace Bloomia.Domain.Entities.Basics
{
    public class DocumentEntity
    {
        public int Id { get; set; }
        public DocumentType DocumentType { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public int TherapistId { get; set; }
        public TherapistEntity Therapist { get; set; } = null!;
    }
}
