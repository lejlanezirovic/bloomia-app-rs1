using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Journals.Queries.List
{
    public sealed class ListQuestionsQueryDto
    {
        public List<ListQuestions> ListOfQuestions  { get; set; }=new List<ListQuestions>();
    }

    public class ListQuestions
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
    }
}
