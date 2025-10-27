using Bloomia.Domain.Entities.TherapistRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class TherapistsTherapyTypesConfiguration : IEntityTypeConfiguration<TherapistsTherapyTypesEntity>
    {
        public void Configure(EntityTypeBuilder<TherapistsTherapyTypesEntity> builder)
        {
            builder.ToTable("TherapistsTherapyTypes");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Therapist)
           .WithMany(t => t.MyTherapyTypesList)
           .HasForeignKey(x => x.TherapistId)
           .OnDelete(DeleteBehavior.NoAction);
          
            builder.HasOne(x => x.TherapyType)
                .WithMany(x=>x.TherapistsList)
                .HasForeignKey(x => x.TherapyTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
