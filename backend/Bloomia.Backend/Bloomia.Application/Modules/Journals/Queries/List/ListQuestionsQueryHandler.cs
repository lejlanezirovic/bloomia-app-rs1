using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Journals.Queries.List
{
    public  class ListQuestionsQueryHandler(IAppDbContext context) : IRequestHandler<ListQuestionsQuery,ListQuestionsQueryDto>
    {
        public async Task<ListQuestionsQueryDto> Handle(ListQuestionsQuery request ,CancellationToken cancellationToken)
        {
            var questions = context.JournalQuestions.AsNoTracking();
            if (questions == null)
                throw new Exception("Questions weren't found in database");

            var questionsDto = new ListQuestionsQueryDto();
            foreach (var q in questions) {
                var qDto = new ListQuestions
                {
                    QuestionId = q.Id,
                    Question = q.QuestionText
                };
                questionsDto.ListOfQuestions.Add(qDto);             
            }
            return questionsDto;
        }
    }
}
