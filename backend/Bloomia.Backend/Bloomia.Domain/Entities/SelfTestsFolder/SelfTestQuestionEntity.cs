using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.SelfTestsFolder
{
    public class SelfTestQuestionEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int SelfTestId { get; set; }
        public SelfTestEntity SelfTest { get; set; }

    }
}
