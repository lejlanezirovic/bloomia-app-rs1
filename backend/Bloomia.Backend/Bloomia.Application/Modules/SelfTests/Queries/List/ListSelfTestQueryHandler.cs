using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.List
{
    public class ListSelfTestQueryHandler(IAppDbContext context) : IRequestHandler<ListSelfTestQuery, ListAllSelfTestsQueryDto>
    {
        public async Task<ListAllSelfTestsQueryDto> Handle(ListSelfTestQuery request, CancellationToken cancellationToken)
        {
            var selfTests = context.SelfTests.AsNoTracking();
            var allSelfTests = new ListAllSelfTestsQueryDto();

            foreach (var test in selfTests) {

                var testDto = new ListSelfTestQueryDto
                {
                    TestId = test.Id,
                    SelfTestName = test.TestName
                };
                var selfTestQuestions = await context.SelfTestQuestions.Include(x => x.SelfTest).Where(x => x.SelfTestId == test.Id).ToListAsync(cancellationToken);
                foreach (var question in selfTestQuestions) {

                    var questionDto = new ListSelfTestQuerySelfTestQuestionsDto
                    {
                        Question = question.Text,
                        QuestionId = question.Id
                    };
                    testDto.SelfTestQuestions.Add(questionDto);
                }
                allSelfTests.AllSelfTests.Add(testDto);
            }
            return allSelfTests;
        }
    }
}
