using Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.DirectChat.Query.NewFolder.GetByIdForTherapist
{
    public class GetDirectChatByIdTherapistQueryDto
    {
        public int ClientId { get; set; }
        public string ClientFullname { get; set; }
        public string ProfileImage { get; set; }
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
    }
}
