using Bloomia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class SavedTherapistsConfiguration : IEntityTypeConfiguration<SavedTherapistsEntity>
    {
        public void Configure(EntityTypeBuilder<SavedTherapistsEntity> builder)
        {
            builder.ToTable("SavedTherapists");

            builder.HasKey(x=>x.Id);

            builder.HasOne(x => x.Client)
                .WithMany(x => x.SavedTherapists)
                .HasForeignKey(st => st.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Therapist)
                .WithMany(x => x.SavedByClients)
                .HasForeignKey(st => st.TherapistId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
