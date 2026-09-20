using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Application.Modules.Users.Queries.GetById;
using Bloomia.Domain.Entities.Admin;

namespace Bloomia.Application.Modules.Articles.Queries.GetById
{
    public class GetArticleByIdQueryHandler(IAppDbContext context) 
        : IRequestHandler<GetArticleByIdQuery, GetArticleByIdQueryDto>
    {
        public async Task<GetArticleByIdQueryDto> Handle(GetArticleByIdQuery request, CancellationToken ct)
        {
            var article = await context.Articles
                .AsNoTracking()
                .Include(x => x.Admin)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);

            if (article == null)
                throw new BloomiaNotFoundException($"Article with Id {request.Id} not found.");

            var articleDto = new GetArticleByIdQueryDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                PublishedAt = article.PublishedAt,
                AdminName = article.Admin.User.Fullname ?? string.Empty
            };

            return articleDto;
        }
    }
}
