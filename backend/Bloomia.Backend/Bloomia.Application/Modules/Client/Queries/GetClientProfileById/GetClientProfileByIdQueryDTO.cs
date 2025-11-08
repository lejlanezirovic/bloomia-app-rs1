using Bloomia.Application.Modules.Journals.Commands;
using Bloomia.Application.Modules.SelfTests.Command;
using Bloomia.Domain.Entities.JournalsFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.GetClientProfileById
{
    public class GetClientProfileByIdQueryDTO
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string NativeLanguage { get; set; }
        public int Age { get; set; }
        public List<CreateJournalCommandDto> Journals { get; set; } = new List<CreateJournalCommandDto>();
        public List<SubmitSelfTestCommandDto> SelfTests { get; set; } = new List<SubmitSelfTestCommandDto>();
    }
}
