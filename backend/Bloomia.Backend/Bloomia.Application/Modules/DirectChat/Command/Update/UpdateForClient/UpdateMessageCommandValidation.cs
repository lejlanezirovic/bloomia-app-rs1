using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Update.UpdateForClient
{
    public class UpdateMessageCommandValidation:AbstractValidator<UpdateMessageCommand>
    {
        public UpdateMessageCommandValidation()
        {
            RuleFor(x => x.DirectChatId).NotEmpty().WithMessage("DirectChatId is required!");
            RuleFor(x => x.MessageId).NotEmpty().WithMessage("MessageId is required!");
            RuleFor(x => x.NewContent).NotEmpty().WithMessage("NewContent is required!");
        }
    }
}
