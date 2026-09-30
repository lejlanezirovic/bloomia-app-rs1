using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.MyResults.List
{
    public class ListMySelfTestResultsQueryValidator : AbstractValidator<ListMySelfTestResultsQuery>
    {
        public ListMySelfTestResultsQueryValidator()
        {
            RuleFor(x => x.TestName)
                .MaximumLength(100).WithMessage("Test name must not be longer than 100 characters")
                .When(x => x.TestName != null);

            RuleFor(x => x.MinAverage)
                .InclusiveBetween(1, 5).WithMessage("Minimum average must be between 1 and 5")
                .When(x => x.MinAverage.HasValue);

            RuleFor(x => x.MaxAverage)
                .InclusiveBetween(1, 5).WithMessage("Maximum average must be between 1 and 5")
                .When(x => x.MaxAverage.HasValue);

            RuleFor(x => x.CompletedTo)
                .GreaterThanOrEqualTo(x => x.CompletedFrom!.Value)
                .WithMessage("'Completed to' date must be after 'completed from' date")
                .When(x => x.CompletedFrom.HasValue && x.CompletedTo.HasValue);
        }
    }
}
