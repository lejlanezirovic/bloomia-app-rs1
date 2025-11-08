using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.GetById
{
    public class GetSelfTestByIdQueryValidator:AbstractValidator<GetSelfTestByIdQuery>
    {
        public GetSelfTestByIdQueryValidator(IAppDbContext context)
        {
            RuleFor(x => x.SelfTestId).GreaterThan(0).WithMessage("SelfTestId must be a positive value");
            RuleFor(x => x.SelfTestId).MustAsync(async (SelfTestId, ct) => await context.SelfTests.AnyAsync(x => x.Id == SelfTestId, ct))
            .WithMessage("SelfTest with that id doesn't exist in database");
        }

    }
}
