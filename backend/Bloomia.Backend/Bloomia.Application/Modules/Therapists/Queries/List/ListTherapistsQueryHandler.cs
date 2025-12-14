using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Modules.Users.Queries.List;

namespace Bloomia.Application.Modules.Therapists.Queries.List
{
    public sealed class ListTherapistsQueryHandler(IAppDbContext context)
        : IRequestHandler<ListTherapistsQuery, PageResult<ListTherapistsQueryDto>>
    {
        public async Task<PageResult<ListTherapistsQueryDto>> Handle(ListTherapistsQuery request, CancellationToken ct)
        {
            var query = context.Therapists
                .Include(x => x.User)
                    .ThenInclude(u => u.Gender)
                .Where(x => x.isVerified)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Firstname))
                query = query.Where(x => x.User.Firstname.ToLower().Contains(request.Firstname.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.Lastname))
                query = query.Where(x => x.User.Lastname.ToLower().Contains(request.Lastname.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.Specialization))
                query = query.Where(x => x.Specialization.ToLower().Contains(request.Specialization.ToLower()));

            if (request.GenderId.HasValue)
                query = query.Where(x => x.User.GenderId == request.GenderId.Value);

            if(request.SortByRatingDesc)
                query = query.OrderByDescending(x => x.RatingAvg);
           

            var projectedQuery = query
                .Select(x => new ListTherapistsQueryDto
                {
                    Id = x.Id,
                    Fullname = x.User.Fullname ?? (x.User.Firstname + " " + x.User.Lastname),
                    Gender = x.User.Gender != null ? x.User.Gender.Name : null,
                    RatingAvg = x.RatingAvg,
                    Specialization = x.Specialization,
                    ProfileImage = x.User.ProfileImage
                });

            return await PageResult<ListTherapistsQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
