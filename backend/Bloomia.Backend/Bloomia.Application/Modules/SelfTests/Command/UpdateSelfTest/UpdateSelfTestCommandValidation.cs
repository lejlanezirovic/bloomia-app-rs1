using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTest
{
    public class UpdateSelfTestCommandValidation: AbstractValidator<UpdateSelfTestCommand>
    {
        public UpdateSelfTestCommandValidation()
        {
            RuleFor(x => x.SelfTestId).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required!")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");
  
        }
    }
}
