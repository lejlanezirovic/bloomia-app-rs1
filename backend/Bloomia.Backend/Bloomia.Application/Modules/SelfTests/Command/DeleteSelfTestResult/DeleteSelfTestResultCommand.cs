using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.DeleteSelfTestResult
{
    public class DeleteSelfTestResultCommand : IRequest<string>
    {
        [JsonIgnore]
        public int ResultId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
