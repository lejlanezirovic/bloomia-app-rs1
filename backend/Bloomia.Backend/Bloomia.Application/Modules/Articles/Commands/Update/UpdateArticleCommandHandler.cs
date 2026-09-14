using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Articles.Commands.Update
{
    internal class UpdateArticleCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<UpdateArticleCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateArticleCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAuthenticated)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You have to be logged in.");

            if (!currentUser.IsAdmin)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only admins can add new articles.");

            var article = await context.Articles
                .Include(x => x.Admin)
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (article == null)
                throw new BloomiaNotFoundException($"Article with Id {request.Id} not found.");

            if (article.Admin.UserId != currentUser.UserId)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only the admin who created this article can update it.");

            var title = request.Title.Trim();
            var content = request.Content.Trim();

            var titleExists = await context.Articles
                .AnyAsync(x => x.Id != request.Id && x.Title.ToLower() == request.Title.ToLower(), ct);

            if (titleExists)
                throw new BloomiaConflictException("Article with that title already exists.");

            article.Title = title;
            article.Content = content;

            await context.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
