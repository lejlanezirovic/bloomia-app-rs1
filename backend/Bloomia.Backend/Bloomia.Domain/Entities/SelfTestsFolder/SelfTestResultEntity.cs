using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.SelfTestsFolder
{
    public class SelfTestResultEntity
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public ClientEntity Client { get; set; }
        public DateTime CompletedAt { get; set; }=DateTime.UtcNow;
        public double AverageScore { get; set; }
        public string? Description { get; set; }
        public List<SelfTestAnswerEntity> TestAnswers { get; set; }= new List<SelfTestAnswerEntity>();
    }
}
