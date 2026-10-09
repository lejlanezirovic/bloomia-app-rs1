using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Articles.Commands.Update
{
    public class UpdateArticleCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
