using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Admin
{
    public class ArticleEntity
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public AdminEntity Admin { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime PublishedAt { get; set; }=DateTime.UtcNow;

    }
}
