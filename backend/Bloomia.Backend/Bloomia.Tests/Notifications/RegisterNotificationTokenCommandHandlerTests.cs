using Bloomia.Application.Modules.Notifications.Command;
using Bloomia.Domain.Entities.Notifications;
using Microsoft.Extensions.Time.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloomia.Tests.Notifications
{
    public class RegisterNotificationTokenCommandHandlerTests
    {
        private static DatabaseContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DatabaseContext(options, new FakeTimeProvider());
        }

        [Fact]
        public async Task Handle_ShouldCreateNewToken_WhenNoneExistsForUser()
        {
            using var context = GetInMemoryDbContext();
            var handler = new RegisterNotificationTokenCommandHandler(context);
            var command = new RegisterNotificationTokenCommand { Token = "abc123", UserId = 1 };

            await handler.Handle(command, CancellationToken.None);

            var saved = await context.NotificationTokens.FirstOrDefaultAsync(x => x.UserId == 1);
            Assert.NotNull(saved);
            Assert.True(saved!.IsActive);
            Assert.Equal("abc123", saved.Token);
        }

        [Fact]
        public async Task Handle_ShouldReactivateExistingToken_InsteadOfCreatingDuplicate()
        {
            using var context = GetInMemoryDbContext();
            context.NotificationTokens.Add(new NotificationTokenEntity
            {
                UserId = 1,
                Token = "abc123",
                IsActive = false
            });
            await context.SaveChangesAsync();

            var handler = new RegisterNotificationTokenCommandHandler(context);
            await handler.Handle(new RegisterNotificationTokenCommand { Token = "abc123", UserId = 1 }, CancellationToken.None);

            var all = await context.NotificationTokens.Where(x => x.UserId == 1).ToListAsync();
            Assert.Single(all);
            Assert.True(all[0].IsActive);
        }
    }
}
