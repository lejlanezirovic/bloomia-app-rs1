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

        // public string Answer1 { get; init; }
        //public string Answer2 { get; init; }
        //public string Answer3 { get; init; }
        //public string Answer4 { get; init; }
        //public string Answer5 { get; init; }

        public List<CreateJournalAnswerCommandDto> ClientsAnswers { get; init; } = new List<CreateJournalAnswerCommandDto>();
        [JsonIgnore]
        public int UserId { get; set; }
    }
   
    
}
