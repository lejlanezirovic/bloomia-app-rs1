using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetByIdForTherapist
{
    public class GetDirectChatByIdTherapistQueryValidation:AbstractValidator<GetDirectChatByIdTherapistQuery>
    {
        public GetDirectChatByIdTherapistQueryValidation()
        {
            RuleFor(x => x.DirectChatId).NotEmpty().WithMessage("DirectChatId is required!");
        }
    }
}
