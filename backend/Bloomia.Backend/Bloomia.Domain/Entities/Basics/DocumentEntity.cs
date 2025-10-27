using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Basics
{
    public class DocumentEntity
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
