using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.SelfTests.Queries.MyResults.List
{
    public class ListMySelfTestResultsQuery: BasePagedQuery<ListMySelfTestResultDto>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public string? TestName { get; set; }
        public DateTime? CompletedFrom { get; set; }
        public DateTime? CompletedTo { get; set; }
        public double? MinAverage { get; set; }
        public double? MaxAverage { get; set; }
        public bool SortByDateDesc { get; set; } = true;
    }
    public class ListMySelfTestResultDto
    {
        public int ResultId { get; set; }
        public int SelfTestId { get; set; }
        public string SelfTestName { get; set; }
        public DateTime CompletedAt { get; set; }
        public double AverageScore { get; set; }
        public string? Description { get; set; }
        public string? ClientNote { get; set; }
    }
}

