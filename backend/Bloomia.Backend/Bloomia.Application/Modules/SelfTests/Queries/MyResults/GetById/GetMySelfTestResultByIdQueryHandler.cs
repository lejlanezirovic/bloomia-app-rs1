using Bloomia.Application.Modules.SelfTests.Command.SubmitSelfTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.MyResults.GetById
{
    public class GetMySelfTestResultByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetMySelfTestResultByIdQuery, GetMySelfTestResultByIdDto>
    {
        public async Task<GetMySelfTestResultByIdDto> Handle(GetMySelfTestResultByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await context.Clients.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
            if (client == null)
            {
                throw new BloomiaNotFoundException("Client not found");
            }

            var result = await context.SelfTestResults
                .Include(x => x.TestAnswers).ThenInclude(x => x.SelfTestQuestion).ThenInclude(x => x.SelfTest)
                .FirstOrDefaultAsync(x => x.Id == request.ResultId && x.ClientId == client.Id && !x.IsDeleted, cancellationToken);

            if (result == null)
            {
                throw new BloomiaNotFoundException("Self test result not found");
            }

            var dto = new GetMySelfTestResultByIdDto
            {
                ResultId = result.Id,
                CompletedAt = result.CompletedAt,
                AverageScore = result.AverageScore,
                Description = result.Description,
                ClientNote = result.ClientNote
            };

            foreach (var answer in result.TestAnswers)
            {
                dto.SelfTestId = answer.SelfTestQuestion.SelfTestId;
                dto.SelfTestName = answer.SelfTestQuestion.SelfTest.TestName;
                dto.Answers.Add(new SelfTestAnswersCommandDto
                {
                    QuestionId = answer.SelfTestQuestionId,
                    QuestionName = answer.SelfTestQuestion.Text,
                    Rating = answer.Rating
                });
            }

            return dto;

        }
    }
}
