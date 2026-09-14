using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Application.Modules.Articles.Commands.Create
{
    public class CreateArticleCommand : IRequest<int>
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
