using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.List
{
    public class ListAllSelfTestsQueryDto
    {
        public List<ListSelfTestQueryDto> AllSelfTests {  get; set; }= new List<ListSelfTestQueryDto>();
    }
    public class ListAllSelfTestQuerySearchDto
    {
        public int NumberOfTests { get; set; }
        public List<ListSelfTestQueryDto> AllSelfTests { get; set; } = new List<ListSelfTestQueryDto>();

    }
    public class ListSelfTestQueryDto
    {
        public int TestId { get; set; }
        public string SelfTestName { get; set; }
        public List<ListSelfTestQuerySelfTestQuestionsDto> SelfTestQuestions { get; set; } = new List<ListSelfTestQuerySelfTestQuestionsDto>();   
    }
    public class ListSelfTestQuerySelfTestQuestionsDto
    {
        public int QuestionId { get; set; }
        public string Question {  get; set; }
    }

}
