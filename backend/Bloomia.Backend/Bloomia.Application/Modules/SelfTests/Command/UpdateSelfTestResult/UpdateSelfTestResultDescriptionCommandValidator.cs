using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTestResult
{
    public class UpdateSelfTestResultDescriptionCommandValidator : AbstractValidator<UpdateSelfTestResultDescriptionCommand>
    {
        public UpdateSelfTestResultDescriptionCommandValidator()
        {
            RuleFor(x => x.ResultId).GreaterThan(0).WithMessage("ResultId must be greater than 0");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not be longer than 500 characters");
        }
    }
}
