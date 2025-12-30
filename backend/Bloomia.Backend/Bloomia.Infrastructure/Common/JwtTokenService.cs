using Bloomia.Application.Abstractions;
using Bloomia.Shared.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bloomia.Infrastructure.Common;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _jwt;
    private readonly TimeProvider _time;
    private readonly IAppDbContext _context;

    public JwtTokenService(IOptions<JwtOptions> options, TimeProvider time, IAppDbContext context)
    {
        _jwt = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _time = time ?? throw new ArgumentNullException(nameof(time));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public JwtTokenPair IssueTokens(UserEntity user)
    {
        // Now from TimeProvider (consistent with the rest of the app)
        var nowInstant = _time.GetUtcNow();
        var nowUtc = nowInstant.UtcDateTime;
        var accessExpires = nowInstant.AddMinutes(_jwt.AccessTokenMinutes).UtcDateTime;
        var refreshExpires = nowInstant.AddDays(_jwt.RefreshTokenDays).UtcDateTime;

        // --- Claims (including jti/aud for standard compliance) ---
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier,   user.Id.ToString()),
            new(ClaimTypes.Email,            user.Email),
            new(ClaimTypes.Role, user.Role.RoleName),
            new Claim("fullname", user.Fullname),
            new("ver",         user.TokenVersion.ToString()),
            new(JwtRegisteredClaimNames.Iat, ToUnixTimeSeconds(nowInstant).ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Aud, _jwt.Audience)
        };

        //dodala
        if(user.Role.RoleName.Equals("Therapist", StringComparison.OrdinalIgnoreCase))
        {
            var therapist = _context.Therapists.FirstOrDefault(t => t.UserId == user.Id);

            if(therapist != null)
            {
                claims.Add(new Claim("therapistId", therapist.Id.ToString()));
            }
        }

        // --- Signature ---
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // --- access token (JWT) ---
        var jwt = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: nowUtc,
            expires: accessExpires,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        // --- refresh token (raw + hash) ---
        var refreshRaw = GenerateRefreshTokenRaw(64); // base64url
        var refreshHash = HashRefreshToken(refreshRaw); // base64url hash

        return new JwtTokenPair
        {
            AccessToken = accessToken,
            AccessTokenExpiresAtUtc = accessExpires,
            RefreshTokenRaw = refreshRaw,
            RefreshTokenHash = refreshHash,
            RefreshTokenExpiresAtUtc = refreshExpires
        };
    }

    public string HashRefreshToken(string rawToken)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawToken));
        // Use Base64Url to avoid problematic characters
        return Base64UrlEncoder.Encode(bytes);
    }

    private static string GenerateRefreshTokenRaw(int numBytes)
    {
        // Base64UrlEncoder from Microsoft.IdentityModel.Tokens (without + / =)
        var bytes = RandomNumberGenerator.GetBytes(numBytes);
        return Base64UrlEncoder.Encode(bytes);
    }

    private static long ToUnixTimeSeconds(DateTimeOffset dto) => dto.ToUnixTimeSeconds();
}