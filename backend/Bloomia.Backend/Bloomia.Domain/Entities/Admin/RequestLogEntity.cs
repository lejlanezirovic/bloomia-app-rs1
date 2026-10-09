using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Admin
{
    public class RequestLogEntity : BaseEntity
    {
        public required string Method { get; set; } 
        public required string Path { get; set; }
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
    }
}
