using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.CreateSelfTest
{
    public class CreateSelfTestCommandDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CreateSelfTestStatementCommandDto> Statements { get; set; } = new List<CreateSelfTestStatementCommandDto>();

    }
    public class CreateSelfTestStatementCommandDto
    {
        [JsonIgnore]
        public int SelfTestId { get; set; }
        public string StatementText { get; set; }
    }
}
