using Reimbit.Contracts.Account;
using System.Security.Claims;

namespace Reimbit.Application.Jwt;

public interface IJwtTokenService
{
    public string GenerateAccessToken(LoginInfo user);
    public string GenerateRefreshToken();
}
