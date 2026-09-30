using Bloomia.Application.Modules.SelfTests.Command.SubmitSelfTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.MyResults.GetById
{
    public class GetMySelfTestResultByIdQuery:IRequest<GetMySelfTestResultByIdDto>
    {
        [JsonIgnore]
        public int ResultId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
    public class GetMySelfTestResultByIdDto
    {
        public int ResultId { get; set; }
        public int SelfTestId { get; set; }
        public string SelfTestName { get; set; }
        public DateTime CompletedAt { get; set; }
        public double AverageScore { get; set; }
        public string? ClientNote { get; set; }
        public string? Description { get; set; }
        public List<SelfTestAnswersCommandDto> Answers { get; set; } = new List<SelfTestAnswersCommandDto>();
    }
}
