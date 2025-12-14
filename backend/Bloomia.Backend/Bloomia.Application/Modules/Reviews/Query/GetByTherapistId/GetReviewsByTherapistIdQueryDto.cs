using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Reviews.Query.GetByTherapistId
{
    public sealed class GetReviewsByTherapistIdQueryDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string ClientInitials { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
