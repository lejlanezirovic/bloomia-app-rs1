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
                .Where(x => x.IsVerified)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Firstname))
                query = query.Where(x => x.User.Firstname.ToLower().Contains(request.Firstname.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.Lastname))
                query = query.Where(x => x.User.Lastname.ToLower().Contains(request.Lastname.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.Specialization))
                query = query.Where(x => x.Specialization.ToLower().Contains(request.Specialization.ToLower()));

            if (request.GenderId.HasValue)
                query = query.Where(x => x.User.GenderId == request.GenderId.Value);

            
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(x =>
                    x.Specialization.ToLower().Contains(search) ||
                    x.Description.ToLower().Contains(search));
            }

            var sortBy = request.Paging.SortBy?.Trim().ToLowerInvariant();
            query = sortBy switch
            {
                "rating" => request.Paging.SortDescending
                    ? query.OrderByDescending(x => x.RatingAvg)
                    : query.OrderBy(x => x.RatingAvg),
                "specialization" => request.Paging.SortDescending
                    ? query.OrderByDescending(x => x.Specialization)
                    : query.OrderBy(x => x.Specialization),
                "name" => request.Paging.SortDescending
                    ? query.OrderByDescending(x => x.User.Firstname).ThenByDescending(x => x.User.Lastname)
                    : query.OrderBy(x => x.User.Firstname).ThenBy(x => x.User.Lastname),
                _ => request.SortByRatingDesc ? query.OrderByDescending(x => x.RatingAvg) : query
            };


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
