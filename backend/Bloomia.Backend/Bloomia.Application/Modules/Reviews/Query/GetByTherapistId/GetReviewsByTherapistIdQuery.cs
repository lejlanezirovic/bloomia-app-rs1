using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Reviews.Query.GetByTherapistId
{
    public sealed class GetReviewsByTherapistIdQuery : BasePagedQuery<GetReviewsByTherapistIdQueryDto>
    {
        public int TherapistId { get; set; }
    }
}
