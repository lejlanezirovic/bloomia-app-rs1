using System.Security.Claims;
using Bloomia.Application.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace Bloomia.API.Auth;

public static class TokenVersionValidator
{
    public static async Task ValidateAsync(TokenValidatedContext context)
    {
        var principal = context.Principal;

        if (!int.TryParse(principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ||
            !int.TryParse(principal?.FindFirstValue("ver"), out var tokenVersion))
        {
            context.Fail("Invalid token.");
            return;
        }

        var db = context.HttpContext.RequestServices.GetRequiredService<IAppDbContext>();

        var currentVersion = await db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => (int?)u.TokenVersion)
            .FirstOrDefaultAsync(context.HttpContext.RequestAborted);

        if (currentVersion is null || currentVersion.Value != tokenVersion)
            context.Fail("Token is no longer valid.");
    }
}
