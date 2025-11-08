using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.GetById
{
    public class GetSelfTestByIdQueryDto
    {
        public int Id { get; set; }
        public string SelfTestName { get; set; }
        public List<GetSelfTestByIdQueryQuestionsDto> SelfTestQuestions { get; set; } = new List<GetSelfTestByIdQueryQuestionsDto>();
    }
    public class GetSelfTestByIdQueryQuestionsDto
    {
        public int QuestionId { get; set; }
        public string Question {  get; set; }
    }
}
