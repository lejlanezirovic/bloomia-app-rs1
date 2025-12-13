using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForTherapist
{
    public sealed class ListDirectChatMessagesTherapistQuery:IRequest<List<ListDirectChatMessagesTherapistQueryDto>>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
