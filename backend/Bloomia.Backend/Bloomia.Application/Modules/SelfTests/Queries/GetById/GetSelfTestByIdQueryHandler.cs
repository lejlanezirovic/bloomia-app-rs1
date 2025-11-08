using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.GetById
{
    public class GetSelfTestByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetSelfTestByIdQuery, GetSelfTestByIdQueryDto>
    {
        public async Task<GetSelfTestByIdQueryDto> Handle(GetSelfTestByIdQuery request, CancellationToken cancellationToken)
        {
            //vadimo test za koji nam dodje zahtjev
            var selfTest = await context.SelfTests.FirstOrDefaultAsync(x => x.Id == request.SelfTestId, cancellationToken);
            //imam test trebam pitanja
            if (selfTest == null)
            {
                throw new ValidationException(message: "SelfTest doesn't exist!");
            }

            var selfTestDto = new GetSelfTestByIdQueryDto
            {
                Id = selfTest.Id,
                SelfTestName = selfTest.TestName
            };

            var selfTestQuestions = await context.SelfTestQuestions.Include(x => x.SelfTest)
                .Where(x => x.SelfTestId == selfTest.Id).ToListAsync(cancellationToken);

            foreach(var q in selfTestQuestions)
            {
                var questionDto = new GetSelfTestByIdQueryQuestionsDto
                {
                    QuestionId = q.Id,
                    Question = q.Text
                };
                selfTestDto.SelfTestQuestions.Add(questionDto);
            }

            return selfTestDto;
        }
    }
}
