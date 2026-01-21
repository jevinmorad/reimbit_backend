using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Security.Account;
using Reimbit.Contracts.Account;

namespace Reimbit.Web.Controllers;

[ApiController]
[Route("api/Security/[controller]")]
public class AccountController(
    ICurrentUserProvider currentUserProvider,
    IAccountService accountService
) : ApiController(currentUserProvider)
{
    private static void AppendAuthCookies(HttpResponse response, string accessToken, string refreshToken)
    {
        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(3)
        };

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        response.Cookies.Append("access_token", accessToken, accessCookieOptions);
        response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
    }

    private static void ClearAuthCookies(HttpResponse response)
    {
        response.Cookies.Delete("access_token");
        response.Cookies.Delete("refresh_token");
    }

    [HttpPost("login")]
    [Produces<LoginResponse<LoginInfo>>]
    [EndpointSummary("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await accountService.Login(request);

        return result.Match(
            success =>
            {
                if (!string.IsNullOrWhiteSpace(success.AccessToken) &&
                    !string.IsNullOrWhiteSpace(success.RefreshToken))
                {
                    AppendAuthCookies(Response, success.AccessToken, success.RefreshToken);
                }

                return Ok(success);
            },
            Problem);
    }

    [HttpPost("register")]
    [Produces<LoginResponse<LoginInfo>>]
    [EndpointSummary("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await accountService.Register(request);

        return result.Match(
            success =>
            {
                if (!string.IsNullOrWhiteSpace(success.AccessToken) &&
                    !string.IsNullOrWhiteSpace(success.RefreshToken))
                {
                    AppendAuthCookies(Response, success.AccessToken, success.RefreshToken);
                }

                return Ok(success);
            },
            Problem);
    }

    [HttpPost("refresh-token")]
    [Produces<LoginResponse<LoginInfo>>]
    [EndpointSummary("Refresh token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await accountService.Refresh(request);

        return result.Match(
            success =>
            {
                if (!string.IsNullOrWhiteSpace(success.AccessToken) &&
                    !string.IsNullOrWhiteSpace(success.RefreshToken))
                {
                    AppendAuthCookies(Response, success.AccessToken, success.RefreshToken);
                }

                return Ok(success);
            },
            Problem);
    }

    [HttpPost("logout")]
    [Produces<OperationResponse<int>>]
    [EndpointSummary("Logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var result = await accountService.Logout(request);

        return result.Match(
            success =>
            {
                ClearAuthCookies(Response);
                return Ok(success);
            },
            Problem);
    }
}
