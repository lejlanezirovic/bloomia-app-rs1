using Bloomia.Infrastructure.Database.Seeders;
using Bloomia.Infrastructure.Database;
using Bloomia.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bloomia.Infrastructure;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider services, IHostEnvironment env)
    {
        await using var scope = services.CreateAsyncScope();
        var ctx = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        if (env.IsTest())
        {
            await ctx.Database.EnsureCreatedAsync();
            await DynamicDataSeedersForBloomia.SeedAsync(ctx);
            return;
        }

        await ctx.Database.MigrateAsync();

        if (env.IsDevelopment())
        {
            await DynamicDataSeedersForBloomia.SeedAsync(ctx);
        }
    }
}