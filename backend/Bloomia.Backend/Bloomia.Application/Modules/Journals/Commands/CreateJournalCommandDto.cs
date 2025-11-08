using Bloomia.Application.Modules.Journals.Queries.List;
using Bloomia.Domain.Entities.JournalsFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Journals.Commands
{
    public class CreateJournalCommandDto
    {     
        public int JournalId {  get; set; }
        public string JournalName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CreateJournalAnswerCommandDto> JournalAnswers { get; set; } = new List<CreateJournalAnswerCommandDto>();

    }
    public class CreateJournalAnswerCommandDto
    {
        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public string AnswerText { get; set; }

    }
}
