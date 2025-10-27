using Bloomia.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Admin
{
    public class AdminEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public List<ArticleEntity>? Articles { get; set; } = new List<ArticleEntity>();
    }
}
