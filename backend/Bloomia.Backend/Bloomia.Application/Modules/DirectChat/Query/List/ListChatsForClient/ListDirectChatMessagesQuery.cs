using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.List.ListChatsForClient
{
    public sealed class ListDirectChatMessagesQuery: IRequest<List<ListDirectChatMessagesQueryDto>>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
