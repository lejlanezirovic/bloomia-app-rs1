using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Moods.Commands.Delete
{
    public sealed class DeleteMoodEntryCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
    }
}
