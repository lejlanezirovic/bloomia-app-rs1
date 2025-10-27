using Bloomia.Domain.Entities.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Infrastructure.Database.Configurations
{
    public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSessionEntity>
    {
        public void Configure(EntityTypeBuilder<ChatSessionEntity> builder)
        {
            builder.ToTable("ChatSessions");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Messages)
                .WithOne(x => x.ChatSession)
                .HasForeignKey(x => x.ChatSessionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
