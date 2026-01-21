using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.EmployeeManagement.Role;
using Reimbit.Contracts.Role;
using Reimbit.Core.Common.Permissions;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[HasPermission(Permission.Role_Manage)]
[Route("/api/[controller]")]
public class RoleController(
    IRoleService roleService,
    ICurrentUserProvider currentUserProvider
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Create role")]
    public async Task<IActionResult> Insert([FromBody] InsertRoleRequest request)
    {
        var result = await roleService.Insert(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPut]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Update role")]
    public async Task<IActionResult> Update(UpdateRoleRequest request)
    {
        var result = await roleService.Update(request);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpDelete("{roleId}")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Delete role")]
    public async Task<IActionResult> Delete(EncryptedInt roleId)
    {
        var result = await roleService.Delete(roleId);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("{roleId}")]
    [Produces<OperationResponse<GetRoleResponse>>]
    [EndpointSummary("Gel role")]
    public async Task<IActionResult> Get(EncryptedInt roleId)
    {
        var result = await roleService.Get(roleId);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("view/{roleId}")]
    [Produces<ViewRoleResponse>]
    [EndpointSummary("View role detil")]
    public async Task<IActionResult> View(EncryptedInt roleId)
    {
        var result = await roleService.View(roleId);
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet]
    [Produces<PagedResult<ListRoleResponse>>]
    [EndpointSummary("List roles")]
    public async Task<IActionResult> List()
    {

        var result = await roleService.List();
        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("assign")]
    [Produces<IActionResult>]
    [EndpointSummary("Assign a role")]
    public async Task<IActionResult> AssignRole([FromBody] UserRoleAssignmentRequest request)
    {
        var result = await roleService.AssignRoleToUser(request);

        if (result.IsError)
        {
            return Problem(detail: result.Errors[0].Description, statusCode: 400);
        }

        return Ok(result.Value);
    }
}