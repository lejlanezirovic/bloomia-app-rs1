using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.JournalsFolder
{
    public class JournalAnswerEntity
    {
        public int Id { get; set; }
        public int JournalId { get; set; }
        public JournalEntity Journal { get; set; }
        public string AnswerText { get; set; }
        public int JournalQuestionId { get; set; }
        public JournalQuestionEntity JournalQuestion { get; set; }
    }
}
