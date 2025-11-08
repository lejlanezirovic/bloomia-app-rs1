using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command
{
    public class SubmitSelfTestCommand:IRequest<SubmitSelfTestCommandDto>
    {
        public int TestId { get; init; }
        public string? TestName { get; init; }
        [JsonIgnore]
        public int UserId { get; set; }
        public List<SelfTestAnswersCommandDto> TestAnswers { get; init; }=new List<SelfTestAnswersCommandDto>();
    }
    public class SelfTestAnswersCommandDto
    {
        public int QuestionId { get; set; }
        public string? QuestionName { get; set; }
        public int Rating { get; set; }
    }
}
