using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.List
{
    public class ListSelfTestsQuerySearchHandler(IAppDbContext context) : IRequestHandler<ListSelfTestsQuerySearch,ListAllSelfTestQuerySearchDto>
    {
        public async Task<ListAllSelfTestQuerySearchDto> Handle(ListSelfTestsQuerySearch request, CancellationToken cancellationToken)
        {
            var search=request.Search?.Trim()?? string.Empty;
            //if (string.IsNullOrWhiteSpace(search))
            //{
            //    throw new ValidationException(message: "String is empty!");
            //}

            var selfTests =await  context.SelfTests.Where(x => x.TestName.ToLower()
                            .Contains(search.ToLower())).ToListAsync(cancellationToken);
            var selfTestsDto = new ListAllSelfTestQuerySearchDto();

            foreach (var t in selfTests) {

                var test = new ListSelfTestQueryDto
                {
                    TestId = t.Id,
                    SelfTestName = t.TestName
                    
                };
                var selfTestQuestions = await context.SelfTestQuestions.Include(x => x.SelfTest).Where(x => x.SelfTestId == t.Id).ToListAsync(cancellationToken);
                foreach (var q in selfTestQuestions) {
                    var question = new ListSelfTestQuerySelfTestQuestionsDto
                    {
                        QuestionId = q.Id,
                        Question = q.Text
                    };
                    test.SelfTestQuestions.Add(question);
                }
                selfTestsDto.AllSelfTests.Add(test);
            }
            selfTestsDto.NumberOfTests = selfTestsDto.AllSelfTests.Count;
            return selfTestsDto;
        }
    }
}
