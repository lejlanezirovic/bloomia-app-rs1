using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Create.CreateForClient
{
    public class SendMessageCommandValidation:AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidation()
        {
            RuleFor(x => x.TherapistId).NotEmpty().WithMessage("TherapistId is required!");
            RuleFor(x=>x.Content).NotEmpty().WithMessage("Content is required!");
        }
    }
}
