using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Articles.Queries.List
{
    public sealed class ListArticlesQueryValidator : AbstractValidator<ListArticlesQuery>
    {
        public ListArticlesQueryValidator()
        {
            RuleFor(x => x.Title) .MaximumLength(150);

            RuleFor(x => x.Content).MaximumLength(500);

            RuleFor(x => x.AdminName).MaximumLength(150);

            RuleFor(x => x)
                .Must(x =>
                    !x.DateFrom.HasValue ||
                    !x.DateTo.HasValue ||
                    x.DateFrom.Value.Date <= x.DateTo.Value.Date)
                .WithMessage("Start date must be before end date.");
        }
    }
}
