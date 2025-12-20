using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Reimbit.Contracts.Security.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Reimbit.Application.Jwt;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings) => _jwtSettings = jwtSettings.Value;

    public string GenerateAccessToken(LoginInfo user)
    {
        var claims = new List<Claim>
        {
            new("userId", user.UserId.ToString() ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("orgId", user.OrganizationId.ToString()),
            new(ClaimTypes.Role, user.RoleId?.ToString() ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}