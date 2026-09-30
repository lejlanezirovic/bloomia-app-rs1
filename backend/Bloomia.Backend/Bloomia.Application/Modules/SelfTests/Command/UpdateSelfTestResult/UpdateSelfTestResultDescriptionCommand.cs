using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTestResult
{
    public class UpdateSelfTestResultDescriptionCommand:  IRequest<UpdateSelfTestResultDescriptionCommandDto>
    {
        [JsonIgnore]
        public int ResultId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        public string Description { get; set; }
    }
    public class UpdateSelfTestResultDescriptionCommandDto
    {
        public int ResultId { get; set; }
        public string Description { get; set; }
    }
}
