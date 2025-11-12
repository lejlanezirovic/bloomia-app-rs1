using Bloomia.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.SelfTestsFolder
{
    public class SelfTestAnswerEntity:BaseEntity
    {
        public int SelfTestQuestionId { get; set; }
        public SelfTestQuestionEntity SelfTestQuestion { get; set; }

        public int Rating { get; set; } // e.g., 1 to 5 scale

        public int SelfTestResultId { get; set; }
        public SelfTestResultEntity SelfTestResult { get; set; }
    }
}
