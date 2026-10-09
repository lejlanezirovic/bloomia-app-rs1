using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Articles.Queries.List
{
    public class ListArticlesQueryHandler(IAppDbContext context)
        : IRequestHandler<ListArticlesQuery, PageResult<ListArticlesQueryDto>>
    {
        public async Task<PageResult<ListArticlesQueryDto>> Handle(ListArticlesQuery request, CancellationToken ct)
        {
            var query = context.Articles.AsNoTracking()
                    .Include(x => x.Admin)
                    .ThenInclude(x => x.User)
                    .Where(x => !x.IsDeleted)
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var title = request.Title.Trim().ToLower();

                query = query.Where(x => x.Title.ToLower().Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                var content = request.Content.Trim().ToLower();

                query = query.Where(x => x.Content.ToLower().Contains(content));
            }

            if (!string.IsNullOrWhiteSpace(request.AdminName))
            {
                var adminName = request.AdminName.Trim().ToLower();

                query = query.Where(x =>
                    x.Admin.User.Fullname != null &&
                    x.Admin.User.Fullname.ToLower().Contains(adminName));
            }

            if (request.DateFrom.HasValue)
            {
                var dateFrom = request.DateFrom.Value.Date;

                query = query.Where(x => x.PublishedAt >= dateFrom);
            }

            if (request.DateTo.HasValue)
            {
                var dateToExclusive = request.DateTo.Value.Date.AddDays(1);

                query = query.Where(x => x.PublishedAt < dateToExclusive);
            }

            var sortBy = request.Paging.SortBy?.Trim().ToLowerInvariant();

            query = sortBy switch
            {
                "title" => request.Paging.SortDescending
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),

                "publishedat" => request.Paging.SortDescending
                    ? query.OrderByDescending(x => x.PublishedAt)
                    : query.OrderBy(x => x.PublishedAt),

                _ => query.OrderByDescending(x => x.PublishedAt)
            };

            var projectedQuery = query
                .Select(x => new ListArticlesQueryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublishedAt = x.PublishedAt,
                    AdminName = x.Admin.User.Fullname ?? string.Empty,
                    Excerpt = x.Content.Length > 150 ? x.Content.Substring(0, 150) + "..." : x.Content
                });

            return await PageResult<ListArticlesQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
