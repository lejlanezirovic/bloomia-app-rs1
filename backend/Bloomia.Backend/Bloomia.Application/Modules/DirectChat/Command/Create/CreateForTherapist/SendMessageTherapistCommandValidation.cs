using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Create.CreateForTherapist
{
    public class SendMessageTherapistCommandValidation:AbstractValidator<SendMessageTherapistCommand>
    {
        public SendMessageTherapistCommandValidation()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId is required!");
            RuleFor(x=>x.Content).NotEmpty().WithMessage("Content is required!");
        }
    }
}
