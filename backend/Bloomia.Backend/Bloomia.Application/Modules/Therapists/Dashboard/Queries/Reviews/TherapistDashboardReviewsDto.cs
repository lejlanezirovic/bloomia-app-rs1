using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Therapists.Dashboard.Queries.Reviews
{
    public class TherapistDashboardReviewsDto
    {
        public int TotalReviews { get; set; }
        public int FiveStarReviews { get; set; }
        public int FourStarReviews { get; set; }
        public int ThreeStarReviews { get; set; }
        public int TwoStarReviews { get; set; }
        public int OneStarReviews { get; set; }
        public List<TherapistDashboardReviewItemDto> LatestReviews { get; set; } = [];
    }
}
