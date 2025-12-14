using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloomia.Domain.Entities.Admin;

namespace Bloomia.Application.Modules.Articles.Commands.Create
{
    public class CreateArticleCommandHandler(IAppDbContext context, IAppCurrentUser currentUser)
        : IRequestHandler<CreateArticleCommand, int>
    {
        public async Task<int> Handle(CreateArticleCommand request, CancellationToken ct)
        {
            if (!currentUser.IsAuthenticated)
                throw new BloomiaBusinessRuleException("NOT_LOGGED_IN", "You have to be logged in.");
            
            if (!currentUser.IsAdmin)
                throw new BloomiaBusinessRuleException("USER_NOT_AUTH", "Only admins can add new articles.");

            var title = request.Title.Trim();
            var content = request.Content.Trim();

            //prebačeno u validator
            //if (string.IsNullOrWhiteSpace(title) || title == "string")
            //    throw new ValidationException("Title cannot be only whitespace or default value.");

            //if (string.IsNullOrWhiteSpace(content) || content == "string")
            //    throw new ValidationException("Content cannot be only whitespace or default value.");

            bool articleExists = await context.Articles
                .AnyAsync(x => x.Title.ToLower() == title.ToLower(), ct);

            if (articleExists)
                throw new BloomiaConflictException($"Article with title {title} already exists.");

            var admin = await context.Admins
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId, ct);

            if (admin == null)
                throw new BloomiaBusinessRuleException("PERMISSION", "Logged-in user is not an admin.");
            
            //id admina uzimamo od prijavljenog korisnika
            var article = new ArticleEntity
            {
                AdminId = admin.Id,
                Title = title,
                Content = content
            };

            context.Articles.Add(article);
            await context.SaveChangesAsync(ct);

            return article.Id;
        }

    }
}
