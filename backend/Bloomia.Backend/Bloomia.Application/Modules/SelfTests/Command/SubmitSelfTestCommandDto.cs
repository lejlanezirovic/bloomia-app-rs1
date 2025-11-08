using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command
{
    public class SubmitSelfTestCommandDto
    {
        public ClientInformationDto ClientInformation {  get; set; }
        public int SelfTestId { get; set; }
        public string SelfTestName { get; set; }
        public List<SelfTestAnswersCommandDto> SelfTestAnswers { get; set; }=new List<SelfTestAnswersCommandDto>();
        public double TestAverage {  get; set; }
        public string ResultDescription { get; set; }
    }
    public class ClientInformationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
