using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Journals.Commands
{
    public class CreateJournalCommand: IRequest<CreateJournalCommandDto>
    {
        public string Title { get; init; }
        public List<CreateJournalAnswerCommandDto> ClientsAnswers { get; init; } = new List<CreateJournalAnswerCommandDto>();
        [JsonIgnore]
        public int UserId { get; set; }
    }
   
    
}
