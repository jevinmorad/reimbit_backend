using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.UserManagement;
using Reimbit.Contracts.User;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController(
    ICurrentUserProvider currentUserProvider,
    IUserService userService
) : ApiController(currentUserProvider)
{
    [HttpGet("profile")]
    [Produces<UserResponse>]
    [EndpointSummary("Get current user profile")]
    public async Task<IActionResult> GetProfile()
    {
        var result = await userService.GetProfile();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPatch("profile")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update current user profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var result = await userService.UpdateProfile(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("permissions")]
    [Produces<PermissionsResponse>]
    [EndpointSummary("Get permission boolean map for current user")]
    public async Task<IActionResult> Permissions()
    {
        var result = await userService.GetPermissions();
        return result.Match(p => Ok(p.Permissions), Problem);
    }

    [HttpGet("info")]
    [Produces<InfoResponse>]
    [EndpointSummary("Get user info for current user")]
    public async Task<IActionResult> Info()
    {
        var result = await userService.GetInfo();
        return result.Match(_ => Ok(result.Value), Problem);
    }
}