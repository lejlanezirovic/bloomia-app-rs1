using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.SubmitSelfTest
{
    public class SubmitSelfTestCommandValidation:AbstractValidator<SubmitSelfTestCommand>
    {
        public SubmitSelfTestCommandValidation(IAppDbContext context)
        {
            RuleFor(x => x.TestId).GreaterThan(0).WithMessage("TestId must be grater than 0");
            RuleFor(x => x.TestId).MustAsync(async (TestId, ct) => await context.SelfTests.AnyAsync(x => x.Id == TestId, ct))
           .WithMessage("SelfTest with that id doesn't exist in database");

            RuleFor(x => x.TestName).NotEmpty().WithMessage("Test name is required!");

            RuleFor(x => x.TestAnswers).NotNull().WithMessage("Test answers cannot be null.")
                .Must(x => x.Any()).WithMessage("At least one test answer must be provided.");

            //validacija odgovora
            RuleForEach(x => x.TestAnswers).ChildRules(x =>
            {
                x.RuleFor(x => x.QuestionId).GreaterThan(0)
                    .WithMessage("Each question must have a valid ID.");

                x.RuleFor(x => x.Rating).InclusiveBetween(1, 5)
                    .WithMessage("Each rating must be between 1 and 5.");
            });
        }
    }
}
