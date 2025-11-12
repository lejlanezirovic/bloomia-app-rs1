using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.SelfTestsFolder
{
    public class SelfTestEntity: BaseEntity
    {
        public string TestName { get; set; }

        public List<SelfTestQuestionEntity> TestQuestions { get; set; } = new List<SelfTestQuestionEntity>();
    }
}
