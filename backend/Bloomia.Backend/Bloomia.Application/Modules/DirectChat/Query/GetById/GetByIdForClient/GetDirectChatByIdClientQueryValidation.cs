using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetById
{
    public class GetDirectChatByIdClientQueryValidation:AbstractValidator<GetDirectChatByIdClientQuery>
    {
        public GetDirectChatByIdClientQueryValidation()
        {
            RuleFor(x => x.DirectChatId).NotEmpty().WithMessage("DirectChatId is required!");
        }
    }
}
