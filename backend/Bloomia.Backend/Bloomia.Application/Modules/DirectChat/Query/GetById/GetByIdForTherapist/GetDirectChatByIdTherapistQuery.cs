using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetByIdForTherapist
{
    public sealed class GetDirectChatByIdTherapistQuery:IRequest<GetDirectChatByIdTherapistQueryDto>
    {
        public int DirectChatId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
