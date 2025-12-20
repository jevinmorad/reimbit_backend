using Common.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.Security.Account;
using Reimbit.Contracts.Security.Account;

namespace Reimbit.Web.Controller;

[ApiController]
[Route("api/Security/[controller]")]
public class AccountController(
    ICurrentUserProvider currentUserProvider,
    IAccountService accountService,
    IValidator<LoginRequest> loginRequestValidator,
    IValidator<RegisterRequest> registerRequestValidator
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Route("login")]
    [Produces<LoginResponse<LoginInfo>>]
    [EndpointSummary("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        await loginRequestValidator.ValidateAndThrowAsync(request);
        var result = await accountService.Login(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost]
    [Route("register")]
    [Produces<LoginResponse<LoginInfo>>]
    [EndpointSummary("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await registerRequestValidator.ValidateAndThrowAsync(request);
        var result = await accountService.Register(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }
}
