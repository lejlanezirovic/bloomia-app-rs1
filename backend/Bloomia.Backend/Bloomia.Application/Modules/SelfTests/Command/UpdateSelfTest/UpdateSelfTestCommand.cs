using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTest
{
    public class UpdateSelfTestCommand:IRequest<UpdateSelfTestCommandDto>
    {
        //id, title, 
        [JsonIgnore]
        public int SelfTestId { get; set; }
        public string Title { get; init; }

    }
}
