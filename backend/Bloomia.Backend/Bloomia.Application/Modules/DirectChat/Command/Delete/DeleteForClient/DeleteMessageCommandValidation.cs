using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Command.Delete.DeleteForClient
{
    public class DeleteMessageCommandValidation:AbstractValidator<DeleteMessageCommand>
    {
        public DeleteMessageCommandValidation()
        {
            RuleFor(x => x.DirectChatId).NotEmpty().WithMessage("DirectChatId is required!");
            RuleFor(x => x.MessageId).NotEmpty().WithMessage("MessageId is required!");
        }
    }
}
