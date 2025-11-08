using Bloomia.Domain.Entities.SelfTestsFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command
{
    public class SubmitSelfTestCommandHandler(IAppDbContext context) : IRequestHandler<SubmitSelfTestCommand, SubmitSelfTestCommandDto>
    {
        public async Task<SubmitSelfTestCommandDto> Handle(SubmitSelfTestCommand request, CancellationToken cancellationToken)
        {
            var SelfTest = await context.SelfTests.FirstOrDefaultAsync(x => x.Id == request.TestId, cancellationToken);
            var client = await context.Clients.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == request.UserId, cancellationToken);

            if (SelfTest== null)
            {
                throw new ValidationException(message: "SelfTest doesn't exist!");
            }
            if (client == null)
            {
                throw new ValidationException(message: "Client not found!, Register or login");
            }
            var selfTestResult = new SelfTestResultEntity
            {
                Client = client,
                ClientId = client.Id,
                CompletedAt = DateTime.UtcNow
            };
            context.SelfTestResults.Add(selfTestResult);


            foreach(var item in request.TestAnswers)
            {
                var answer = new SelfTestAnswerEntity
                {
                    SelfTestResultId = selfTestResult.Id,
                    SelfTestResult = selfTestResult,
                    SelfTestQuestionId = item.QuestionId,
                    Rating = item.Rating 
                };            
                context.SelfTestAnswers.Add(answer);          
            } 
            selfTestResult.AverageScore = request.TestAnswers.Average(x=>x.Rating);
            await context.SaveChangesAsync(cancellationToken);

            var savedResult = await context.SelfTestResults.Include(x => x.TestAnswers)
                .ThenInclude(x => x.SelfTestQuestion).FirstOrDefaultAsync(x => x.Id == selfTestResult.Id, cancellationToken);

            var clientInformations = new ClientInformationDto
            {
                FirstName = client.User.Firstname,
                LastName = client.User.Lastname
            };
            var selfTestDto = new SubmitSelfTestCommandDto
            {
                ClientInformation = clientInformations,
                SelfTestId=SelfTest.Id,
                SelfTestName = SelfTest.TestName,
                TestAverage = selfTestResult.AverageScore
            };
            foreach(var item in savedResult.TestAnswers)
            {
                var answerDto = new SelfTestAnswersCommandDto
                {
                    QuestionId = item.SelfTestQuestionId,
                    QuestionName = item.SelfTestQuestion.Text,
                    Rating = item.Rating
                };
                selfTestDto.SelfTestAnswers.Add(answerDto);               
            }

            if (selfTestDto.SelfTestId ==4)
            {
                if (selfTestDto.TestAverage <= 2.4)
                {
                    selfTestDto.ResultDescription = "You are characterized by strong emotional " +
                        "self-regulation and a balanced mood.\r\nYou clearly understand, recognize," +
                        " and express your emotions without being overwhelmed by them.\r\n" +
                        "This demonstrates a high degree of emotional maturity and inner peace.";

                    selfTestResult.Description = "High level of emotional stability";
                }
                if (selfTestDto.TestAverage >= 2.5 && selfTestDto.TestAverage <= 3.4)
                {
                    selfTestDto.ResultDescription = "Moderate to good level of emotional regulation. You show a solid awareness of your emotions and generally manage " +
                        "to control them, although mood swings sometimes occur.\r\nYou mostly react in a balanced way" +
                        " and understand your feelings, but there is room for additional development of emotional " +
                        "stability and expression.";
                    selfTestResult.Description = "Moderate to good level of emotional regulation";
                }
                if (selfTestDto.TestAverage >= 3.5)
                {
                    selfTestDto.ResultDescription = "Low level of emotional regulation. Emotional changes are frequent and sometimes overwhelm you." +
                         "There is difficulty in understanding and expressing your own feelings, which can affect" +
                         " your inner stability and relationships with others." +
                         "I recommend working on calming techniques and awareness of emotions, such as mindfulness" +
                         " or keeping a diary of feelings.";
                    selfTestResult.Description = "Low level of emotional regulation";
                }
            }
            if (selfTestDto.SelfTestId == 5)
            {
                if (selfTestDto.TestAverage <= 2.4)
                {
                    selfTestDto.ResultDescription = "Mature and open communication\r\n\r\nYou " +
                       "demonstrate a high level of emotional maturity in relationships.\r\nYou " +
                       "clearly express your thoughts and feelings, know how to set boundaries, " +
                       "and have constructive conversations even in conflict situations.\r\nYour" +
                       " ability to communicate respectfully contributes to stable and quality " +
                       "relationships.";
                    selfTestResult.Description = "Mature and open communication";
                }
                else if(selfTestDto.TestAverage>=2.5 && selfTestDto.TestAverage <= 3.4)
                {
                    selfTestDto.ResultDescription = "Balanced but occasionally insecure approach to relationships\r\n\r\n" +
                        "You are usually able to communicate clearly and respectfully, but sometimes" +
                        " you find it difficult to express your feelings honestly or stand up for " +
                        "yourself.\r\nYou have a good foundation for healthy relationships, and you" +
                        " can make further progress by developing emotional openness and confidence " +
                        "in communication.";
                    selfTestResult.Description = "Balanced but occasionally insecure approach to relationships";
                }
                if(selfTestDto.TestAverage >= 3.5)                
                {
                    selfTestDto.ResultDescription = "Difficulty in communication and setting boundaries\r\n\r\n" +
                       "You have pronounced challenges in expressing your feelings and needs.\r\nYou often avoid" +
                      " conflicts and find it difficult to set boundaries, which can lead to dissatisfaction in " +
                      "relationships.\r\nAnd I would recommend that you work on assertive communication and open" +
                     " expression of emotions with respect for others.";
                    selfTestResult.Description = "Difficulty communicating and setting boundaries";
                }
            }
            if (selfTestDto.SelfTestId == 6)
            {
                if (selfTestDto.TestAverage <= 2.4)
                {
                    selfTestDto.ResultDescription = "Low level of stress and exhaustion. Good emotional resilience\r\n\r\nYou show a high" +
                      " level of internal resilience and manage stress well.\r\nYou have developed " +
                      "recovery strategies and know how to recognize the early signs of exhaustion" +
                      " before they become serious.\r\nMaintaining this balance helps you stay " +
                      "motivated and mentally stable in the long run.";
                    selfTestResult.Description = "Low level of stress and exhaustion";
                }
                else if(selfTestDto.TestAverage>=2.5 && selfTestDto.TestAverage <= 3.4)
                {
                    selfTestDto.ResultDescription = "Moderate level of stress and exhaustion\r\n\r\nYou" +
                        " occasionally feel tired and lose concentration, but you generally manage to" +
                        " recover and continue with your responsibilities.\r\nIt is important to " +
                        "recognize your limits and find ways to recharge your energy through rest, " +
                        "hobbies, and the support of your environment.";
                    selfTestResult.Description = "Moderate level of stress and exhaustion";
                }
                if (selfTestDto.TestAverage >= 3.5)
                {
                    selfTestDto.ResultDescription = "High risk of burning\r\n\r\nYou feel chronic" +
                        " fatigue and exhaustion even after rest.\r\nIt's hard for you to focus," +
                        " you lose motivation and your daily obligations burden you.\r\nThis could be" +
                        " a sign that you need a break, a better work-life balance, or a talk with an" +
                        " expert.";
                    selfTestResult.Description = "High risk of burning";
                }
            }
            await context.SaveChangesAsync(cancellationToken);

            return selfTestDto;
        }
    }
}
