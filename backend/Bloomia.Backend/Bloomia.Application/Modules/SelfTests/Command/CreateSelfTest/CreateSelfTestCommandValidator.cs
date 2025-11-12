using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.CreateSelfTest
{
    public class CreateSelfTestCommandValidator: AbstractValidator<CreateSelfTestCommand>
    {
        public CreateSelfTestCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required!")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

            RuleForEach(x => x.Statements).ChildRules(x =>

                x.RuleFor(x => x.StatementText).NotEmpty().WithMessage("Statement text is required!")
            );
        }

    }
}
