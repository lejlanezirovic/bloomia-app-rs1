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
            var query = context.Articles
                .Include(x => x.Admin)
                .ThenInclude(admin => admin.User)
                .Where(a => !a.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(x =>
                    x.Title.ToLower().Contains(search) ||
                    x.Content.ToLower().Contains(search));
            }

            var sortBy = request.Paging.SortBy?.Trim().ToLowerInvariant();
            query = sortBy switch
            {
                "title" => request.Paging.SortDescending
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),
                "publishedat" => request.Paging.SortDescending
                    ? query.OrderBy(x => x.PublishedAt)
                    : query.OrderByDescending(x => x.PublishedAt),
                _ => query.OrderByDescending(x => x.PublishedAt) 
            };

            var projectedQuery = query
                .Select(x => new ListArticlesQueryDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublishedAt = x.PublishedAt,
                    AdminName = x.Admin.User.Fullname,
                    Excerpt = x.Content.Length > 150 ? x.Content.Substring(0, 150) + "..." : x.Content
                });

            return await PageResult<ListArticlesQueryDto>.FromQueryableAsync(projectedQuery, request.Paging, ct);
        }
    }
}
