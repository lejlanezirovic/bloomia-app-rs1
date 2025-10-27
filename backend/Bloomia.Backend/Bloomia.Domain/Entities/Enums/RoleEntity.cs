using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Domain.Entities.Enums
{
    public class RoleEntity
    {
        public int Id { get; set; }
        
        public string RoleName { get; set; }


        /*
         
         Ako želiš da se u bazi čuva kao tekst (string) — što je čitljivije — onda u OnModelCreating (u DbContext klasi) možeš dodati:

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>()
                .Property(r => r.RoleName)
                .HasConversion<string>();
        }
         
         */

    }
}
