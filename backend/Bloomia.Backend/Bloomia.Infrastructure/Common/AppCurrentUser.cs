using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Bloomia.Application.Abstractions;

namespace Bloomia.Infrastructure.Common;

/// <summary>
/// Implementation of IAppCurrentUser that reads data from a JWT token.
/// </summary>
public sealed class AppCurrentUser(IHttpContextAccessor httpContextAccessor)
    : IAppCurrentUser
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public int? UserId =>  int.TryParse(_user?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public string? Email => _user?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated => _user?.Identity?.IsAuthenticated ?? false;

    public bool IsAdmin =>
        _user?.FindFirstValue(ClaimTypes.Role)?.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ?? false;

    public bool IsClient =>
        _user?.FindFirstValue(ClaimTypes.Role)?.Equals("CLIENT", StringComparison.OrdinalIgnoreCase) ?? false;

    public bool IsTherapist =>
        _user?.FindFirstValue(ClaimTypes.Role)?.Equals("THERAPIST", StringComparison.OrdinalIgnoreCase) ?? false;
}