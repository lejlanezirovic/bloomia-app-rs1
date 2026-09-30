using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.DeleteSelfTestResult
{
    public class DeleteSelfTestResultCommandValidator : AbstractValidator<DeleteSelfTestResultCommand>
    {
        public DeleteSelfTestResultCommandValidator()
        {
            RuleFor(x => x.ResultId).GreaterThan(0).WithMessage("ResultId must be greater than 0");
        }
    }
}
