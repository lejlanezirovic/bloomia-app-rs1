using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Client.Queries.List
{
    public sealed class ListClientsQueryDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
