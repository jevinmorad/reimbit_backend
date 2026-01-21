using AegisInt.Core;
using Common.Data.Models;
using Common.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reimbit.Application.EmployeeManagement.Employee;
using Reimbit.Contracts.Employee;
using Reimbit.Core.Common.Permissions;

namespace Reimbit.Web.Controllers;

[ApiController]
[Authorize]
[HasPermission(Permission.User_Manage)]
[Route("api/[controller]")]
public class EmployeeController(
    ICurrentUserProvider currentUserProvider,
    IEmployeeService employeeService
) : ApiController(currentUserProvider)
{
    [HttpPost]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Insert")]
    public async Task<IActionResult> Insert([FromBody] InsertEmployeeRequest request)
    {
        var result = await employeeService.Insert(request);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpPost("assign-manager")]
    [Produces<OperationResponse<EncryptedInt>>]
    [EndpointSummary("Assign manager to employees (bulk)")]
    public async Task<IActionResult> AssignManager([FromBody] AssignEmployeesToManagerRequest request)
    {
        var result = await employeeService.AssignEmployeesToManager(request);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet("{userId}")]
    [Produces<ViewEmployeeResponse>]
    [EndpointSummary("View employee details")]
    public async Task<IActionResult> View(EncryptedInt userId)
    {
        var result = await employeeService.View(userId);

        return result.Match(_ => Ok(result.Value), Problem);
    }

    [HttpGet]
    [Produces<PagedResult<ListEmployeeResponse>>]
    [EndpointSummary("List all employees")]
    public async Task<IActionResult> List()
    {
        var result = await employeeService.List();

        return result.Match(_ => Ok(result.Value), Problem);
    }
}
