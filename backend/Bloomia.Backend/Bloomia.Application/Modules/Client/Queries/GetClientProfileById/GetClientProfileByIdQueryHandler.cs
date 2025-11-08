using Bloomia.Application.Modules.Journals.Commands;
using Bloomia.Application.Modules.SelfTests.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetClientProfileById
{
    public class GetClientProfileByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetClientProfileByIdQuery, GetClientProfileByIdQueryDTO>
    {
        public async Task<GetClientProfileByIdQueryDTO> Handle(GetClientProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var client = await context.Clients.Include(x => x.User).Where(x => x.UserId==request.UserId).Select
                 (x => new GetClientProfileByIdQueryDTO
                 {
                    UserId = request.UserId,
                     ClientId = x.Id,
                     Firstname = x.User.Firstname,
                     Lastname = x.User.Lastname,
                     Username = x.User.Username,
                     Age = x.User.DateOfBirth.HasValue ? today.Year - x.User.DateOfBirth.Value.Year : 20,
                     Gender=x.User.Gender.Name,
                     City=x.User.Location.City,
                     Country=x.User.Location.Country,
                     NativeLanguage=x.User.Language.Name
                    
                 }).FirstOrDefaultAsync(cancellationToken);

            var journals = await context.Journals.Where(x => x.ClientId == client.ClientId).ToListAsync(cancellationToken);
            foreach (var item in journals)
            {
                var journal = new CreateJournalCommandDto
                {
                    JournalId = item.Id,
                    JournalName = item.Title,
                    CreatedAt = item.CreatedAt
                };
                var journalAnswers=await context.JournalAnswers.Include(x=>x.Journal).Include(x=>x.JournalQuestion)
                    .Where(x=>x.JournalId==item.Id).ToListAsync(cancellationToken);
                foreach (var ans in journalAnswers) {

                    var answer = new CreateJournalAnswerCommandDto
                    {
                        QuestionId = ans.JournalQuestionId,
                        QuestionText = ans.JournalQuestion.QuestionText,
                        AnswerText = ans.AnswerText
                    };
                    journal.JournalAnswers.Add(answer);
                }
                client.Journals.Add(journal);
            }
            var selfTestsResults=await context.SelfTestResults.Include(x=>x.Client)
                    .Include(x=>x.TestAnswers).ThenInclude(x=>x.SelfTestQuestion)
                    .ThenInclude(x=>x.SelfTest).Where(x=>x.ClientId==client.ClientId).ToListAsync(cancellationToken);

            foreach(var result in selfTestsResults)
            {
                var resultDto = new SubmitSelfTestCommandDto {                   
                   TestAverage=result.AverageScore,
                   ResultDescription=result.Description
                };
                foreach(var item in result.TestAnswers)
                {
                    var answerDto = new SelfTestAnswersCommandDto { 
                        QuestionId=item.SelfTestQuestionId,
                        QuestionName=item.SelfTestQuestion.Text,
                        Rating=item.Rating                   
                    };
                    resultDto.SelfTestId = item.SelfTestQuestion.SelfTestId;
                    resultDto.SelfTestName = item.SelfTestQuestion.SelfTest.TestName;
                    resultDto.SelfTestAnswers.Add(answerDto);
                }         
                client.SelfTests.Add(resultDto);
            }
            if (client == null)
                throw new Exception($"Client with that id doesn't exist in database");

            return client;
        }
    }
}
